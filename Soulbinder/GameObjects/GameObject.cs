using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    public abstract class GameObject
    {
        // Fields
        protected Texture2D sprite;
        protected Rectangle position;
        protected Rectangle originalPosition;

        // Properties
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public int X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public int Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        // Constructor
        public GameObject(Texture2D sprite, Rectangle position)
        {
            this.sprite = sprite;
            this.position = position;
            originalPosition = position;
        }

        // Methods

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, new Rectangle(X, Y, position.Width, position.Height), Color.White);
        }
        public virtual void Draw(SpriteBatch sb, int camX)
        {
            sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
        }
        public virtual void Draw(SpriteBatch sb, int camX, Color color)
        {
            sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), color);
        }
    }
}
