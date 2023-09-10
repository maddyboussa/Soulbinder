using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    // represents an abstract spell
    public abstract class Spell
    {
        // fields
        protected Texture2D spellTexture;

        protected bool isCasting;

        protected Rectangle spellRect;

        protected bool unlocked;

        protected string name;

        // properties

        public bool IsCasting { get { return isCasting;  } set { isCasting = value; } }

        public int X { get { return spellRect.X; } set { spellRect.X = value; } }

        public int Y { get { return spellRect.Y; } set { spellRect.Y = value; } }

        public Rectangle SpellRect { get { return spellRect; } }

        public bool Unlocked { get { return unlocked; } set { unlocked = value; } }

        public string Name { get { return name; } set { name = value; } }

        // constructor
        public Spell(Texture2D spellTexture, int x, int y, int width, int height)
        {
            this.spellTexture = spellTexture; 
            spellRect = new Rectangle(x, y, width, height);

            unlocked = false;

            name = "Spell";
        }
    }
}
