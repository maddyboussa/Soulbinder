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
    class CastleLevel5Top : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        private double rockTimer;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevel5Top(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 126);

            // Load the background
            Background = game.SpriteManager.CastleBackground;

            // Make projectiles here not apply gravity
            DontApplyProjectileGravity = true;
            rockTimer = 0;
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(130, 68, 64, 142),
                game.L_Castle3,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 500, 64, 142),
                game.L_Castle6Top,
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
                    new Rectangle(1200, 200, 50, 50),
                    15, 10, 10, 4));

                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(1200, 400, 50, 50),
                    15, 10, 10, 4));


                rockTimer = 0;
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
