using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using DungianoDesktop.Components.Scenes;
using DungianoDesktop.Components.Map;
using DungianoDesktop.Components.Objects;
using DungianoDesktop.Components.MenuComponents;

namespace DungianoDesktop.Components.Characters
{
    public class Player : HealthBody
    {
        private bool _moving = false;

        private Itinerary _itinerary;

        private (int Damage, int ShootInterval) _weapon;

        private int _greyKeys = 1;
        private int _goldKeys = 1;
        private int _originalSpeed;


        private int _stamina;
        private double _elapsedShootTime;

        public Player(LevelScene scene, DungianoGame dungianoGame, Vector2 position) :
            base(
                dungianoGame,
                scene,
                textureNames: new List<string>(new string[] {"Characters/Player/01", "Characters/Player/02", "Characters/Player/03", "Characters/Player/04", "Characters/Player/05" }),
                animationInterval: 100,
                scale: 1f,
                health: 100,
                elapsedDamageTime: 300,
                speed: 5,
                position
                )
        {
            _itinerary = new Itinerary(dungianoGame, this.scene.GetLevel(), "Flute");
            _originalSpeed = speed;
            _stamina = 100;

            _setWeapon(scene.WeaponBank.GetWeapon(dungianoGame.GameData.LastWeapon));
        }

        public override void LoadContent()
        {
            _itinerary.LoadContent();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _moving = false;

            _elapsedShootTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            KeyboardState keyboardState = Keyboard.GetState();

            _move(keyboardState);

            _shoot(keyboardState, gameTime);

            _collisions(gameTime);

            _updateAnimation(gameTime);

            base.Update(gameTime);
        }

        #region Moving player
        //checks if the new position is valid and sets it
        private void _move(KeyboardState keyboardState)
        {
            Vector2 oldPosition = new Vector2(sprite.Position.X, sprite.Position.Y);
            _run(keyboardState);
            Vector2 dir = _getNewPosition(keyboardState);

            if (_moving && !collideWith(scene.GetActualRoom()))
            {
                sprite.Rotation = (float)(Math.Atan2(dir.Y, dir.X) + Math.PI / 2);
                direction = dir;
            }
            else
            {
                sprite.Position = oldPosition;
            }
            
        }

        //calculates new position based on input
        private Vector2 _getNewPosition(KeyboardState keyboardState)
        {
            Vector2 newDir = new Vector2(0, 0);

            if (keyboardState.IsKeyDown(Keys.W))
            {
                newDir.Y = -1;
                _moving = true;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                newDir.Y = 1;
                _moving = true;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                newDir.X = -1;
                _moving = true;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                newDir.X = 1;
                _moving = true;
            }

            newDir = Vector2.Normalize(newDir);

            sprite.Position += Vector2.Multiply(newDir, speed);

            if (newDir == Vector2.Zero)
            {
                newDir = direction;
            }
        
            return newDir;
        }

        private void _run(KeyboardState keyboardState)
        {
            speed = _originalSpeed;

            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                if (_stamina > 0)
                {
                    speed = _originalSpeed * 2;
                    _stamina -= 1;
                }
            }
            else
            {
                if (_stamina < 100)
                    _stamina += 1;
            }
            _itinerary.StaminaBar.SetValue(_stamina);
        }

        //animates sprite
        private void _updateAnimation(GameTime gameTime)
        {
            if (_moving)
            {
                sprite.Update(gameTime);
            }
            else
            {
                sprite.SetFrame(2);
            }
        }
        #endregion

        protected override void die()
        {
            dungianoGame.SceneManager.ChangeToMenu(SceneType.DieMenu);
            dungianoGame.SceneManager.RefreshLinkedList();
        }

        private void _shoot(KeyboardState keyboardState, GameTime gameTime)
        {

            if (_elapsedShootTime > _weapon.ShootInterval)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    scene.AddComponent(new Note(dungianoGame, scene, direction, sprite.Position, _weapon.Damage));
                    _elapsedShootTime = 0;
                }
            }
                
        }

        private void _collisions(GameTime gameTime)
        {
            foreach (Body body in scene.GetActualEntities())
            {
                if (collideWith(body))
                {

                    if (body.GetType() == typeof(Door))
                        _nextRoom((Door)body);
                    else if (body.GetType() == typeof(FinalDoor))
                        _nextLevel();
                    else if (body is CleanerLikeEnemy)
                        UpdateHealth((Enemy)body, gameTime, _itinerary.HealthBar);
                    else if (body.GetType() == typeof(Key))
                        _getKey((Key)body);
                    else if (body.GetType() == typeof(Gramophone))
                        _getGramophone((Gramophone)body, gameTime);
                    else if (body.GetType() == typeof(Weapon))
                        _getWeapon((Weapon)body);    
                }
            }
        }

        #region WeaponSetting mechanics
        private void _getWeapon(Weapon weapon)
        {
            _setWeapon(weapon);

            dungianoGame.GameData.LastWeapon = weapon.Name;

            scene.RemoveComponent(weapon);
        }

        private void _setWeapon(WeaponInfo weaponInfo)
        {
            _weapon = (weaponInfo.Damage, weaponInfo.ShootInterval);

            _itinerary.UpdateWeapon(weaponInfo.Name, 10000 / weaponInfo.ShootInterval + weaponInfo.Damage);
        }

        private void _setWeapon(Weapon weapon)
        {
            _weapon = (weapon.Damage, weapon.ShootInterval);

            _itinerary.UpdateWeapon(weapon.Name, 10000 / weapon.ShootInterval + weapon.Damage);
        }
        #endregion

        private void _getKey(Key key)
        {
            if (key.GetColor() == KeyColor.Grey)
            {
                _greyKeys += 1;
                _itinerary.UpdateKeys(key.GetColor());
            }
            if (key.GetColor() == KeyColor.Gold)
            {
                _goldKeys += 1;
                _itinerary.UpdateKeys(key.GetColor());
            }

            scene.RemoveComponent(key);
        }

        private void _getGramophone(Gramophone gramophone, GameTime gameTime)
        {
            UpdateHealth(-20, gameTime, _itinerary.HealthBar);
            scene.RemoveComponent(gramophone);
        }

        private void _nextRoom(Door door)
        {
            if (door.IsClosed())
            {
                if (_greyKeys > 0)
                {
                    scene.SetRoomTo(door.GetNumber());
                    _greyKeys -= 1;
                    _itinerary.UpdateKeys(KeyColor.Grey, -1);
                    door.Open();
                }
            }
            else
            {
                scene.SetRoomTo(door.GetNumber());
            }
        }

        private void _nextLevel()
        {
            if (_goldKeys > 0)
            {
                int nextLevel = scene.GetLevel() + 1;

                if (nextLevel <= dungianoGame.LastLevel)
                {
                    if (nextLevel > dungianoGame.GameData.MaxLevelVisited)
                        dungianoGame.GameData.MaxLevelVisited = nextLevel;
                    dungianoGame.SceneManager.ChangeToLevel(scene.GetLevel() + 1);
                }
                else
                    dungianoGame.SceneManager.ChangeToMenu(SceneType.FinalMenu);

                dungianoGame.SceneManager.RefreshLinkedList();
            }
                
        }

        public override void Draw()
        {
            _itinerary.Draw();
            base.Draw();
        }

        public Vector2 GetPosition()
        {
            return sprite.Position;
        }

        public void SetPosition(Vector2 position)
        {
            sprite.Position = position;
        }
    }
}

