using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    public class UIElement
    {
        // Fields
        protected Texture2D sprite;
        protected Rectangle position;

        // Properties
        public Rectangle Position
        {
            get { return position; }
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
        public UIElement(Texture2D sprite, Rectangle position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        // Methods
        public void Update(Player player)
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, position, Color.White);
        }
    }
}
