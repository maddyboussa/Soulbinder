using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    class Slider : UIElement
    {
        // Fields
        private float sliderValue;
        private bool beingHeld;
        private Rectangle originalPosition;
        private float originalSliderValue;

        // Properties

        public float SliderValue
        {
            get { return sliderValue; }
            set { sliderValue = value; }
        }

        public bool BeingHeld
        {
            get { return beingHeld; }
            set { beingHeld = value; }
        }

        // Constructor
        public Slider(Texture2D sliderTexture, Rectangle sliderRect, float sliderValue) : base(sliderTexture, sliderRect)
        {
            this.sliderValue = sliderValue;
            beingHeld = false;
            originalPosition = sliderRect;
            originalSliderValue = sliderValue;
        }

        // Methods
        /// <summary>
        /// Draw the slider with a specific color
        /// </summary>
        /// <param name="sb">The SpriteBatch to draw from</param>
        /// <param name="color">The color of the slider</param>
        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(sprite, position, color);
        }

        /// <summary>
        /// Check if the mouse is in the bounds of the Slider's Rectangle
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
        /// Move the slider
        /// </summary>
        /// <param name="ms">The MouseState to move the Slider with</param>
        /// <param name="boundLeft">The left bound of the slider</param>
        /// <param name="boundRight">The right bound of the slider</param>
        public void Move(MouseState ms, int boundLeft, int boundRight)
        {
            // Create a vector based on the mouse position
            Vector2 mousePosition = new Vector2(ms.X, ms.Y);

            // Add it to the slider x value
            X = (int)mousePosition.X;

            // Ensure the slider does not pass the bounds
            if(X < boundLeft)
            {
                X = boundLeft;
            }
            if(X > boundRight)
            {
                X = boundRight;
            }

            // Set slider bounds
            float sliderWidth = boundRight - boundLeft;
            float artificialMouse = ms.X - boundLeft;

            // If the mouse is past the left bound, make it 0
            if (ms.X < boundLeft)
            {
                artificialMouse = 0;
            }

            // If the mouse is past the right bound, make it the sliderWidth
            if (ms.X > boundRight)
            {
                artificialMouse = sliderWidth;
            }

            // Set slider value
            sliderValue = 1.0f * (artificialMouse / sliderWidth);
        }

        public bool MouseHeld(MouseState ms, MouseState prevMS)
        {
            // Check if both the last state and the current state are both pressing the left mouse button
            if (ms.LeftButton == ButtonState.Pressed && prevMS.LeftButton == ButtonState.Pressed)
            {
                // If so, return true
                return true;
            }
            // Otherwise, return false
            else return false;
        }

        public void SliderHeld(MouseState ms, MouseState prevMS, GraphicsDeviceManager gdm)
        {
            if (MouseInBounds(ms) && MouseHeld(ms, prevMS))
            {
                BeingHeld = true;
            }

            if (BeingHeld == true && MouseHeld(ms, prevMS))
            {
                Move(ms,
                    (gdm.PreferredBackBufferWidth / 2) - (600 / 2), (gdm.PreferredBackBufferWidth / 2) + (600 / 2));
            }

            if (!MouseHeld(ms, prevMS))
            {
                BeingHeld = false;
            }
        }

        /// <summary>
        /// Reset the Slider to it's original position and values
        /// </summary>
        public void Reset()
        {
            position = originalPosition;
            sliderValue = originalSliderValue;
        }
    }
}
