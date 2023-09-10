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
    class MainMenu : Menu
    {
        // FIELDS =======================================================================
        private Button start;
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
        public Button Start { get => start; set => start = value; }
        public Button Quit { get => quit; set => quit = value; }


        // CONSTRUCTORS
        public MainMenu(Game1 game) : base(game, game.SpriteManager.TitleScreen)
        {
            int buttonWidth = 150;
            int buttonHeight = 30;

            int startHeight = game.GraphicsManager.PreferredBackBufferHeight * 3 / 5;
            int marginsInBetween = 30;


            Start = new Button(
                game.SpriteManager.Pixel,
                game.SpriteManager.Arial16,
                new Rectangle((game.GraphicsManager.PreferredBackBufferWidth / 2) - (buttonWidth / 2),
                startHeight, buttonWidth, buttonHeight));

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

            // If the Start button is pressed, start the game
            if (start.Clicked(game.MouseState))
            {
                game.GameManager.GameState = GameState.Game;
            }

            // If the Quit button is pressed, quit the game
            if (quit.Clicked(game.MouseState))
            {
                game.Exit();
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

            if (start.MouseInBounds(game.MouseState))
            {
                start.Draw(
                    game.SpriteBatch,
                    "Start",
                    start.ButtonShape,
                    Alignment.Center,
                    Color.Red,
                    Color.Black);
            }
            else
            {
                start.Draw(
                    game.SpriteBatch,
                    "Start",
                    start.ButtonShape,
                    Alignment.Center,
                    Color.White,
                    Color.Black);
            }

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
