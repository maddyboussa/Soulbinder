using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soulbinder
{
    // Ben Sultzer
    // Class: Enemy
    // Purpose: Represents an entity
    // that can cause damage to the player
    // through it's own attacks and movement
    // Restrictions: None
    public abstract class Enemy : Entity
    {
        // Fields
        protected bool dead;
        private bool onScreen;
        private Vector2 gravity;
        private bool dealtDamage;

        public bool Dead
        {
            get
            {
                if (CurrentHealth <= 0)
                {
                    return true;
                }
                else return false;
            }
            set { dead = value; }
        }

        /// <summary>
        /// Property for getting the value of gravity
        /// </summary>
        public Vector2 Gravity
        {
            get
            {
                return gravity;
            }
        }

        /// <summary>
        /// Property for getting whether or not the Enemy 
        /// is on the screen
        /// </summary>
        public bool OnScreen
        {
            get
            {
                /*
                if (this.X >= 0 &&
                    this.X <=
                    //OldGameManager.graphicsEnemy.PreferredBackBufferWidth - position.Width &&
                    this.Y >= 0 &&
                    this.Y <=
                    //OldGameManager.graphicsEnemy.PreferredBackBufferHeight - position.Height)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                */
                return true;
            }
        }

        /// <summary>
        /// Property for indicating whether or not this Enemy was dealt
        /// damage during the current frame
        /// </summary>
        public bool DealtDamage
        {
            get { return dealtDamage; }
            set { dealtDamage = value; }
        }

        /// <summary>
        /// Constructor for initializing the Enemy's fields
        /// </summary>
        /// <param name="sprite">The image texture of the
        /// Enemy</param>
        /// <param name="position">The rectangle position of
        /// the Enemy</param>
        /// <param name="speed">The movement speed of the 
        /// Enemy</param>
        /// <param name="healthCur">The current health of
        /// the Enemy</param>
        /// <param name="healthMax">The max health of the
        /// Enemy</param>
        public Enemy(Texture2D sprite, Rectangle position, int speed,
            int healthCur, int healthMax)
            : base(sprite, position, speed, healthCur, healthMax)
        {
            gravity = new Vector2(0, 0.5f);
            dealtDamage = false;
        }

        // Methods
        /// <summary>
        /// Reset the enemy
        /// </summary>
        public override void Reset()
        {
            // Base reset
            base.Reset();

            // Reset other values
            dealtDamage = false;
        }

        /// <summary>
        /// Overridable method for moving an Enemy to a certain
        /// position
        /// </summary>
        public virtual void Move(int targetPositionX, int targetPositionY)
        {
        }

        /// <summary>
        /// Generic overridable method for moving an Enemy around
        /// the screen in a specific pattern
        /// </summary>
        public virtual void Move()
        {
        }

        public virtual void Update(Game1 game)
        {
            // Override plz
        }

        public virtual void Draw(Game1 game)
        {

        }
    }
}
