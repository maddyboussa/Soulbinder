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
    class DungeonLevel6Bottom : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevel6Bottom(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 350);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            Name = "Level 6 Bottom";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 352, 64, 128),
                game.L_Dungeon5,
                false));

            Doors.Add(new Door(
                new Rectangle(1100, 232, 64, 128),
                game.L_Dungeon7Bottom,
                false));
        }

        public override void DrawText(Game1 game)
        {
            // N/A
            base.DrawText(game);
        }
    }
}
