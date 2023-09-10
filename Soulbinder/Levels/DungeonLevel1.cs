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
    class DungeonLevel1 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel1(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(576, 525);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            Name = "Level 1";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(1100, 315, 64, 128),
                game.L_Dungeon2,
                false));
        }

        public override void DrawText(Game1 game)
        {

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Press 'A' to Move Left",
                new Vector2(700 - game.Camera, 240),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Press 'D' to Move Right",
                new Vector2(700 - game.Camera, 270),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Press 'SPACE' to Jump",
                new Vector2(700 - game.Camera, 300),
                Color.White);
        }
    }
}
