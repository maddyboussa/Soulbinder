using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soulbinder.GameObjects;

namespace Soulbinder
{
    public class RestSite
    {
        // Fields
        private Texture2D sprite;
        private Rectangle position;
        private bool displayLabel;

        // Properties
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        public Rectangle Position { get { return position; } set { position = value; } }

        // Constructor
        public RestSite(Texture2D sprite, Rectangle position)
        {
            this.sprite = sprite;
            this.position = position;
            displayLabel = false;
        }

        /// <summary>
        /// Update the Rest Site
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool Update(Game1 game)
        {
            // Check if the player is in range of the unlockable
            if (game.Player.Collides(position))
            {
                // Display the label
                displayLabel = true;

                // Check if the player unlocks the unlockable by pressing F
                if (game.GameManager.SingleKeyPress(Keys.F))
                {
                    // Restore player health and mana
                    game.Player.CurrentHealth = game.Player.MaximumHealth;
                    game.Player.CurrentMana = game.Player.MaximumMana;
                    game.UIManager.HealthBar.Update(game.Player.MaximumHealth);
                    game.UIManager.ManaBar.Update(game.Player.MaximumMana);

                    return true;
                }
            }
            else
            {
                displayLabel = false;
            }

            return false;
        }

        /// <summary>
        /// Draw the Rest SIte
        /// </summary>
        /// <param name="game"></param>
        public void Draw(Game1 game)
        {
            // Always draw the rest site
            game.SpriteBatch.Draw(sprite,
                    new Rectangle(position.X - game.Camera, position.Y, position.Width, position.Height),
                    Color.White);

            if (displayLabel)
            {
                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    "Press 'F' to Rest",
                    new Vector2(
                        (position.X - (position.Width)) - game.Camera,
                        position.Y - 50),
                    Color.White
                    );
            }
        }
    }
}
