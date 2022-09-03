using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungianoDesktop.Components.Scenes
{
    public abstract class GameScene
    {
        protected DrawableComponentDatabase drawableComponents = new DrawableComponentDatabase();
        protected ComponentDatabase components = new ComponentDatabase();

        protected DungianoGame dungianoGame;

        public GameScene PreviousScene;

        // public GameScene Previous;

        public GameScene(DungianoGame dungianoGame)
        {
            this.dungianoGame = dungianoGame;
        }

        public virtual void Update(GameTime gameTime)
        {

            foreach (DrawableComponent drawableComponent in new List<DrawableComponent>(drawableComponents.GetData()))
            {
                drawableComponent.Update(gameTime);
            }

            foreach (Component component in new List<Component>(components.GetData()))
            {
                component.Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach (DrawableComponent drawableComponent in new List<DrawableComponent>(drawableComponents.GetData()))
            {
                drawableComponent.Draw();
            }

        }

        protected abstract void addComponents();
    }
}
