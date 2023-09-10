using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soulbinder.GameObjects;

namespace Soulbinder
{
    public class Door
    { 
        // FIELDS =======================================================================
        private Rectangle position;
        private Level levelToLoad;
        private bool locked;
        private bool displayHint;



        // PROPERTIES ===================================================================
        public Rectangle Position { get => position; set => position = value; }
        public Level LevelToLoad { get => levelToLoad; set => levelToLoad = value; }
        public bool Locked { get => locked; set => locked = value; }



        // CONSTRUCTORS =================================================================
        public Door(Rectangle position, Level levelToLoad, bool locked)
        {
            this.position = position;
            this.levelToLoad = levelToLoad;
            this.locked = locked;
            this.displayHint = false;
        }



        // METHODS ======================================================================
        /// <summary>
        /// Returns true if a new level has been loaded. Otherwise returns false.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool Update(Game1 game)
        {
            if (game.Player.Collides(position))
            {
                displayHint = true;

                // Check if player activates unlocked door
                if (game.GameManager.SingleKeyPress(Keys.F)
                    && !locked)
                {
                    // Save current positon as *this* level's checkpoint
                    game.CurrentLevel.CheckpointPosition = new Vector2(
                        game.Player.X,
                        game.Player.Y);

                    // Set new level
                    game.CurrentLevel = levelToLoad;

                    // Move player to *new* level's checkpoint.
                    game.Player.X = (int) game.CurrentLevel.CheckpointPosition.X;
                    game.Player.Y = (int)game.CurrentLevel.CheckpointPosition.Y;

                    return true;
                }
            }
            else
            {
                displayHint = false;
            }

            return false;

        }

        public void Draw(Game1 game)
        {
            if(locked)
            {
                game.SpriteBatch.Draw(
                    game.SpriteManager.Pixel,
                    new Rectangle(
                        position.X - game.Camera,
                        position.Y,
                        position.Width,
                        position.Height),
                    Color.Red);

                if (displayHint)
                {
                    game.SpriteBatch.DrawString(
                        game.SpriteManager.Arial16,
                        "This Door is Locked",
                        new Vector2(
                            (position.X - (position.Width)) - game.Camera,
                            position.Y - 50),
                        Color.White
                        );
                }
            }

            else
            {
                game.SpriteBatch.Draw(
                    game.SpriteManager.Pixel,
                    new Rectangle(
                        position.X - game.Camera,
                        position.Y,
                        position.Width,
                        position.Height),
                    Color.Green);

                if(displayHint)
                {
                    game.SpriteBatch.DrawString(
                        game.SpriteManager.Arial16,
                        "Press 'F' to Open Door",
                        new Vector2(
                            (position.X - (position.Width)) - game.Camera,
                            position.Y - 50),
                        Color.White
                        );
                }
            }
        }
    }
}
