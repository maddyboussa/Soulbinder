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
    class WinMenu : Menu
    {
        // FIELDS =======================================================================
        private Button quit;

        private int buttonWidth;
        private int buttonHeight;

        private int startHeight;
        private int marginsInBetween;

        // PROPERTIES ===================================================================
        public int ButtonWidth { get => buttonWidth; set => buttonWidth = value; }
        public int ButtonHeight { get => buttonHeight; set => buttonHeight = value; }
        public int StartHeight { get => startHeight; set => startHeight = value; }
        public int MarginsInBetween { get => marginsInBetween; set => marginsInBetween = value; }
        public Button Quit { get => quit; set => quit = value; }


        // CONSTRUCTORS
        public WinMenu(Game1 game) : base(game, game.SpriteManager.TitleScreenNoText)
        {
            int buttonWidth = 150;
            int buttonHeight = 30;

            int startHeight = game.GraphicsManager.PreferredBackBufferHeight * 2 / 5;
            int marginsInBetween = 30;

            Quit = new Button(
                game.SpriteManager.Pixel,
                game.SpriteManager.Arial16,
                new Rectangle((game.GraphicsManager.PreferredBackBufferWidth / 2) - (buttonWidth / 2),
                startHeight + buttonHeight + marginsInBetween, buttonWidth, buttonHeight));

        }

        // METHODS ======================================================================
        public override void Update(Game1 game)
        {
            game.GameManager.PreviousGameState = GameState.Menu;

            // If the Quit button is pressed, quit the game
            if (quit.Clicked(game.MouseState))
            {
                game.CurrentMenu = game.MainMenu;
                game.ResetGame();
            }
        }

        public override void Draw(Game1 game)
        {
            if (Background != null)
            {
                game.SpriteBatch.Draw(
                    Background,
                    new Rectangle(
                        0, 0,
                        game.GraphicsManager.PreferredBackBufferWidth,
                        game.GraphicsManager.PreferredBackBufferHeight),
                    Color.White);
            }

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "YOU HAVE RECLAIMED YOUR SOUL",
                new Vector2(
                        (game.GraphicsManager.PreferredBackBufferWidth / 2) - 170,
                        (game.GraphicsManager.PreferredBackBufferHeight / 3) - 70),
                Color.White);

            game.SpriteBatch.DrawString(
                    game.SpriteManager.Arial16,
                    string.Format(
                        "{0:00}:{1:00}:{2:00}",
                        Math.Round(((game.UIManager.TimerValue * 0.001) / 60) % 60, MidpointRounding.ToZero),
                        ((game.UIManager.TimerValue * 0.001)) % 60,
                        game.UIManager.TimerValue % 1000),
                    new Vector2(
                        (game.GraphicsManager.PreferredBackBufferWidth / 2) - 54,
                        (game.GraphicsManager.PreferredBackBufferHeight / 3)),
                    Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                $"Deaths: {game.Player.DeathCounter}",
                new Vector2(
                        (game.GraphicsManager.PreferredBackBufferWidth / 2) - 50,
                        (game.GraphicsManager.PreferredBackBufferHeight / 3) + 30),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "A GAME CREATED BY ELDRITCH PACT",
                new Vector2(
                        (game.GraphicsManager.PreferredBackBufferWidth / 2) - 190,
                        (game.GraphicsManager.PreferredBackBufferHeight / 2) + 170),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Abby Doskey    Dylan Pegg    Vincent Le",
                new Vector2(
                        (game.GraphicsManager.PreferredBackBufferWidth / 2) - 186,
                        (game.GraphicsManager.PreferredBackBufferHeight / 2) + 210),
                Color.White);

            game.SpriteBatch.DrawString(
                game.SpriteManager.Arial16,
                "Maddy Boussa    Ben Sultzer",
                new Vector2(
                        (game.GraphicsManager.PreferredBackBufferWidth / 2) - 140,
                        (game.GraphicsManager.PreferredBackBufferHeight / 2) + 240),
                Color.White);

            if (quit.MouseInBounds(game.MouseState))
            {
                quit.Draw(
                    game.SpriteBatch,
                    "Quit",
                    quit.ButtonShape,
                    Alignment.Center,
                    Color.Red,
                    Color.Black);
            }
            else
            {
                quit.Draw(
                game.SpriteBatch,
                "Quit",
                quit.ButtonShape,
                Alignment.Center,
                Color.White,
                Color.Black);
            }
        }
    }
}
