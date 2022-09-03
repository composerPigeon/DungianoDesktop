using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using DungianoDesktop.Components.Map;
using DungianoDesktop.Components.Characters;
using DungianoDesktop.Components.Objects;




namespace DungianoDesktop.Components.Scenes
{
    public class LevelScene : GameScene
    {
        private List<Room> _map;
        private int _actualRoom;
        private Player _player;
        private Vector2 _spawnPlayerPosition;

        private int _level;

        private LevelBuilder _levelBuilder;

        public WeaponBank WeaponBank = new WeaponBank();

        public LevelScene(DungianoGame dungianoGame, int level) : base(dungianoGame)
        {
            _level = level;
            _levelBuilder = new LevelBuilder(dungianoGame, this);
            _map = _levelBuilder.BuildMap(level);
            _player = _levelBuilder.CreatePlayer();
            _spawnPlayerPosition = _levelBuilder.GetPlayerSpawnPosition();
            _actualRoom = 0;

            addComponents();
        }

        protected override void addComponents()
        {
            _addRoomComponents();
            _player.SetPosition(_spawnPlayerPosition);
            drawableComponents.Add(_player);
        }

        private void _addRoomComponents()
        {
            foreach (DrawableComponent drawableComponent in GetActualRoom().GetComponents())
            {
                drawableComponents.Add(drawableComponent);
            }
        }

        public void SetRoomTo(int door)
        {
            drawableComponents.Clear();
            _actualRoom = door;
            _addRoomComponents();
            _player.SetPosition(_spawnPlayerPosition);
            drawableComponents.Add(_player);
        }

        public void AddComponent(Component component)
        {
            if (component is DrawableComponent)
            {
                drawableComponents.Add((DrawableComponent)component);

                if (component is Body)
                    GetActualRoom().AddEntity((Body)component);
            }
            else
                components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            if (component is DrawableComponent)
            {
                drawableComponents.Remove((DrawableComponent)component);

                if (component is Body)
                    GetActualRoom().RemoveEntity((Body)component);
            }
            else
                components.Remove(component);
        }

        public List<Body> GetActualEntities()
        {
            return GetActualRoom().GetEntities();
        }

        public Room GetActualRoom()
        {
            return _map[_actualRoom];
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape))
                _pause();

            base.Update(gameTime);
        }

        private void _pause()
        {
            dungianoGame.SceneManager.ChangeToMenu(SceneType.PauseMenu);
        }

        public int GetLevel()
        {
            return _level;
        }

        public Player GetPlayer()
        {
            return _player;
        }
    }
}

