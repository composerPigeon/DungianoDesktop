using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components
{
    public abstract class Body : DrawableComponent
    {
        protected DungianoGame dungianoGame;

        public Body(DungianoGame dungianoGame) : base()
        {
            this.dungianoGame = dungianoGame;
        }

        public abstract Rectangle GetCollisionRectangle();
    }
}

