using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Soulbinder.GameObjects;
using System.IO;

namespace Soulbinder
{
    public class UIManager
    {
        // FIELDS =======================================================================
        private Game1 game;

        private StatusBar healthBar;
        private StatusBar manaBar;
        private StatusBar bossBar;

        private bool extraActive;
        private double timerValue;
        private bool timerActive;



        // PROPERTIES ===================================================================
        public StatusBar HealthBar { get => healthBar; set => healthBar = value; }
        public StatusBar ManaBar { get => manaBar; set => manaBar = value; }
        public StatusBar BossBar { get => bossBar; set => bossBar = value; }
        public bool ExtraActive { get => extraActive; set => extraActive = value; }
        public double TimerValue { get => timerValue; set => timerValue = value; }
        public bool TimerActive { get => timerActive; set => timerActive = value; }



        // CONSTRUCTORS =================================================================
        public UIManager(Game1 game)
        {
            this.game = game;

            healthBar = new StatusBar(
                game.SpriteManager.Pixel,
                new Rectangle(0, 0, 300, 30),
                game.Player.CurrentHealth,
                game.Player.MaximumHealth,
                game.SpriteManager.Arial16);
            manaBar = new StatusBar(
                game.SpriteManager.Pixel,
                new Rectangle(0, 30, 300, 30),
                game.Player.CurrentMana,
                game.Player.MaximumMana,
                game.SpriteManager.Arial16);
            bossBar = null;

            extraActive = false;
            timerActive = true;
            TimerValue = 0;
        }
        

        // METHODS ======================================================================
        public void Update(Game1 game)
        {
            // Update Health & Mana
            healthBar.Update(game.Player.CurrentHealth);
            manaBar.Update(game.Player.CurrentMana);

            // If there is a boss, but no boss bar yet, create one
            if (game.CurrentLevel.Boss != null && bossBar == null)
            { 
                for (int i = game.CurrentLevel.Boss.Count - 1; i >= 0; i--)
                {
                    bossBar = new StatusBar(
                    game.SpriteManager.Pixel,
                    new Rectangle(game.GraphicsManager.PreferredBackBufferWidth - 980, 0, 980, 60),
                    game.CurrentLevel.Boss.Count * game.CurrentLevel.Boss[i].CurrentHealth,
                    game.CurrentLevel.Boss.Count * game.CurrentLevel.Boss[i].MaximumHealth,
                    game.SpriteManager.Arial16);
                    bossBar.Update(game.CurrentLevel.Boss.Count * game.CurrentLevel.Boss[i].CurrentHealth);
                }
            }
            // If boss bar already exists, then update
            else if(game.CurrentLevel.Boss != null)
            {
                int totalHealth = 0;

                for (int i = game.CurrentLevel.Boss.Count - 1; i >= 0; i--)
                {
                    totalHealth += game.CurrentLevel.Boss[i].CurrentHealth;
                }

                bossBar.Update(totalHealth);
            }
            else
            {
                bossBar = null;
            }

            // Update Speedrun Timer
            if(timerActive)
            {
                TimerValue += game.ElapsedMilliseconds;
            }

            // Check if Extra UI is toggled on/off
            if(game.GameManager.SingleKeyPress(Keys.T))
            {
                if(extraActive == true) { extraActive = false; }
                else { extraActive = true; }
            }
        }

        public void Draw(Game1 game)
        {
            // Draw Health & Mana
            healthBar.Draw(
                game.SpriteBatch, 
                " HP", 
                healthBar.Position, 
                Alignment.Left, 
                Color.DarkGreen, 
                Color.ForestGreen, 
                Color.DarkGreen);
            manaBar.Draw(
                game.SpriteBatch, 
                " MP", 
                manaBar.Position, 
                Alignment.Left, 
                Color.Navy, 
                Color.Blue, 
                Color.Navy);

            // Draw Boss (if present)
            if (bossBar != null)
            {
                if (bossBar.CurrentValue <= bossBar.MaximumValue / 2)
                {
                    for (int i = game.CurrentLevel.Boss.Count - 1; i >= 0; i--)
                    {
                        bossBar.Draw(
                        game.SpriteBatch,
                        game.CurrentLevel.Boss[0].Name.ToUpper(),
                        bossBar.Position,
                        Alignment.Center,
                        Color.DarkRed,
                        Color.Red,
                        Color.Red);
                    }
                }
                else
                {
                    for (int i = game.CurrentLevel.Boss.Count - 1; i >= 0; i--)
                    {
                        bossBar.Draw(
                        game.SpriteBatch,
                        game.CurrentLevel.Boss[0].Name.ToUpper(),
                        bossBar.Position,
                        Alignment.Center,
                        Color.DarkRed,
                        Color.Red,
                        Color.DarkRed);
                    }
                }
            }

            // If Extra UI is toggled on, draw Speedrun timer and Death Counter
            if(extraActive) 
            {
                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    string.Format(
                        "{0:00}:{1:00}:{2:00}",
                        Math.Round(((TimerValue * 0.001) / 60) % 60, MidpointRounding.ToZero),
                        ((TimerValue * 0.001)) % 60,
                        TimerValue % 1000),
                    new Vector2(
                        20,
                        game.GraphicsManager.PreferredBackBufferHeight - 80),
                    Color.White);

                game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    $"Deaths: {game.Player.DeathCounter}",
                    new Vector2(
                        20,
                        game.GraphicsManager.PreferredBackBufferHeight - 50),
                    Color.White);
            }
        }

        /*
        /// <summary>
        /// Transition the game by locking the player and setting a fade, then transition to a new GameState
        /// </summary>
        /// <param name="newGameState"></param>
        /// <param name="gameTime"></param>
        public void TransitionGame(GameState newGameState, GameTime gameTime)
        {
            // Lock the player
            game.Player.Locked = true;

            // Set a timer before losing
            fadeTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (fadeTimer > 3)
            {
                gameState = newGameState;
            }
        }

        /// <summary>
        /// Draw the fade overlay for transitions
        /// </summary>
        public void DrawFade()
        {
            loseFade = 1.0f - ((3 * brightness) - (float)fadeTimer);

            _spriteBatch.Draw(pixel, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight), Color.Black * loseFade);
        }
        */
    }
}
