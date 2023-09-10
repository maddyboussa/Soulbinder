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
    class CastleLevel5Bottom : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        int wave;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevel5Bottom(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(120, 525);

            // Load the background
            Background = game.SpriteManager.CastleBackground;

            // Spawn enemy
            // Load Enemy
            wave = 1;

        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(100, 518, 64, 128),
                game.L_Castle4Bottom,
                true));

            Doors.Add(new Door(
                new Rectangle(1100, 518, 64, 128),
                game.L_Castle6Bottom,
                true));
        }


        public override void Update(Game1 game)
        {
            if (wave == 1 && Enemies.Count == 0)
            {
                // Add Enemy
                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(500, 532, 32, 64),
                2, 10, 10, 200));

                wave++;
            }
            else if (wave == 2 && Enemies.Count == 0)
            {
                // Add Enemy
                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(150, 352, 32, 64),
                2, 10, 10, 200));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(850, 352, 32, 64),
                2, 10, 10, 200));

                wave++;
            }
            else if (wave == 3 && Enemies.Count == 0)
            {
                // Add Enemy
                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(500, 232, 32, 64),
                2, 10, 10, 200));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(150, 352, 32, 64),
                2, 10, 10, 200));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(850, 352, 32, 64),
                2, 10, 10, 200));

                wave++;
            }
            if (wave == 4 && Enemies.Count == 0)
            {
                // Open the doors
                Doors[0].Locked = false;
                Doors[1].Locked = false;
            }
        }


        public override void DrawText(Game1 game)
        {
            // N/A
        }
    }
}
