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
    class CastleLevel3 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevel3(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(120, 600);

            // Load the background
            Background = game.SpriteManager.CastleBackground;

            // Spawn enemy
            // Load Enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(402, 352, 32, 64),
               2, 15, 15, 75));

            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(626, 492, 32, 64),
               2, 15, 15, 125));
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(100, 552, 64, 128),
                game.L_Castle2,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 192, 64, 128),
                game.L_Castle4Bottom,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 38, 64, 128),
                game.L_Castle4Top,
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
