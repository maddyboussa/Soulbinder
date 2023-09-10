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
    class DungeonLevelBoss : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public DungeonLevelBoss(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 600);

            // Load the background
            Background = game.SpriteManager.DungeonBackground;

            // Creating Boss (if present in level; otherwise left null)
            Boss.Add(new Executioner(
                game.SpriteManager.ExecutionerSprite,
                new Rectangle(800, 200, game.SpriteManager.ExecutionerSprite.Width / 2, game.SpriteManager.ExecutionerSprite.Height / 2),
                3, 125, 125,
                game.SpriteManager.ExecutionerAttackAnims,
                game.SpriteManager.Pixel));

            Name = "Level Boss";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(140, 552, 64, 128),
                game.L_Dungeon9,
                true));

            Doors.Add(new Door(
                new Rectangle(1100, 552, 64, 128),
                game.L_Castle1,
                true));
        }

        public override void Update(Game1 game)
        {
            if (Boss == null)
            {
                // Open the doors
                Doors[0].Locked = false;
                Doors[1].Locked = false;
            }
        }
        public override void DrawText(Game1 game)
        {
            // N/A
            base.DrawText(game);
        }
    }
}
