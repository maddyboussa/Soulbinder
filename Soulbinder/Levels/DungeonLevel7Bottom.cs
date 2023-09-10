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
    class DungeonLevel7Bottom : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields
        private double rockDropTimer;

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel7Bottom(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 350);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            rockDropTimer = 1000;

            Name = "Level 7 Bottom";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 352, 64, 128),
                game.L_Dungeon6Bottom,
                false));
        }

        public override void CreateUnlockable(Game1 game)
        {
            // Add the Deflect unlockable
            Unlockables.Add(new Unlockable(game.SpriteManager.DeflectSprite, new Rectangle(1725, 496, game.Player.Deflect.SpellRect.Width/2, 
                game.Player.Deflect.SpellRect.Height/2), game.Player.Deflect));
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
                    new Rectangle(518, 100, 50, 50),
                    5, 10, 10, 4));

                rockDropTimer = 1000;
            }
        }

        public override void DrawText(Game1 game)
        {
            if(game.Player.Deflect.Unlocked)
            {
                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    "Press 'RIGHT CLICK' to Cast Deflect",
                    new Vector2(370 - game.Camera, 300),
                    Color.White);

                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    "(Deflects Projectiles and Grants Brief Invincibility)",
                    new Vector2(300 - game.Camera, 330),
                    Color.Gray);
            }
            
        }
    }
}
