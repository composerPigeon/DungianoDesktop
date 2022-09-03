using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DungianoDesktop.Components.Scenes;

namespace DungianoDesktop.Components.Characters
{
    public abstract class Enemy : HealthBody
    {
        protected int damage;

        public Enemy(DungianoGame dungianoGame, LevelScene scene, List<string> textureNames, int animationInterval, float scale, int health, int elapsedDamageTime, int speed, Vector2 position, int damage) :
            base(
                dungianoGame,
                scene,
                textureNames,
                animationInterval,
                scale,
                health,
                elapsedDamageTime,
                speed,
                position
                )
        {
            this.damage = damage;
            this.scene = scene;
        }

        protected override void die()
        {
            scene.RemoveComponent(this);
        }

        public int GetDamage()
        {
            return damage;
        }

        protected bool collideWithRoom()
        {
            foreach (Rectangle wall in scene.GetActualRoom().GetWalls())
            {
                if (sprite.GetSquare().Intersects(wall))
                    return true;
            }

            return false;
        }
    }
}

