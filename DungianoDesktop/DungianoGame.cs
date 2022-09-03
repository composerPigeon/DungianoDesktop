using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using DungianoDesktop.Components.Scenes;

namespace DungianoDesktop
{

    public class DungianoGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private (int Width, int Height) _screenSize;

        public SpriteBatch SpriteBatch;
        public SceneManager SceneManager;
        public GameData GameData;
        public Brush Brush;

        public int LastLevel { get; }

        public DungianoGame()
        {
            _screenSize.Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _screenSize.Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            //setting graphicsDeviceManager
            _graphics = new GraphicsDeviceManager(this);
            (_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight) = _screenSize;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            LastLevel = 12;

            GameData = new GameData();
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            GameData.Load();

            Brush = new Brush(Color.Violet, GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            SceneManager = new SceneManager(this);
        }

        protected override void Update(GameTime gameTime)
        {
            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneManager.Draw();

            base.Draw(gameTime);
        }

        public void QuitGame()
        {
            GameData.Save();
            Exit();
        }

        public (int Width, int Height) GetScreenSize()
        {
            return _screenSize;
        }
    }

    public class GameData
    {
        public int MaxLevelVisited { get; set; }
        public string LastWeapon { get; set; }

        public GameData()
        {
            MaxLevelVisited = 1;
            LastWeapon = "Flute";
        }

        public void Save()
        {
            JsonSerializerOptions opt = new JsonSerializerOptions() { WriteIndented = true };
            string json = JsonSerializer.Serialize<GameData>(this, opt);

            File.WriteAllText("data.json", json);
        }

        public void Load()
        {
            if (File.Exists("data.json"))
            {
                using (StreamReader reader = new StreamReader("data.json"))
                {
                    string json = reader.ReadToEnd();
                    GameData data = (GameData)JsonSerializer.Deserialize(json, typeof(GameData));

                    MaxLevelVisited = data.MaxLevelVisited;
                    LastWeapon = data.LastWeapon;
                }
            }
        }
    }

    

    public class Brush
    {
        private Color _color;
        private Texture2D _texture;

        public Brush(Color color, GraphicsDevice graphicsDevice)
        {
            _color = color;
            _initTexture(graphicsDevice);
        }

        private void _initTexture(GraphicsDevice graphicsDevice)
        {
            _texture = new Texture2D(graphicsDevice, 1, 1);
            _texture.SetData(new Color[] { _color });
        }

        public Texture2D GetTexture()
        {
            return _texture;
        }
    }
}
