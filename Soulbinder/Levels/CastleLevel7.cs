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
    class CastleLevel7 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevel7(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(120, 536);

            // Load the background
            Background = game.SpriteManager.CastleBackground;

            // Spawn enemy
            // Load Enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(1120, 175, 32, 32),
               0, 1, 1, 1));
            Enemies[0].DoNothing = true;

            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(322, 452, 32, 32),
               0, 1, 1, 1));
            Enemies[1].DoNothing = true;

            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(75, 100, 32, 32),
               0, 1, 1, 1));
            Enemies[2].DoNothing = true;

            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(986, 575, 32, 32),
               0, 1, 1, 1));
            Enemies[3].DoNothing = true;

        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(100, 518, 64, 128),
                game.L_Castle6Top,
                true));

            Doors.Add(new Door(
                new Rectangle(1100, 518, 64, 128),
                game.L_Castle8,
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
            
        }
    }
}
