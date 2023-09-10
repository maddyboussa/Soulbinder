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
    public abstract class Menu
    {
        // FIELDS =======================================================================
        private Game1 game;
        private Texture2D background;



        // PROPERTIES ===================================================================
        public Game1 Game { get => game; set => game = value; }
        public Texture2D Background { get => background; set => background = value; }



        // CONSTRUCTORS
        public Menu(Game1 game, Texture2D background)
        {
            this.Game = game;
            this.Background = background;
        }



        // METHODS ======================================================================
        public virtual void Update(Game1 game)
        {

        }

        public virtual void Draw(Game1 game)
        {

        }

        
    }
}
