using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Soulbinder
{
    /// <summary>
    /// represents  texture2D that will be animated over multiple frames
    /// this class handles the animation
    /// </summary>
    class AnimatedTexture
    {
        //fields
        private SpriteBatch sb;
        private GameTime gameTime;

        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;

        // properties

        // constructor
        public AnimatedTexture(GameTime gameTime, SpriteBatch sb)
        {
            this.gameTime = gameTime;
            this.sb = sb;

            // Set up animation stuff
            currentFrame = 1;
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
        }

        // methods

        /// <summary>
        /// updates the animation of a sprite sheet texture,
        /// would be called in a class' Update() method
        /// returns the current frame of animation, to be then used in Draw()
        /// </summary>
        /// <param name="texture">texture to animate</param>
        /// <param name="numSpritesInSheet">number of frames in its animation</param>
        public Rectangle UpdateAnimation(Texture2D texture, int numSpritesInSheet)
        {
            // number of disctinct frames in animation (length of sprite sheet)
            int numFrames = numSpritesInSheet;
            int widthOfSingleSprite = texture.Width / numFrames;

            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame
                currentFrame++;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }

            return new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, texture.Height);
        }


    }
}
