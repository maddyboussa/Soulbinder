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
    class CastleLevel2 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevel2(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(120, 452);

            // Load the background
            Background = game.SpriteManager.CastleBackground;

            // Spawn enemy
            // Load Enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(464, 300, 32, 64),
               2, 15, 15, 175));
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(100, 392, 64, 128),
                game.L_Castle1,
                false));

            Doors.Add(new Door(
                new Rectangle(1750, 232, 64, 128),
                game.L_Castle3,
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
