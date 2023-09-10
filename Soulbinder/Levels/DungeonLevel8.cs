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
    class DungeonLevel8 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        double rockDropTimer1;
        double rockDropTimer2;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel8(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 450);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            // Spawn enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(800, 350, 32, 64),
               2, 15, 15, 375));

            rockDropTimer1 = 800;
            rockDropTimer2 = 1200;

            Name = "Level 8";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 392, 64, 128),
                game.L_Dungeon7Top,
                false));

            Doors.Add(new Door(
                new Rectangle(2250, 352, 64, 128),
                game.L_Dungeon9,
                false));
        }

        public override void Update(Game1 game)
        {
            // Spawn a new projectile at set intervals
            rockDropTimer1 -= game.ElapsedMilliseconds;
            rockDropTimer2 -= game.ElapsedMilliseconds;

            if (rockDropTimer1 <= 0)
            {
                game.Player.ProjectileList = Projectiles;

                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(836, 74, 50, 50),
                    5, 10, 10, 4));
                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(1116, 74, 50, 50),
                    5, 10, 10, 4));
                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(1796, 132, 50, 50),
                    5, 10, 10, 4));

                rockDropTimer1 = 800;
            }

            if (rockDropTimer2 <= 0)
            {
                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(1594, 132, 50, 50),
                    5, 10, 10, 4));
                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(2040, 132, 50, 50),
                    5, 10, 10, 4));

                rockDropTimer2 = 800;
            }
        }

        public override void DrawText(Game1 game)
        {
            // N/A
            base.DrawText(game);
        }
    }
}
