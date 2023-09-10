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
    class KeepLevel9 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields


        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public KeepLevel9(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(75, 236);

            // Load the background
            Background = game.SpriteManager.KeepBackground;

        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(65, 192, 64, 128),
                game.L_Keep8,
                false));

            Doors.Add(new Door(
                new Rectangle(2425, 552, 64, 128),
                game.L_KeepBoss,
                false));

        }

        public override void CreateRestSite(Game1 game)
        {
            RestSites.Add(new RestSite(game.SpriteManager.Pixel,
                new Rectangle(2100, 600, 40, 80)));
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
