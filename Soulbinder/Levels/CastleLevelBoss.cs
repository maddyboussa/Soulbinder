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
    class CastleLevelBoss : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields

        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public CastleLevelBoss(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(140, 550);
            
            // Load the background
            Background = game.SpriteManager.CastleBackground;

            Boss.Add(new Statue(game.SpriteManager.StatueSprite,
                new Rectangle(800, 200, game.SpriteManager.StatueSprite.Width / 2, game.SpriteManager.StatueSprite.Height / 2),
                3, 50, 50, game.SpriteManager.StatueAttackAnims, game.SpriteManager.StatueRockTexture));
            Boss.Add(new Statue(game.SpriteManager.StatueSprite,
                new Rectangle(400, 200, game.SpriteManager.StatueSprite.Width / 2, game.SpriteManager.StatueSprite.Height / 2),
                3, 50, 50, game.SpriteManager.StatueAttackAnims, game.SpriteManager.StatueRockTexture));
            Boss.Add(new Statue(game.SpriteManager.StatueSprite,
                new Rectangle(600, 200, game.SpriteManager.StatueSprite.Width / 2, game.SpriteManager.StatueSprite.Height / 2),
                3, 50, 50, game.SpriteManager.StatueAttackAnims, game.SpriteManager.StatueRockTexture));

            Name = "Level Boss";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            Doors.Add(new Door(
                new Rectangle(120, 474, 64, 128),
                game.L_Castle8,
                true));

            Doors.Add(new Door(
                new Rectangle(1100, 474, 64, 128),
                game.L_Keep1,
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
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "DISCLAIMER: Not Working as Intended",
                new Vector2(460 - game.Camera, 300),
                Color.Red);
        }
    }
}
