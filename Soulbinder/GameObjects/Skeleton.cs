using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soulbinder
{
    // Skeleton phase enum
    public enum SkeletonStates
    {
        Move,
        Idle,
        Aggro
    }

    // Ben Sultzer
    // Class: Skeleton
    // Purpose: The basic enemy for the
    // first level
    // Restrictions: None
    public class Skeleton : Enemy
    {
        // Fields
        private int patrolDistance;
        private int originX;
        private int originY;
        private int direction;
        private Rectangle patrolArea;
        private bool doNothing;

        private SkeletonStates state;
        private double stateCounter;

        /// <summary>
        /// Property for getting and setting
        /// the distance the Skeleton walks
        /// while patrolling
        /// </summary>
        public int PatrolDistance
        {
            get
            {
                return patrolDistance;
            }

            set
            {
                patrolDistance = value;
            }
        }

        /// <summary>
        /// Property for getting and setting the
        /// starting x-position of the Skeleton
        /// </summary>
        public int OriginX
        {
            get
            {
                return originX;
            }

            set
            {
                originX = value;
            }
        }

        /// <summary>
        /// Property for getting and setting the
        /// starting y-position of the Skeleton
        /// </summary>
        public int OriginY
        {
            get
            {
                return originY;
            }

            set
            {
                originY = value;
            }
        }

        /// <summary>
        /// Property for getting and setting
        /// the direction that the Skeleton is 
        /// facing
        /// </summary>
        public int Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
            }
        }

        /// <summary>
        /// Property for getting and setting the
        /// patrol area rectangle of the Skeleton
        /// </summary>
        public Rectangle PatrolArea
        {
            get
            {
                return patrolArea;
            }

            set
            {
                patrolArea = value;
            }
        }

        public SkeletonStates State { get => State1; set => State1 = value; }
        public double StateCounter { get => StateCounter1; set => StateCounter1 = value; }
        public SkeletonStates State1 { get => state; set => state = value; }
        public double StateCounter1 { get => stateCounter; set => stateCounter = value; }
        public bool DoNothing { get => doNothing; set => doNothing = value; }

        /// <summary>
        /// Constructor for initializing the Skeleton's fields
        /// </summary>
        /// <param name="sprite">The image texture of the
        /// Skeleton</param>
        /// <param name="position">The rectangle position of
        /// the Skeleton</param>
        /// <param name="speed">The movement speed of the 
        /// Skeleton</param>
        /// <param name="healthCur">The current health of
        /// the Skeleton</param>
        /// <param name="healthMax">The max health of the
        /// Skeleton</param>
        /// <param name="patrolDistance">The distance from
        /// its origin the Skeleton moves during patrol</param>
        public Skeleton(Texture2D sprite, Rectangle position, int speed,
            int healthCur, int healthMax, int patrolDistance)
            : base(sprite, position, speed, healthCur, healthMax)
        {
            this.patrolDistance = patrolDistance;
            originX = position.X;
            direction = 1;
        }

        // Methods
        public override void Update(Game1 game)
        {
            if (doNothing)
            {
                return;
            }

            if (!dead)
            {
                // Check and update the skeleton state
                switch (State1)
                {
                    case SkeletonStates.Move:

                        // Determime if the rectangle between the player and the 
                        // skeleton is within the skeleton's patrol space
                        if (WithinLineOfSight(game.Player))
                        {
                            // Change the state to Idle if the player has been
                            // seen
                            State1 = SkeletonStates.Idle;
                            // Otherwise, allow the skeleton to move
                        }
                        else
                        {
                            Move();
                        }
                        break;

                    case SkeletonStates.Idle:
                        // After 0.4 seconds, change the state to Aggro and 
                        // increase the movement speed of the skeleton
                        if (StateCounter1 > 0.4)
                        {
                            speed = 5;
                            State1 = SkeletonStates.Aggro;
                            StateCounter1 = 0;
                            // Otherwise, continue to count the elapsed game time
                        }
                        else
                        {
                            StateCounter1 += game.ElapsedMilliseconds;
                        }
                        break;

                    case SkeletonStates.Aggro:
                        // If the rectangle between the player and the skeleton
                        // is no longer within the patrol space of the skeletion,
                        // change the state back to Move
                        if (WithinLineOfSight(game.Player))
                        {
                            Move();

                            // If the skeleton hasn't deal damage to the player,
                            // check for collision - then deal damage if it collides
                            if (Collides(game.Player))
                            {
                                if (!game.Player.IsInvincible)
                                {
                                    game.Player.DealDamage(3);
                                    game.Player.StartInvincibility();
                                }

                            }
                            // Otherwise, allow the skeleton to move
                        }
                        else
                        {
                            State1 = SkeletonStates.Move;
                            speed = 2;
                        }
                        break;
                }
            }
        }

        public override void Reset()
        {
            // Base reset
            base.Reset();

            // Reset other values
            originX = position.X;
            direction = 1;
        }

        /// <summary>
        /// Move the Skeleton in a simple horizontal patrol
        /// pattern
        /// </summary>
        public override void Move()
        {
            ApplyGravity(Gravity);

            originY = position.Y;

            patrolArea = new Rectangle(originX, originY, 
                patrolDistance + this.position.Width, position.Height);

            // Adjust the x-position of the Skeleton until
            // it gets a certain distance away from its
            // starting position and then move it back to
            // the starting position
            if ((this.X <= (originX + patrolDistance)) && (direction == 1))
            {
                if (this.X + speed > originX + patrolDistance)
                {
                    this.X += speed;
                    direction = -1;
                } else
                {
                    this.X += speed;
                }
            } else if ((this.X > originX) && (direction == -1))
            {
                if (this.X - speed <= originX)
                {
                    this.X -= speed;
                    direction = 1;
                } else
                {
                    this.X -= speed;
                }
            }
        }

        /// <summary>
        /// Change the appearance of the Skeleton, based on what state it
        /// is in
        /// </summary>
        /// <param name="currentState">The current state of the 
        /// Skeleton</param>
        /// <param name="sb">The SpriteBatch with which to draw</param>
        public void DrawState(SkeletonStates currentState, SpriteBatch sb, int camX)
        {
            if (currentState == SkeletonStates.Move)
            {
                sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
            }
            else if (currentState == SkeletonStates.Idle)
            {
                sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.Purple);
            }
            else
            {
                sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.Red);
            }
        }

        /// <summary>
        /// Allows the Skeleton to determine if the player is within their
        /// line of sight
        /// </summary>
        public bool WithinLineOfSight(Player player)
        {           
            // Determine if the player is within this rectangle, and thus in the Skeleton's
            // line of sight
            if (player.Position.Intersects(patrolArea))
            {
                return true;
            }

            return false;
        }
    }
}
