using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Soulbinder
{
    public class StatusBar : UIElement
    {
        // Fields
        int currentValue;
        int maximumValue;
        SpriteFont font;

        // Properties
        public int CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        public int MaximumValue
        {
            get { return maximumValue; }
            set { maximumValue = value; }
        }

        // Constructor
        public StatusBar(Texture2D sprite, Rectangle position, int currentValue, int maximumValue, SpriteFont font) : base(sprite, position)
        {
            this.currentValue = currentValue;
            this.maximumValue = maximumValue;
            this.font = font;
        }

        // Methods
        /// <summary>
        /// Update the healthbar based on a single value
        /// </summary>
        /// <param name="currentValue">The value to update the healthbar to match</param>
        public void Update(int currentValue)
        {
            this.currentValue = currentValue;
        }

        /// <summary>
        /// Update the healthbar based on a current and maximum value
        /// </summary>
        /// <param name="currentValue">The value to update the healthbar to match</param>
        public void Update(int currentValue, int maximumValue)
        {
            this.maximumValue = maximumValue;
            this.currentValue = currentValue;
        }

        /// <summary>
        /// Update the healthbar based on multiple values
        /// </summary>
        /// <param name="value1">The first value</param>
        /// <param name="value2">The second value</param>
        /// <param name="value3">The third value</param>
        public void Update(int value1, int value2, int value3)
        {
            this.CurrentValue = value1 + value2 + value3;
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
        public void Draw(SpriteBatch sb, string text, Rectangle bounds, Alignment align, Color colorMax, Color colorCurrent, Color textColor)
        {
            // Draw maximum box
            sb.Draw(sprite, position, colorMax);

            // Create second bar that overlays on top with a percentage equal to currentValue / maximumValue
            Rectangle currentValueBar = position;
            currentValueBar.Width = position.Width * currentValue / maximumValue;

            // Draw current box
            sb.Draw(sprite, currentValueBar, colorCurrent);

            // Create centralized location
            Vector2 size = font.MeasureString(text);
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
            sb.DrawString(font, text, positionVec, textColor, 0, origin, 1, SpriteEffects.None, 0);
        }
    }
}
