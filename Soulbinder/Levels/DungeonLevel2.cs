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
    class DungeonLevel2 : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel2(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(100, 425);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            Name = "Level 2";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(100, 392, 64, 128),
                game.L_Dungeon1,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 392, 64, 128),
                game.L_Dungeon3,
                false));
        }

        public override void DrawText(Game1 game)
        {
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Press 'G' to Activate God Mode",
                new Vector2(320 - game.Camera, 300),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "(Infinite Health, Infinite Mana, Infinite Jumps, Unlocks All Spells)",
                new Vector2(200 - game.Camera, 330),
                Color.Gray);
        }
    }
}
