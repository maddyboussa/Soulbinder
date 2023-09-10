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
    class DungeonLevel7Top : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        double rockDropTimer;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel7Top(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 400);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            // Load enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(725, 375, 32, 64),
               2, 15, 15, 200));

            rockDropTimer = 700;

            Name = "Level 7 Top";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 352, 64, 128),
                game.L_Dungeon6Top,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 352, 64, 128),
                game.L_Dungeon8,
                false));
        }

        public override void Update(Game1 game)
        {
            // Spawn a new projectile at set intervals
            rockDropTimer -= game.ElapsedMilliseconds;

            if (rockDropTimer <= 0)
            {
                game.Player.ProjectileList = Projectiles;

                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(318, 74, 50, 50),
                    5, 10, 10, 4));

                rockDropTimer = 700;
            }
        }

        public override void DrawText(Game1 game)
        {
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Explore to Find New Spells!",
                new Vector2(450 - game.Camera, 260),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "(Spells open up new gameplay options)",
                new Vector2(380 - game.Camera, 290),
                Color.Gray);
        }
    }
}
