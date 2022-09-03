using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DungianoDesktop.Components.MenuComponents;
using Microsoft.Xna.Framework.Input;

namespace DungianoDesktop.Components.Scenes
{
    public class MenuScene : GameScene
    {
        private Background _background;
        private List<Button> _buttons;

        public MenuScene(DungianoGame dungianoGame, List<Button> buttons, Background background) : base(dungianoGame)
        {
            _background = background;
            _buttons = buttons;

            addComponents();
        }

        protected override void addComponents()
        {
            drawableComponents.Add(_background);
            foreach (Button button in _buttons)
            {
                drawableComponents.Add(button);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


    }
}
