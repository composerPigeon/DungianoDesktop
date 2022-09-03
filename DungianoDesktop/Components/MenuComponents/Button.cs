using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using DungianoDesktop.Components.Scenes;
using System;

namespace DungianoDesktop.Components.MenuComponents
{
    public enum ButtonMode
    {
        Active,
        Selected,
        Pressed,
        Disabled
    } 

    public abstract class Button : DrawableComponent
    {
        private ButtonSprite _sprite;

        private bool _disabled = false;

        //private float _scale = 0.5f;

        private ButtonState _previousState = ButtonState.Released; 

        protected DungianoGame dungianoGame;

        public Button(DungianoGame dungianoGame, (string, string, string, string) texturesNames, Vector2 position, float scale) : base()
        {
            this.dungianoGame = dungianoGame;
            _sprite = new ButtonSprite(texturesNames, scale, position);
        }

        // here we need to load textures
        public override void LoadContent()
        {
            _sprite.LoadTextures(dungianoGame.Content);
        }

        private void _updateButtonMode(ButtonMode newButtonMode)
        {
            _sprite.UpdateTexture(newButtonMode);
        }

        private bool isMouseColliding(Vector2 mousePos)
        {
            return _sprite.GetRectangle().Contains(mousePos.X, mousePos.Y);
        }

        protected abstract void onButtonClick();

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            ButtonState _actualState = mouseState.LeftButton;

            if (_disabled)
            {
                _updateButtonMode(ButtonMode.Disabled);
            }
            else
            {
                _updateButtonMode(ButtonMode.Active);

                if (isMouseColliding(new Vector2(mouseState.X, mouseState.Y)))
                {
                    _updateButtonMode(ButtonMode.Selected);

                    if (_actualState != _previousState)
                    {
                        if (_actualState == ButtonState.Pressed)
                            _updateButtonMode(ButtonMode.Pressed);
                        else
                        {
                           _updateButtonMode(ButtonMode.Selected);
                            onButtonClick();
                        }
                    }
                    else
                    {
                        if (_actualState == ButtonState.Pressed)
                            _updateButtonMode(ButtonMode.Pressed);
                        else
                            _updateButtonMode(ButtonMode.Selected);
                    }
                }
            }

            _previousState = _actualState;
        }

        public override void Draw()
        {
            _sprite.Draw(dungianoGame.SpriteBatch);
            //_sprite.Draw(dungianoGame.SpriteBatch, dungianoGame.Brush);
        }

        public void SetDisabled(bool value)
        {
            _disabled = value;
        }
    }

    public class LevelNavigationButton : Button
    {
        private int _level;

        public LevelNavigationButton(DungianoGame dungianoGame, (string, string, string, string) texturesNames, Vector2 position, float scale, int level) : base(dungianoGame, texturesNames, position, scale)
        {
            _level = level;
        }

        protected override void onButtonClick()
        {
            dungianoGame.SceneManager.ChangeToLevel(_level);
        }

    }

    public class MenuNavigationButton : Button
    {
        private SceneType _nextScene;

        public MenuNavigationButton(DungianoGame dungianoGame, (string, string, string, string) texturesNames, Vector2 position, float scale, SceneType nextScene) : base(dungianoGame, texturesNames, position, scale)
        {
            _nextScene = nextScene;
        }

        protected override void onButtonClick()
        {
            dungianoGame.SceneManager.ChangeToMenu(_nextScene);
        }
    }

    public class ClassicButton : Button
    {
        private Action _function;

        public ClassicButton(DungianoGame dungianoGame, (string, string, string, string) texturesNames, Vector2 position, float scale, Action function) : base(dungianoGame, texturesNames, position, scale)
        {
            _function = function;
        }

        protected override void onButtonClick()
        {
            _function();
        }
    }
}
