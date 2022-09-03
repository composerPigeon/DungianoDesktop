using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungianoDesktop.Components.MenuComponents
{
    public class Bar
    {
        private int _maxValue;
        private int _value;
        private Color _valueColor;
        private Rectangle _sizeRectangle;

        private Texture2D _background;
        private Texture2D _textureValue;

        private DungianoGame _dungianoGame;

        public Bar(DungianoGame dungianoGame, Color color, Rectangle sizeRectangle)
        {
            _dungianoGame = dungianoGame;

            _valueColor = color;
            _maxValue = 100;
            _value = _maxValue;

            _sizeRectangle = sizeRectangle;
        }

        public void LoadTextures()
        {
            _textureValue = new Texture2D(_dungianoGame.GraphicsDevice, 1, 1);
            _textureValue.SetData(new Color[] { _valueColor });

            _background = new Texture2D(_dungianoGame.GraphicsDevice, 1, 1);
            _background.SetData(new Color[] { Color.Black });
        }

        public void SetValue(int value)
        {
            _value = value;

            if (_value > _maxValue)
                _value = _maxValue;
        }

        public void UpdateValue(int decrease)
        {
            _value -= decrease;
            if (_value < 0)
                _value = 0;
            else if (_value > _maxValue)
                _value = _maxValue;
        }

        public void Draw()
        {
            _dungianoGame.SpriteBatch.Begin();
            _dungianoGame.SpriteBatch.Draw(_background, _sizeRectangle, Color.White);
            _dungianoGame.SpriteBatch.Draw(_textureValue, _getValueRectangle(), Color.White);
            _dungianoGame.SpriteBatch.End();
        }

        private Rectangle _getValueRectangle()
        {
            return new Rectangle(_sizeRectangle.X, _sizeRectangle.Y, _sizeRectangle.Width * _value / _maxValue, _sizeRectangle.Height);
        }
    }
}

