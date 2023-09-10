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
    class KeepLevelBoss : Level
    {
        // FIELDS =======================================================================
        // Level Specific Fields


        // PROPERTIES ===================================================================
        // There shouldn't be any properties not already included with Level.

        // CONSTRUCTORS =================================================================
        public KeepLevelBoss(Game1 game, string titFile)
            : base(game, titFile)
        {
            // Setting Initial Checkpoint (a.k.a Player Spawn)
            CheckpointPosition = new Vector2(1780, 500);

            Boss.Add(new LichKing(
                game.SpriteManager.LichKingSprite, 
                new Rectangle(1280, 0, 0, 0),
                3, 175, 175,
                game.SpriteManager.LichKingHandLeftFist,
                game.SpriteManager.LichKingProjectile,
                game.SpriteManager.SkeletonSprite,
                new Entity(
                    game.SpriteManager.LichKingSprite, 
                    new Rectangle(1680, 0, 300, 300), 3, 175, 175),
                new Entity(
                    game.SpriteManager.LichKingHandRightFist, 
                    new Rectangle(2400, 100, 170, 170), 3, 0, 0),
                new Entity(
                    game.SpriteManager.LichKingHandLeftFist, 
                    new Rectangle(1180, 100, 170, 170), 3, 0, 0)));

            // Load the background
            Background = game.SpriteManager.LitchKingBackground;

            // Create Boss


            Name = "Level Boss";
        }

        // METHODS ======================================================================
        public override void ConnectDoors(Game1 game)
        {
            
        }


        public override void Update(Game1 game)
        {
            if(Boss == null)
            {
                game.UIManager.TimerActive = false;
                game.CurrentMenu = game.WinMenu;
                game.GameManager.GameState = GameState.Menu;
            }
        }
        public override void DrawText(Game1 game)
        {
            // N/A
        }

        public override void Draw(Game1 game)
        {
            if (Background != null)
            {
                game.SpriteBatch.Draw(
                    Background,
                    new Rectangle(
                        1200 - game.Camera, 
                        0,
                        game.GraphicsManager.PreferredBackBufferWidth,
                        game.GraphicsManager.PreferredBackBufferHeight),
                    Color.White);
            }

            if (Tiles == null)
            {
                return;
            }

            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    if (Tiles[x, y] != null)
                    {
                        Tiles[x, y].Draw(game.SpriteBatch, game.Camera);
                    }
                }
            }

            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Draw(game.SpriteBatch, game.Camera);
            }
        }
    }
}
