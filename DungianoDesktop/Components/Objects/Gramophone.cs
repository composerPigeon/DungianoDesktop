using System;

using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components.Objects
{
    public class Gramophone : StaticBody
    {
        public Gramophone(DungianoGame dungianoGame, Vector2 position) : base(dungianoGame)
        {
            sprite = new Sprite("Objects/gramophone", scale: 1f, position);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}

