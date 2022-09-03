using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components
{
    public abstract class Component
    {

        public Component()
        {
        }

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);
    }

    public abstract class DrawableComponent : Component
    {
        public DrawableComponent() : base()
        {
        }

        public abstract void Draw();
    }
}

