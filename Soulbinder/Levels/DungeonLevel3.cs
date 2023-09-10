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
    class DungeonLevel3 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel3(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 475);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            // Add enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(680, 452, 32, 64),
               2, 15, 15, 200));

            Name = "Level 3";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 435, 64, 128),
                game.L_Dungeon2,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 435, 64, 128),
                game.L_Dungeon4,
                false));
        }

        public override void DrawText(Game1 game)
        {
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Press 'Left Click' to Attack",
                new Vector2(650 - game.Camera, 300),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "(Deals Damage and Generates Mana)",
                new Vector2(600 - game.Camera, 330),
                Color.Gray);
        }
    }
}
