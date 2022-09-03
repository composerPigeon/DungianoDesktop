using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DungianoDesktop.Components.MenuComponents;
using DungianoDesktop.Components.Scenes;
using DungianoDesktop.Components.Map;

namespace DungianoDesktop.Components.Characters
{
    public abstract class KinematicBody : Body
    {
        protected int speed;
        protected AnimatedSprite sprite;
        protected LevelScene scene;
        protected Vector2 direction;

        public KinematicBody(DungianoGame dungianoGame, LevelScene scene, List<string> textureNames, int animationInterval, float scale, int speed, Vector2 position) : base(dungianoGame)
        {
            this.speed = speed;
            this.scene = scene;
            direction = new Vector2(0, -1);

            sprite = new AnimatedSprite(textureNames, scale, animationInterval, position, rotation: 0f);
        }

        public override void LoadContent()
        {
            sprite.LoadTextures(dungianoGame.Content);
        }

        public override Rectangle GetCollisionRectangle()
        {
            return sprite.GetSquare();
        }

        public override void Draw()
        {
            sprite.Draw(dungianoGame.SpriteBatch);
            //sprite.Draw(dungianoGame.SpriteBatch, dungianoGame.Brush);
        }

        protected bool collideWith(Room room)
        {
            foreach (Rectangle wall in room.GetWalls())
            {
                if (sprite.GetSquare().Intersects(wall))
                    return true;
            }
            return false;
        }

        protected bool collideWith(Body body)
        {
            return GetCollisionRectangle().Intersects(body.GetCollisionRectangle());
        }

    }

    public abstract class HealthBody : KinematicBody
    {
        protected int health;
        private double _elapsedDamageTime;
        private int _damageInterval;

        public HealthBody(DungianoGame dungianoGame, LevelScene scene, List<string> textureNames, int animationInterval, float scale, int health, int elapsedDamageTime, int speed, Vector2 position) :
            base(
                dungianoGame,
                scene,
                textureNames,
                animationInterval,
                scale,
                speed,
                position)
        {
            this.health = health;
            _elapsedDamageTime = elapsedDamageTime;
            _damageInterval = 300;
        }

        #region UpdateHealth
        #region UpdateHealthBody
        public void UpdateHealth(Enemy enemy, GameTime gameTime)
        {

            if (_elapsedDamageTime > _damageInterval)
            {
                health -= enemy.GetDamage();
                _elapsedDamageTime = 0;
            }
                
            if (health <= 0)
                die();
        }

        public void UpdateHealth(Enemy enemy, GameTime gameTime, Bar bar)
        {
            if (_elapsedDamageTime > _damageInterval)
            {
                health -= enemy.GetDamage();
                bar.UpdateValue(enemy.GetDamage());
                _elapsedDamageTime = 0;
            }

            if (health <= 0)
                die();
        }
        #endregion

        #region UpdateHelthDamage
        public void UpdateHealth(int damage, GameTime gameTime)
        {

            if (_elapsedDamageTime > _damageInterval)
            {
                health -= damage;
                _elapsedDamageTime = 0;
            }

            if (health <= 0)
                die();
        }

        public void UpdateHealth(int damage, GameTime gameTime, Bar bar)
        {
            if (_elapsedDamageTime > _damageInterval)
            {
                health -= damage;
                bar.UpdateValue(damage);
                _elapsedDamageTime = 0;
            }

            if (health <= 0)
                die();
        }
        #endregion
        #endregion

        public override void Update(GameTime gameTime)
        {
            _elapsedDamageTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        protected abstract void die();
    }
}

