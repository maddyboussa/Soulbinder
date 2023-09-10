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
    class DungeonLevel9 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        double rockDropTimer;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel9(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 475);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            rockDropTimer = 600;

            Name = "Level 9";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 432, 64, 128),
                game.L_Dungeon8,
                false));

            Doors.Add(new Door(
                new Rectangle(1700, 392, 64, 128),
                game.L_DungeonBoss,
                false));
        }

        public override void CreateRestSite(Game1 game)
        {
            RestSites.Add(new RestSite(game.SpriteManager.Pixel,
                new Rectangle(1300, 442, 40, 80)));
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
                    new Rectangle(876, 40, 50, 50),
                    5, 10, 10, 4));

                rockDropTimer = 600;
            }
        }

        public override void DrawText(Game1 game)
        {
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Stop at Rest Sites!",
                new Vector2(1220 - game.Camera, 330),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "(Regain all HP and Mana)",
                new Vector2(1190 - game.Camera, 360),
                Color.Gray);
        }
    }
}
