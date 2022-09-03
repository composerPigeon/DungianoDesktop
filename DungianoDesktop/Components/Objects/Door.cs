using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components.Objects
{

    public class Door : StaticBody
    {
        private bool _close;

        private int _number;

        public Door(DungianoGame dungianoGame, (string Closed, string Opened) texturesNames, Vector2 position, float rotation, int number, bool close = true) : base(dungianoGame) 
        {
            sprite = new RotableSprite(new List<string>(new string[] { texturesNames.Closed, texturesNames.Opened }), scale: 0.5f, position, rotation);

            _number = number;
            _close = close;
        }

        public int GetNumber()
        {
            return _number;
        }

        public bool IsClosed()
        {
            return _close;
        }

        public void Open()
        {
            _close = false;
        }

        private void _updateSprite()
        {
            if (_close)
                sprite.SetFrame(0);
            else
                sprite.SetFrame(1);

        }

        public override void Update(GameTime gameTime)
        {
            _updateSprite();
        }
    }

    public class FinalDoor : Door
    {
        public FinalDoor(DungianoGame dungianoGame, Vector2 position) :
            base(
                dungianoGame,
                texturesNames: ("Objects/finalDoorClose", "Objects/doorOpen"),
                position,
                rotation: (float)Math.PI/2,
                number: -1
                )
        {
        }
    }
}

