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
    class KeepLevel4Bottom : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields


        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public KeepLevel4Bottom(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(250, 452);

            // Load the background
            Background = game.SpriteManager.KeepBackground;

            // Add enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(500, 432, 32, 64),
               2, 15, 15, 200));
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(240, 392, 64, 128),
                game.L_Keep3,
                false));

            Doors.Add(new Door(
                new Rectangle(960, 392, 64, 128),
                game.L_Keep5Bottom,
                false));
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
