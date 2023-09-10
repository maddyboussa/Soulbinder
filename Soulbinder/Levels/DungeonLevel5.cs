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
    class DungeonLevel5 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel5(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(160, 600);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            // Load Enemy
            Enemies.Add(new Skeleton(
               game.SpriteManager.SkeletonSprite,
               new Rectangle(464, 300, 32, 64),
               2, 15, 15, 200));

            Name = "Level 5";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(160, 552, 64, 128),
                game.L_Dungeon4,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 75, 64, 128),
                game.L_Dungeon6Top,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 392, 64, 128),
                game.L_Dungeon6Bottom,
                false));
        }

        public override void DrawText(Game1 game)
        {
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Explore Different Paths!",
                new Vector2(520 - game.Camera, 100),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "(You can always Back-Track)",
                new Vector2(490 - game.Camera, 130),
                Color.Gray);
        }
    }
}
