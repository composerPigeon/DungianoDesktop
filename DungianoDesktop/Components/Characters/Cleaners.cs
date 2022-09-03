using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DungianoDesktop.Components.Scenes;
using DungianoDesktop.Components.Objects;

namespace DungianoDesktop.Components.Characters
{
    public enum CleanerItem
    {
        Empty,
        GreyKey,
        GoldKey,
        Gramophone,
        Weapon
    }

    public class CleanerLikeEnemy : Enemy
    {
        protected Random random;
        protected CleanerItem item;
        protected WeaponInfo weaponToSpawn;

        #region Constructors
        public CleanerLikeEnemy(DungianoGame dungianoGame, LevelScene scene, List<string> textureNames, int animationInterval, float scale, int health, int speed, Vector2 position, int damage, CleanerItem item) :
            base(
                dungianoGame,
                scene,
                textureNames,
                animationInterval,
                scale,
                health,
                elapsedDamageTime: 100,
                speed,
                position,
                damage
                )
        {
            this.item = item;
            random = new Random();
        }

        public CleanerLikeEnemy(DungianoGame dungianoGame, LevelScene scene, List<string> textureNames, int animationInterval, float scale, int health, int speed, Vector2 position, int damage, CleanerItem item, WeaponInfo weapon) :
            base(
                dungianoGame,
                scene,
                textureNames,
                animationInterval,
                scale,
                health,
                elapsedDamageTime: 100,
                speed,
                position,
                damage
                )
        {
            this.item = item;
            random = new Random();
            weaponToSpawn = weapon;
        }
        #endregion

        protected virtual void move()
        {
            Vector2 oldPosition = new Vector2(sprite.Position.X, sprite.Position.Y);
            sprite.Position += Vector2.Multiply(direction, speed);

            if (collideWith(scene.GetActualRoom()))
            {
                direction = _generateRandomNormalizeVector();
                sprite.Rotation = (float)(Math.Atan2(direction.Y, direction.X) + Math.PI / 2);
                sprite.Position = oldPosition;
            }
        }

        private Vector2 _generateRandomNormalizeVector()
        {
            double angle = 360.0 * random.NextDouble();

            return Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
        }

        protected void spawnKey(KeyColor keyColor)
        {
            scene.AddComponent(new Key(dungianoGame, keyColor, sprite.Position));
        }

        protected void spawnGramophone()
        {
            scene.AddComponent(new Gramophone(dungianoGame, sprite.Position));
        }

        protected void spawnWeapon()
        {
            if (weaponToSpawn.Name != "")
                scene.AddComponent(new Weapon(dungianoGame, weaponToSpawn.Damage, weaponToSpawn.ShootInterval, weaponToSpawn.Name, weaponToSpawn.TextureName, sprite.Position));
        }

        protected override void die()
        {
            switch (item)
            {
                case CleanerItem.Empty:
                    base.die();
                    break;
                case CleanerItem.GoldKey:
                    spawnKey(KeyColor.Gold);
                    base.die();
                    break;
                case CleanerItem.GreyKey:
                    spawnKey(KeyColor.Grey);
                    base.die();
                    break;
                case CleanerItem.Gramophone:
                    spawnGramophone();
                    base.die();
                    break;
                case CleanerItem.Weapon:
                    spawnWeapon();
                    base.die();
                    break;
                default:
                    base.die();
                    break;
            }
        }

        protected void collisions(GameTime gameTime)
        {
            foreach (Body body in scene.GetActualRoom().GetEntities())
            {
                if (collideWith(body))
                {
                    if (body.GetType() == typeof(Note))
                    {
                        UpdateHealth((Enemy)body, gameTime);
                        scene.RemoveComponent(body);
                    }
                }

            }
        }

        public override void Update(GameTime gameTime)
        {
            move();

            collisions(gameTime);

            sprite.Update(gameTime);

            base.Update(gameTime);
        }
    }


    public class Cleaner : CleanerLikeEnemy
    {
        #region Constructors
        public Cleaner(DungianoGame dungianoGame, LevelScene scene, Vector2 position, CleanerItem item) :
            base(
                dungianoGame,
                scene,
                textureNames: new List<string>(new string[] { "Characters/Cleaner/01", "Characters/Cleaner/02" }),
                animationInterval: 100,
                scale: 1f,
                health: 40,
                speed: 5,
                position,
                damage: 10,
                item
                )
        {
        }

        public Cleaner(DungianoGame dungianoGame, LevelScene scene, Vector2 position, WeaponInfo weapon) :
            base(
                dungianoGame,
                scene,
                textureNames: new List<string>(new string[] { "Characters/Cleaner/01", "Characters/Cleaner/02" }),
                animationInterval: 100,
                scale: 1f,
                health: 40,
                speed: 5,
                position,
                damage: 10,
                item: CleanerItem.Weapon,
                weapon
                )
        {
        }
        #endregion
    }

    public class FatCleaner : CleanerLikeEnemy
    {
        #region Constructors
        public FatCleaner(DungianoGame dungianoGame, LevelScene scene, Vector2 position, CleanerItem item) :
            base(
                dungianoGame,
                scene,
                textureNames: new List<string>(new string[] { "Characters/FatCleaner/01", "Characters/FatCleaner/02" }),
                animationInterval: 100,
                scale: 1f,
                health: 80,
                speed: 5,
                position,
                damage: 10,
                item
                )
        {
        }

        public FatCleaner(DungianoGame dungianoGame, LevelScene scene, Vector2 position, WeaponInfo weapon) :
            base(
                dungianoGame,
                scene,
                textureNames: new List<string>(new string[] { "Characters/FatCleaner/01", "Characters/FatCleaner/02" }),
                animationInterval: 100,
                scale: 1f,
                health: 80,
                speed: 5,
                position,
                damage: 10,
                item: CleanerItem.Weapon,
                weapon
                )
        {
        }
        #endregion
    }

    public class FollowingCleaner : CleanerLikeEnemy
    {
        #region Constructors
        public FollowingCleaner(DungianoGame dungianoGame, LevelScene scene, Vector2 position, CleanerItem item) :
            base(
                dungianoGame,
                scene,
                textureNames: new List<string>(new string[] { "Characters/FollowingCleaner/01", "Characters/FollowingCleaner/02" }),
                animationInterval: 100,
                scale: 1f,
                health: 100,
                speed: 5,
                position,
                damage: 10,
                item
                )
        {
        }

        public FollowingCleaner(DungianoGame dungianoGame, LevelScene scene, Vector2 position, WeaponInfo weapon) :
            base(
                dungianoGame,
                scene,
                textureNames: new List<string>(new string[] { "Characters/FollowingCleaner/01", "Characters/FollowingCleaner/02" }),
                animationInterval: 100,
                scale: 1f,
                health: 100,
                speed: 5,
                position,
                damage: 10,
                item: CleanerItem.Weapon,
                weapon
                )
        {
        }
        #endregion

        protected override void move()
        {
            if (!_collideWithVisionSpace())
                base.move();
            else
                _followPlayer();

        }

        private void _followPlayer()
        {
            Vector2 oldPosition = new Vector2(sprite.Position.X, sprite.Position.Y);

            direction = Vector2.Normalize(scene.GetPlayer().GetPosition() - sprite.Position);
            sprite.Position += Vector2.Multiply(direction, speed);
            sprite.Rotation = (float)(Math.Atan2(direction.Y, direction.X) + Math.PI / 2);

            if (collideWith(scene.GetActualRoom()))
                sprite.Position = oldPosition;
        }

        private Rectangle _getVisionSpace()
        {
            Rectangle rectangle = GetCollisionRectangle();

            return new Rectangle(rectangle.X - rectangle.Width, rectangle.Y - rectangle.Height, rectangle.Width * 3, rectangle.Height * 3);
        }

        private bool _collideWithVisionSpace()
        {
            return scene.GetPlayer().GetCollisionRectangle().Intersects(_getVisionSpace());
        }
    }
}

