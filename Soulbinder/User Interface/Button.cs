using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Soulbinder
{
    [Flags]
    public enum Alignment
    {
        Center = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8
    }

    public class Button : UIElement
    {
        // Fields
        private SpriteFont buttonFont;
        private bool waitingInput;

        // Properties
        public Rectangle ButtonShape
        {
            get { return position; }
        }

        public bool WaitingInput
        {
            get { return waitingInput; }
            set { waitingInput = value; }
        }
        // Constructor
        public Button(Texture2D buttonTexture, SpriteFont buttonFont, Rectangle buttonRect) : base(buttonTexture, buttonRect)
        {
            this.buttonFont = buttonFont;
            waitingInput = false;
        }

        // Methods
        /// <summary>
        /// Check if the mouse is in the bounds of the Button's Rectangle
        /// </summary>
        /// <param name="ms">The MouseState to check</param>
        /// <returns>True if the mouse is within bounds, false if not</returns>
        public bool MouseInBounds(MouseState ms)
        {
            // Initialize variables
            bool inXBounds = false;
            bool inYBounds = false;

            // If the mouse x position is within the button's x bounds, set inXBounds to true
            if (ms.X < position.Location.X + position.Width && ms.X > position.Location.X)
            {
                inXBounds = true;
            }

            // If the mouse y position is within the button's y bounds, set inYBounds to true
            if (ms.Y < position.Location.Y + position.Height && ms.Y > position.Location.Y)
            {
                inYBounds = true;
            }

            // If the mouse position is within both bounds, return true
            if (inXBounds && inYBounds)
            {
                return true;
            }
            else // Otherwise, return false
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the button has been clicked
        /// </summary>
        /// <param name="ms">The MouseState to compare</param>
        /// <returns>A bool representing if the button has been clicked or not</returns>
        public bool Clicked(MouseState ms)
        {
            if (MouseInBounds(ms) && ms.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else return false;

        }

        /// <summary>
        /// Draw the button with aligned text in it
        /// </summary>
        /// <param name="sb">The SpriteBatch to draw from</param>
        /// <param name="text">The text to put in the button</param>
        /// <param name="bounds">The rectangle of the button</param>
        /// <param name="align">Where to align the text within the button</param>
        /// <param name="boxColor">The color of the button box</param>
        /// <param name="textColor">The color of the button text</param>
        public void Draw(SpriteBatch sb, string text, Rectangle bounds, Alignment align, Color boxColor, Color textColor)
        {
            sb.Draw(sprite, position, boxColor);

            // Create centralized location
            Vector2 size = buttonFont.MeasureString(text);
            Vector2 positionVec = new Vector2(bounds.Center.X, bounds.Center.Y);
            Vector2 origin = size * 0.5f;

            // Check alignments
            if (align.HasFlag(Alignment.Left))
            {
                origin.X += bounds.Width / 2 - size.X / 2;
            }

            if (align.HasFlag(Alignment.Right))
            {
                origin.X -= bounds.Width / 2 - size.X / 2;
            }

            if (align.HasFlag(Alignment.Top))
            {
                origin.Y += bounds.Height / 2 - size.Y / 2;
            }

            if (align.HasFlag(Alignment.Bottom))
            {
                origin.Y -= bounds.Height / 2 - size.Y / 2;
            }

            // Draw the text within the box
            sb.DrawString(buttonFont, text, positionVec, textColor, 0, origin, 1, SpriteEffects.None, 0);
        }
    }
}
