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
    class KeepLevel6 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields


        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public KeepLevel6(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(180, 325);

            // Load the background
            Background = game.SpriteManager.KeepBackground;

        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(160, 276, 64, 128),
                game.L_Keep5Top,
                false));

            Doors.Add(new Door(
                new Rectangle(1000, 276, 64, 128),
                game.L_Keep7,
                false));
        }

        public override void CreateRestSite(Game1 game)
        {
            RestSites.Add(new RestSite(game.SpriteManager.Pixel,
                new Rectangle(800, 320, 40, 80)));
        }
        public override void Update(Game1 game)
        {

        }
        public override void DrawText(Game1 game)
        {
            // N/A
        }
    }
}
