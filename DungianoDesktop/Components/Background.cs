using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungianoDesktop.Components
{
    public class Background : DrawableComponent
    {
        private string _textureName;
        private Texture2D _texture;
        private DungianoGame _dungianoGame;
        private (int Width, int Height) _screenSize;

        private Rectangle _backgroundRectangle;

        public Background(DungianoGame dungianoGame, string textureName) : base()
        {
            _textureName = textureName;
            _dungianoGame = dungianoGame;
            _screenSize = dungianoGame.GetScreenSize();
            _backgroundRectangle = new Rectangle(0, 0, _screenSize.Width, _screenSize.Height);
        }

        public override void Update(GameTime gameTime) { }

        public override void LoadContent()
        {
            _texture = _dungianoGame.Content.Load<Texture2D>(_textureName);
        }

        public override void Draw()
        {
            _dungianoGame.SpriteBatch.Begin();
            _dungianoGame.SpriteBatch.Draw(_texture, _backgroundRectangle, Color.White);
            _dungianoGame.SpriteBatch.End();

            /*_dungianoGame.SpriteBatch.Begin();
            _dungianoGame.SpriteBatch.Draw(_dungianoGame.Brush.GetTexture(), new Rectangle(0, 0, _screenSize.Width, 50), Color.White);
            _dungianoGame.SpriteBatch.Draw(_dungianoGame.Brush.GetTexture(), new Rectangle(0, 0, 50, _screenSize.Height), Color.White);
            _dungianoGame.SpriteBatch.Draw(_dungianoGame.Brush.GetTexture(), new Rectangle(0, _screenSize.Height - 50, _screenSize.Width, 50), Color.White);
            _dungianoGame.SpriteBatch.Draw(_dungianoGame.Brush.GetTexture(), new Rectangle(_screenSize.Width - 50, 0, 50, _screenSize.Height), Color.White);
            _dungianoGame.SpriteBatch.End();*/
        }
    }
}
