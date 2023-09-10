using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    public class Projectile : Entity
    {
        // Fields
        protected int damage;
        protected bool dealtDamage;
        protected Vector2 projectileVector;
        private bool deflected;

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public bool DealtDamage
        {
            get { return dealtDamage; }
            set { dealtDamage = value; }
        }

        public Vector2 ProjectileVector
        {
            get { return projectileVector; }
            set { projectileVector = value; }
        }

        public bool Deflected
        {
            get { return deflected; }
            set { deflected = value; }
        }

        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }

        // Propertyfor speed of projectile after deflection
        public Vector2 DeflectSpeed { get; set; }

        // Constructor
        public Projectile(Texture2D sprite, Rectangle position, int speed, int healthCur, int healthMax, int damage) : 
            base(sprite, position, speed, healthCur, healthMax)
        {
            this.damage = damage;
            dealtDamage = false;
        }

        // Methods
        /// <summary>
        /// Checks if the Projectile collides with another Entity
        /// </summary>
        /// <param name="ent">The Entity to check a collision with</param>
        /// <returns>True if the Entity's Rectangle collides with the Entity's, false if not</returns>
        public override bool Collides(Entity ent)
        {
            if (Position.Intersects(ent.Position))
            {
                dealtDamage = true;
                return true;
            }
            else return false;
        }
    }
}
