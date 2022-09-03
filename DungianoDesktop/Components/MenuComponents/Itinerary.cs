using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using DungianoDesktop.Components.Objects;

namespace DungianoDesktop.Components.MenuComponents
{
    public class Itinerary
    {
        public Bar HealthBar;
        public Bar WeaponBar;
        public Bar StaminaBar;

        private (int Width, int Height) _size;

        private int _greyKeys;
        private int _goldKeys;
        private string _weapon;

        private DungianoGame _dungianoGame;
        private SpriteFont _spriteFont;
        private int _levelNumber;

        private int _itineraryStartY;

        private int _firstLine;
        private int _secondLine;
        private int _barLen;
        private int _lineHeight;
        private int _textWidth;

        private int _firstColumn;
        private int _secondColumn;
        private int _thirdColumn;

        public Itinerary(DungianoGame dungianoGame, int level, string weapon)
        {
            _itineraryStartY = (dungianoGame.GetScreenSize().Height * 8) / 9;
            _size = (dungianoGame.GetScreenSize().Width, dungianoGame.GetScreenSize().Height / 9);
            _firstLine = _size.Height / 3 + _itineraryStartY;
            _secondLine = _size.Height * 2 / 3 + _itineraryStartY;
            _barLen = 300;
            _lineHeight = 25;
            _textWidth = 200;
            _firstColumn = 15;
            _secondColumn = 650;
            _thirdColumn = 1200;

            HealthBar = new Bar(dungianoGame, Color.Green, new Rectangle(_firstColumn + _textWidth, _firstLine, _barLen, _lineHeight));
            WeaponBar = new Bar(dungianoGame, Color.Blue, new Rectangle(_firstColumn + _textWidth, _secondLine, _barLen, _lineHeight));
            StaminaBar = new Bar(dungianoGame, Color.Yellow, new Rectangle(_secondColumn + _textWidth, _secondLine, _barLen, _lineHeight));

            _goldKeys = 0;
            _greyKeys = 1;
            _weapon = weapon;

            _dungianoGame = dungianoGame;
            _levelNumber = level;

            
        }

        public void LoadContent()
        {
            _spriteFont = _dungianoGame.Content.Load<SpriteFont>("xugglybug");

            HealthBar.LoadTextures();
            WeaponBar.LoadTextures();
            StaminaBar.LoadTextures();
        }

        //add number of keys of some keyColor
        public void UpdateKeys(KeyColor keyColor, int number=1)
        {
            if (keyColor == KeyColor.Grey)
            {
                _greyKeys += number;
            }
            else if (keyColor == KeyColor.Gold)
            {
                _goldKeys += number;
            }
        }

        public void UpdateWeapon(string weapon, int weaponStats)
        {
            _weapon = weapon;
            WeaponBar.SetValue(weaponStats);
        }

        private void _drawString(string text, Vector2 position, int size)
        {
            _dungianoGame.SpriteBatch.Begin();
            _dungianoGame.SpriteBatch.DrawString(_spriteFont, text, position, Color.Black, 0f, new Vector2(0,0), (float)size/50f, SpriteEffects.None, 0);
            _dungianoGame.SpriteBatch.End();
        }

        private void _drawLevel()
        {
            _drawString("Level: " + _levelNumber.ToString(), new Vector2(_secondColumn, _firstLine), _lineHeight);
        }

        private void _drawKeys()
        {
            _drawString("Grey Keys: " + _greyKeys.ToString(), new Vector2(_thirdColumn, _firstLine), _lineHeight);
            _drawString("Gold Keys: " + _goldKeys.ToString(), new Vector2(_thirdColumn, _secondLine), _lineHeight);
        }

        private void _drawBars()
        {
            _drawString("Health Points: ", new Vector2(_firstColumn, _firstLine), _lineHeight);
            HealthBar.Draw();
            _drawString( _weapon + ": ", new Vector2(_firstColumn, _secondLine), _lineHeight);
            WeaponBar.Draw();
            _drawString("Stamina: ", new Vector2(_secondColumn, _secondLine), _lineHeight);
            StaminaBar.Draw();
        }

        public void Draw()
        {
            _drawLevel();
            _drawKeys();
            _drawBars();
        }
    }
}

