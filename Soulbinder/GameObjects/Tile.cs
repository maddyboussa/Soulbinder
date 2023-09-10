using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder.GameObjects
{
    public class Tile : GameObject
    {
        // FIELDS ----

        // PROPERTIES ----

        // CONSTRUCTORS ----
        public Tile(Texture2D sprite, Rectangle position) : base(sprite, position)
        {
            // OMEGALUL
        }

        // METHODS ----
        public override void Draw(SpriteBatch sb, int camX)
        {
            sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
        }
    }
}
