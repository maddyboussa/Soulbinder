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
    class KeepLevel4Top : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields


        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public KeepLevel4Top(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 462);

            // Load the background
            Background = game.SpriteManager.KeepBackground;

            // Add keys
            // Spawn Enemies
            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(430, 640, 32, 32),
               0, 1, 1, 1));
            Enemies[0].DoNothing = true;

            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(930, 640, 32, 32),
               0, 1, 1, 1));
            Enemies[1].DoNothing = true;

            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(700, 192, 32, 32),
               0, 1, 1, 1));
            Enemies[2].DoNothing = true;

            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(120, 126, 32, 32),
               0, 1, 1, 1));
            Enemies[3].DoNothing = true;

        }

        public override void CreateRestSite(Game1 game)
        {
            RestSites.Add(new RestSite(game.SpriteManager.Pixel,
                new Rectangle(500, 400, 40, 80)));
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(120, 392, 64, 128),
                game.L_Keep3,
                true));

            Doors.Add(new Door(
                new Rectangle(900, 392, 64, 128),
                game.L_Keep5Top,
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
            // N/A
        }
    }
}
