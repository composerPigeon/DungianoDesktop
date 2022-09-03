using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using DungianoDesktop.Components.MenuComponents;


namespace DungianoDesktop.Components
{

    public class Sprite
    {
        protected List<string> texturesNames = new List<string>();
        protected List<Texture2D> textures = new List<Texture2D>();

        protected int frame = 0;
        protected float scale;
        protected int width;
        protected int height;

        public Vector2 Position;

        public Sprite(List<string> texturesNames, float scale, Vector2 position)
        {
            this.scale = scale;
            this.texturesNames = texturesNames;
            Position = position;
        }

        public Sprite(string textureName, float scale, Vector2 position)
        {
            this.scale = scale;
            texturesNames.Add(textureName);
            Position = position;
        }

        public void LoadTextures(ContentManager content)
        {
            foreach (string textureName in texturesNames)
            {
                textures.Add(content.Load<Texture2D>(textureName));
            }

            setWidthHeight();
        }

        public Texture2D GetTexture()
        {
            return textures[frame];
        }

        // returns rectangle of texture depends on scale
        public Rectangle GetRectangle() 
        {
            int positionX = (int)Position.X - width / 2;
            int positionY = (int)Position.Y - height / 2;

            return new Rectangle(positionX, positionY, width, height);

        }

        // returns collision square for kinematic bodies
        public Rectangle GetSquare()
        {
            int diameter = (width + height) / 2;

            int positionX = (int)Position.X - diameter / 2;
            int positionY = (int)Position.Y - diameter / 2;

            return new Rectangle(positionX, positionY, diameter, diameter);
        }

        public void SetFrame(int frame)
        {
            if (frame > -1 && frame < textures.Count)
            {
                this.frame = frame;

                setWidthHeight();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GetTexture(), GetRectangle(), Color.White);
            spriteBatch.End();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Brush brush)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GetTexture(), GetRectangle(), Color.White);
            spriteBatch.Draw(brush.GetTexture(), GetRectangle(), Color.White);
            //spriteBatch.Draw(brush.GetTexture(), GetAvarageRectangle(), Color.White);
            spriteBatch.End();
        }

        protected virtual void setWidthHeight()
        {
            width = (int)(GetTexture().Width * scale);
            height = (int)(GetTexture().Height * scale);
        }
    }

    public class RotableSprite : Sprite
    {
        protected Vector2 origin = new Vector2();
        public float Rotation;

        public RotableSprite(List<string> texturesNames, float scale, Vector2 position, float rotation) : base(texturesNames, scale, position)
        {
            Rotation = rotation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GetTexture(), Position, null, Color.White, Rotation, origin, scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public override void Draw(SpriteBatch spriteBatch, Brush brush)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GetTexture(), Position, null, Color.White, Rotation, origin, scale, SpriteEffects.None, 0);
            //spriteBatch.Draw(brush.GetTexture(), GetRectangle(), Color.White);
            spriteBatch.Draw(brush.GetTexture(), GetSquare(), Color.White);
            spriteBatch.End();
        }

        protected override void setWidthHeight()
        {
            base.setWidthHeight();
            origin.X = width / 2;
            origin.Y = height / 2;
        }
    }

    public class ButtonSprite : Sprite
    {
        public ButtonSprite((string Active, string Selected, string Pressed, string Disabled) texturesNames, float scale, Vector2 position) : base(new List<string>(
            new string[] { texturesNames.Active, texturesNames.Selected, texturesNames.Pressed, texturesNames.Disabled }), scale, position)
        { }

        public void UpdateTexture(ButtonMode buttonMode)
        {
            switch(buttonMode)
            {
                case ButtonMode.Active:
                    SetFrame(0);
                    break;
                case ButtonMode.Selected:
                    SetFrame(1);
                    break;
                case ButtonMode.Pressed:
                    SetFrame(2);
                    break;
                case ButtonMode.Disabled:
                    SetFrame(3);
                    break;
            }
        }
    }

    public class AnimatedSprite : RotableSprite
    {
        private int _updateInterval;
        private double _elapsed;

        public AnimatedSprite(List<string> texturesNames, float scale, int updateInterval, Vector2 position, float rotation) : base(texturesNames, scale, position, rotation)
        {
            _updateInterval = updateInterval;
        }

        private void _updateFrame()
        {
            SetFrame((frame += 1) % textures.Count);
        }

        public void Update(GameTime gameTime)
        {

            _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_updateInterval < _elapsed)
            {
                _updateFrame();
                _elapsed = 0;
            }
        }
    }
}

