using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soulbinder.GameObjects;

namespace Soulbinder
{
    public enum GameState
    {
        Menu,
        Game
    }

    public class GameManager
    {
        // FIELDS =======================================================================
        private Game1 game;
        private GameState gameState;
        private GameState previousGameState;

        // Create the lich king
        private LichKing lichKing;



        // PROPERTIES ===================================================================
        public GameState GameState { get => gameState; set => gameState = value; }
        public GameState PreviousGameState { get => previousGameState; set => previousGameState = value; }

        // CONSTRUCTORS =================================================================
        public GameManager(Game1 game)
        {
            this.game = game;
            this.gameState = GameState.Menu;
        }

        // METHODS ======================================================================
        public void Update(Menu menu)
        {
            menu.Update(game);
        }

        public void Update(Level level)
        {
            PreviousGameState = GameState.Game;

            // Check for Interactions with Doors
            foreach (Door currentDoor in level.Doors)
            {
                if (currentDoor.Update(game))
                {
                    break;
                }
            }

            // Check for Interactions with Unlockables
            foreach (Unlockable currentUnlockable in level.Unlockables)
            {
                if (currentUnlockable.Update(game))
                {
                    break;
                }
            }

            // Check Interactions with Rest Sites
            foreach (RestSite currentRestSite in level.RestSites)
            {
                if (currentRestSite.Update(game))
                {
                    break;
                }
            }

            // Update Player
            game.Player.Update(game);

            // Update Enemies
            for (int i = level.Enemies.Count - 1; i >= 0; i--)
            {
                Skeleton currentEnemy = level.Enemies[i];

                currentEnemy.ResolveCollisions(level.Collisions);
                currentEnemy.Update(game);

                if (currentEnemy.Dead)
                {
                    level.Enemies.Remove(currentEnemy);
                }
            }

            // Update Boss (if exists)
            if (level.Boss != null)
            {
                for (int i = level.Boss.Count - 1; i >= 0; i--)
                {
                    level.Boss[i].Update(game);

                    if (level.Boss[i].Dead)
                    {
                        level.Boss[i] = null;
                        level.Boss.RemoveAt(i);

                        if(level.Boss.Count == 0)
                        {
                            level.Boss = null;
                        }
                    }
                }
            }

            // Update Enviornment
            level.Update(game);

            // Update Projectiles
            level.UpdateProjectiles(game);

            // Update UI
            game.UIManager.Update(game);

            // Check if Player is Dead and reset level
            if (game.Player.Dead)
            {
                game.Player.Reset();

                game.Player.Position = new Rectangle(
                     (int)game.CurrentLevel.CheckpointPosition.X,
                     (int)game.CurrentLevel.CheckpointPosition.Y,
                     game.Player.Position.Width,
                     game.Player.Position.Height);

                foreach (Skeleton currentEnemy in level.Enemies)
                {
                    currentEnemy.Reset();
                }

                if (level.Boss != null)
                {
                    for (int i = level.Boss.Count - 1; i >= 0; i--)
                    {
                        game.CurrentLevel.Boss[i].Reset();
                    }
                }

                game.CurrentMenu = game.DeathMenu;
                GameState = GameState.Menu;

            }
        }

        public void Draw(Menu menu)
        {
            menu.Draw(game);
        }

        public void Draw(Level level)
        {
            // Draw Enviornment
            level.Draw(game);

            // Draw Doors
            foreach (Door currentDoor in level.Doors)
            {
                currentDoor.Draw(game);
            }

            // Draw Unlockables
            foreach (Unlockable currentUnlockable in level.Unlockables)
            {
                currentUnlockable.Draw(game);
            }

            // Draw Restsites
            foreach (RestSite currentRestSite in level.RestSites)
            {
                currentRestSite.Draw(game);
            }

            // Draw Enemies
            foreach (Skeleton currentEnemy in level.Enemies)
            {
                currentEnemy.DrawState(currentEnemy.State, game.SpriteBatch, game.Camera);
            }

            // Draw Boss (if present)
            if (level.Boss != null)
            {
                for (int i = level.Boss.Count - 1; i >= 0; i--)
                {
                    level.Boss[i].Draw(game);
                    level.Boss[i].DrawShapes(game);
                }
            }

            // Draw Player
            game.Player.Draw(game);

            /*
            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                $"{game.Player.X}, {game.Player.Y}",
                new Vector2(800, 400),
                Color.White);
            */

            // Draw Level Text
            level.DrawText(game);

            // Draw UI
            game.UIManager.Draw(game);

            if (game.Player.Dead)
            {
                //game.UIManager.DrawFade();
            }
        }

        // HELPER METHODS ===============================================================
        /// <summary>
        /// Checks if a given key was pressed this frame.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool SingleKeyPress(Keys key)
        {
            // Check if the key was pressed this frame.
            if (game.KeyboardState.IsKeyDown(key)
                && game.PreviousKeyboardState.IsKeyUp(key))
            {
                // If so, return true
                return true;
            }

            // Otherwise, return false
            else return false;
        }

        /// <summary>
        /// Check if the left mouse button is being held
        /// </summary>
        /// <param name="ms">The MouseState to check</param>
        /// <returns>True if the left mouse button is being held, false is not</returns>
        public bool MouseHeld(MouseState ms)
        {
            // Check if both the last state and the current state are both pressing the left mouse button
            if (game.MouseState.LeftButton == ButtonState.Pressed
                && game.PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                // If so, return true
                return true;
            }

            // Otherwise, return false
            else return false;
        }

        /// <summary>
        /// Calculate the distance between two GameObjects
        /// </summary>
        /// <param name="go1">The first GameObject</param>
        /// <param name="go2">The second GameObject</param>
        /// <returns>The distance between two GameObjects</returns>
        double CalcDistance(GameObject go1, GameObject go2)
        {
            return Math.Sqrt(Math.Pow(go1.X - go2.X, 2) + Math.Pow(go1.Y - go2.Y, 2));
        }

        /// <summary>
        /// Returns the difference between the x-positions of two GameObjects
        /// </summary>
        /// <param name="go1">The first GameObject</param>
        /// <param name="go2">The second GameObject</param>
        /// <returns>The x distance between the two GameObjects</returns>
        public double CalcDistanceFromCenterX(GameObject go1, GameObject go2)
        {
            return Math.Abs((go2.X + go2.Position.Width / 2) - (go1.X + go1.Position.Width / 2));
        }

        /// <summary>
        /// Returns the difference between the y-positions of two GameObjects
        /// </summary>
        /// <param name="go1">The first GameObject</param>
        /// <param name="go2">The second GameObject</param>
        /// <returns>The y distance between the two GameObjects</returns>
        public double CalcDistanceFromCenterY(GameObject go1, GameObject go2)
        {
            return Math.Abs((go2.Y + go2.Position.Height / 2) - (go1.Y + go1.Position.Height / 2));
        }
    }
}