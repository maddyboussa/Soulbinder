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
    class DungeonLevel6Top : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        private double rockDropTimer;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel6Top(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 454);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            rockDropTimer = 500;

            Name = "Level 6 Top";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 392, 64, 128),
                game.L_Dungeon5,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 392, 64, 128),
                game.L_Dungeon7Top,
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
                    new Rectangle(396, 132, 50, 50),
                    5, 10, 10, 4));
                Projectiles.Add(new Projectile(
                    game.SpriteManager.RockSprite,
                    new Rectangle(878, 132, 50, 50),
                    5, 10, 10, 4));

                rockDropTimer = 500;
            }

        }

        public override void DrawText(Game1 game)
        {
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Press 'ESC' to Pause",
                new Vector2(555 - game.Camera, 260),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "(Pressing 'ESC' on a Menu closes the Game)",
                new Vector2(450 - game.Camera, 290),
                Color.Gray);
        }
    }
}
