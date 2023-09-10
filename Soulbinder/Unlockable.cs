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
    public class Unlockable
    {
        // Fields
        private Texture2D sprite;
        private Rectangle position;
        private Spell spellToUnlock;
        private bool unlocked;
        private bool displayLabel;

        // Properties
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        public Rectangle Position { get { return position; } set { position = value; } }
        public Spell Spell { get { return spellToUnlock; } set { spellToUnlock = value; } }
        public bool Unlocked { get { return unlocked; } set { unlocked = value; } }

        // Constructor
        public Unlockable(Texture2D sprite, Rectangle position, Spell spellToUnlock)
        {
            this.sprite = sprite;
            this.position = position;
            this.spellToUnlock = spellToUnlock;
            unlocked = false;
            displayLabel = false;
        }

        public bool Update(Game1 game)
        {
            // Check if the player is in range of the unlockable
            if (game.Player.Collides(position))
            {
                // Display the label
                displayLabel = true;

                // Check if the player unlocks the unlockable by pressing F
                if (game.GameManager.SingleKeyPress(Keys.F) && !unlocked)
                {
                    // Unlock the unlockable
                    unlocked = true;

                    // Unlock the spell
                    spellToUnlock.Unlocked = true;

                    return true;
                }
            } else
            {
                displayLabel = false;
            }

            return false;
        }

        public void Draw(Game1 game)
        {
            // If the unlockable has not been unlocked, draw the unlockable 
            // and the appropriate labels
            if (!unlocked)
            {
                game.SpriteBatch.Draw(sprite,
                    new Rectangle(position.X - game.Camera, position.Y, position.Width, position.Height),
                    Color.White);

                if (displayLabel)
                {
                    game.SpriteBatch.DrawString(
                        game.SpriteManager.Arial16,
                        "Press 'F' to Unlock " + spellToUnlock.Name,
                        new Vector2(
                            (position.X - (sprite.Width/2) + 50) - game.Camera,
                            position.Y - 50),
                        Color.White
                        );
                }
            }
        }
    }
}
