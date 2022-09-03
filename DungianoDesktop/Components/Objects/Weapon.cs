using System;

using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components.Objects
{
    public class Weapon : StaticBody
    {
        public int Damage;
        public int ShootInterval;
        public string Name;

        public Weapon(DungianoGame dungianoGame, int damage, int shootInterval, string name, string textureName, Vector2 position) : base(dungianoGame)
        {
            Damage = damage;
            ShootInterval = shootInterval;
            Name = name;

            sprite = new Sprite(textureName, scale: 1f, position);
        }

        public override void Update(GameTime gameTime)
        {
        }

    }
}

