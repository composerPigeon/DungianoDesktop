using System;

using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components.Objects
{
    public abstract class StaticBody : Body
    {
        protected Sprite sprite;

        public StaticBody(DungianoGame dungianoGame) : base(dungianoGame)
        {
        }

        public override void LoadContent()
        {
            sprite.LoadTextures(dungianoGame.Content);
        }

        public override void Draw()
        {
            sprite.Draw(dungianoGame.SpriteBatch);
        }

        public override Rectangle GetCollisionRectangle()
        {
            return sprite.GetRectangle();
        }

    }
}

