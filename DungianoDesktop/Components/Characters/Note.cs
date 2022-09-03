using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DungianoDesktop.Components.Scenes;

namespace DungianoDesktop.Components.Characters
{
    public class Note : Enemy
    {

        public Note(DungianoGame dungianoGame, LevelScene scene, Vector2 direction, Vector2 position, int damage) :
            base(
                dungianoGame,
                scene,
                textureNames: new List<string>(new string[] { "Objects/note" }),
                animationInterval: 0,
                scale: 1f,
                health: 1,
                elapsedDamageTime: 100,
                speed: 10,
                position,
                damage
                )
        {
            this.direction = direction;
        }

        public override void Update(GameTime gameTime)
        {
            _move();
        }

        private void _move()
        {
            if (!_collidewithRoom())
            {
                sprite.Position += Vector2.Multiply(direction, speed);
            }
            else
                die();
        }

        private bool _collidewithRoom()
        {
            foreach (Rectangle wall in scene.GetActualRoom().GetWalls())
            {
                if (GetCollisionRectangle().Intersects(wall))
                    return true;
            }

            return false;
        }

    }
}

