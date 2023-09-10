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
    class CastleLevel6Top : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevel6Top(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(150, 418);

            // Load the background
            Background = game.SpriteManager.CastleBackground;

            // Spawn enemy
            // Load Enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(1082, 372, 32, 32),
               0, 1, 1, 1));

            Enemies[0].DoNothing = true;

        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 352, 64, 128),
                game.L_Castle5Top,
                true));

            Doors.Add(new Door(
                new Rectangle(1700, 352, 64, 128),
                game.L_Castle7,
                true));
        }

        public override void Update(Game1 game)
        {
            if (Enemies.Count == 0)
            {
                Doors[0].Locked = false;
                Doors[1].Locked = false;
            }
        }
        public override void DrawText(Game1 game)
        {
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Some Doors can be Unlocked with Keys",
                new Vector2(930 - game.Camera, 300),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "(Hit a Key to Activate It)",
                new Vector2(980 - game.Camera, 330),
                Color.DarkGray);
        }
    }
}
