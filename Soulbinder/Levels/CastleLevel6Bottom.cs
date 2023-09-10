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
    class CastleLevel6Bottom : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevel6Bottom(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(280, 212);

            // Load the background
            Background = game.SpriteManager.CastleBackground;

            // Spawn enemy


        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(270, 152, 64, 128),
                game.L_Castle5Bottom,
                false));

        }

        public override void CreateUnlockable(Game1 game)
        {
            Unlockables.Add(new Unlockable(
                game.SpriteManager.FireDashSprite,
                new Rectangle(2350, 582, game.Player.FireDash.SpellRect.Width, game.Player.FireDash.SpellRect.Height),
                game.Player.FireDash));
        }

        public override void Update(Game1 game)
        {

        }

        public override void DrawText(Game1 game)
        {
            if (game.Player.FireDash.Unlocked)
            {
                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    "Press 'SHIFT' to Cast Fire Dash",
                    new Vector2(2000 - game.Camera, 300),
                    Color.White);

                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    "(Speed Boost and Deals Damage)",
                    new Vector2(2000 - game.Camera, 330),
                    Color.DarkGray);
            }
        }
    }
}
