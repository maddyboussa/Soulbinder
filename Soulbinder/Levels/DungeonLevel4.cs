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
    class DungeonLevel4 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel4(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(160, 410);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            // Load enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(1200, 300, 32, 64),
               2, 15, 15, 200));

            Name = "Level 4";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(160, 352, 64, 128),
                game.L_Dungeon3,
                false));

            Doors.Add(new Door(
                new Rectangle(1700, 475, 64, 128),
                game.L_Dungeon5,
                false));
        }

        public override void DrawText(Game1 game)
        {
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Press 'T' to Toggle Extra UI",
                new Vector2(920 - game.Camera, 300),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "(Speedrun Timer and Death Counter)",
                new Vector2(880 - game.Camera, 330),
                Color.Gray);
        }
    }
}
