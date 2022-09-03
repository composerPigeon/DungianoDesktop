using System;
using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components.Objects
{
    public enum KeyColor
    {
        Grey,
        Gold
    }

    public class Key : StaticBody
    {
        private KeyColor _color;

        public Key(DungianoGame dungianoGame, KeyColor keyColor, Vector2 position) : base(dungianoGame)
        {
            _color = keyColor;

            sprite = new Sprite((_color == KeyColor.Grey) ? "Objects/greyKey" : "Objects/goldKey", scale: 1f, position);
        }

        public KeyColor GetColor()
        {
            return _color;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}

