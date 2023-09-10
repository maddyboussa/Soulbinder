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
    class KeepLevel5Bottom : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields


        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public KeepLevel5Bottom(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(35, 325);

            // Load the background
            Background = game.SpriteManager.KeepBackground;

            // Spawn Enemies
            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(1385, 436, 32, 32),
               0, 1, 1, 1));
            Enemies[0].DoNothing = true;

            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(958, 92, 32, 32),
               0, 1, 1, 1));
            Enemies[1].DoNothing = true;

            Enemies.Add(new Skeleton(
               game.SpriteManager.Pixel,
               new Rectangle(500, 92, 32, 32),
               0, 1, 1, 1));
            Enemies[2].DoNothing = true;

            // Add enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(500, 400, 32, 64),
               2, 15, 15, 1));

        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(15, 272, 64, 128),
                game.L_Keep4Bottom,
                true));

        }

        public override void CreateUnlockable(Game1 game)
        {
            Unlockables.Add(new Unlockable(
                game.SpriteManager.BasicAttackSprite,
                new Rectangle(1550, 346, game.Player.ProjectileSpell.SpellRect.Width, 
                game.Player.ProjectileSpell.SpellRect.Height),
                game.Player.ProjectileSpell));
        }

        public override void Update(Game1 game)
        {
            if (Enemies.Count == 0)
            {
                Doors[0].Locked = false;
            }
        }

        public override void DrawText(Game1 game)
        {
            if (game.Player.ProjectileSpell.Unlocked)
            {
                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    "Press 'Q' to Cast Projectile",
                    new Vector2(800 - game.Camera, 300),
                    Color.White);

                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    "(Ranged Attack with High Damage)",
                    new Vector2(760 - game.Camera, 330),
                    Color.DarkGray);
            }
        }
    }
}
