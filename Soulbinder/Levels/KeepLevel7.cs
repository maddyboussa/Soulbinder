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
    class KeepLevel7 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        int wave;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public KeepLevel7(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(235, 552);

            // Load the background
            Background = game.SpriteManager.KeepBackground;

            // Set wave
            wave = 1;

        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(225, 516, 64, 128),
                game.L_Keep6,
                true));

            Doors.Add(new Door(
                new Rectangle(1700, 436, 64, 128),
                game.L_Keep8,
                true));

        }

        public override void CreateRestSite(Game1 game)
        {
            RestSites.Add(new RestSite(game.SpriteManager.Pixel,
                new Rectangle(800, 320, 40, 80)));
        }
        public override void Update(Game1 game)
        {
            if (wave == 1 && Enemies.Count == 0)
            {
                // Add Enemy
                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(865, 532, 32, 64),
                2, 15, 15, 200));

                wave++;
            }
            else if (wave == 2 && Enemies.Count == 0)
            {
                // Add Enemy
                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(665, 352, 32, 64),
                2, 15, 15, 200));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(1065, 352, 32, 64),
                2, 15, 15, 200));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(865, 252, 32, 64),
                2, 15, 15, 200));

                wave++;
            }
            else if (wave == 3 && Enemies.Count == 0)
            {
                // Add Enemy
                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(665, 352, 32, 64),
                2, 15, 15, 200));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(480, 325, 32, 64),
                2, 15, 15, 50));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(1065, 352, 32, 64),
                2, 15, 15, 200));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(1380, 325, 32, 64),
                2, 15, 15, 50));

                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(865, 252, 32, 64),
                2, 15, 15, 200));

                // Add Enemy
                Enemies.Add(new Skeleton(
                game.SpriteManager.SkeletonSprite,
                new Rectangle(865, 532, 32, 64),
                2, 15, 15, 200));

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
