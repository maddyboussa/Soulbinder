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
    class CastleLevel4Top : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        private double rockTimer;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevel4Top(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 452);

            // Load the background
            Background = game.SpriteManager.CastleBackground;

            // Make projectiles here not apply gravity
            DontApplyProjectileGravity = true;
            rockTimer = 200;
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(130, 398, 64, 128),
                game.L_Castle3,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 398, 64, 128),
                game.L_Castle5Top,
                false));

        }

        public override void Update(Game1 game)
        {
            // Spawn a new projectile at set intervals
            rockTimer -= game.ElapsedMilliseconds;

            if (rockTimer <= 0)
            {
                game.Player.ProjectileList = Projectiles;

                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(1200, 272, 50, 50),
                    12, 10, 10, 4));


                rockTimer = 500;
            }

            if (Projectiles.Count > 0)
            {
                // Move projectile left
                Projectiles[0].X -= Projectiles[0].Speed;
            }
        }
        public override void DrawText(Game1 game)
        {
            // N/A
        }
    }
}
