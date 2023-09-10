using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soulbinder
{
    // Boss phase enum
    public enum BossStates
    {
        Move,
        Idle,
        Attack,
        SimultaneousAttack,
        SequenceAttack
    }

    // Boss pathfinding enum
    public enum PathfindingStates
    {
        Move,
        CheckVertical,
        CheckHorizontal
    }

    // Executioner attack enum
    public enum ExecutionerAttack
    {
        None,
        Horizontal,
        Slam,
        Cleave
    }

    // Ben Sultzer
    // Class: Boss
    // Purpose: Represents the larger and 
    // more difficult enemies with complex
    // movement and attack patterns
    // Restrictions: None
    public class Boss : Enemy
    {
        // Fields
        private string name;
        
        private Vector2 bossJump;
        public double jumpCounter;
        private Rectangle lineOfSightLeft;
        private Rectangle lineOfSightRight;

        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Constructor for initializing the Boss's fields
        /// </summary>
        /// <param name="sprite">The image texture of the
        /// Boss</param>
        /// <param name="position">The rectangle position of
        /// the Boss</param>
        /// <param name="speed">The movement speed of the 
        /// Boss</param>
        /// <param name="healthCur">The current health of
        /// the Boss</param>
        /// <param name="healthMax">The max health of the
        /// Boss</param>
        public Boss(Texture2D sprite, Rectangle position, int speed,
            int healthCur, int healthMax)
            : base(sprite, position, speed, healthCur, healthMax)
        {
            dead = false;
            bossJump = new Vector2(0, -13.5f);
            jumpCounter = 0;
        }

        public override void Update(Game1 game)
        {
            base.Update(game);
        }

        public override void Draw(Game1 game)
        {
            base.Draw(game);
        }

        public virtual void DrawShapes(Game1 game)
        {
            
        }

        public virtual void Reset()
        {
            CurrentHealth = MaximumHealth;
        }

        /// <summary>
        /// Keeps the Boss constantly moving while in the Move
        /// phase, attempting to get into attacking range of the 
        /// player
        /// </summary>
        public override void Move(int targetPositionX, int targetPositionY)
        {
            // Adjust the horizontal position of the Boss overtime
            // to chase the player
            if (this.X < targetPositionX)
            {
                this.X += speed;
            }
            else if (this.X > targetPositionX)
            {
                this.X -= speed;
            }
        }

        /// <summary>
        /// Generic method to allow the boss to attack after a transition 
        /// from the Move phase to the Attack phase
        /// </summary>
        public virtual bool Attack(Player player, StatusBar playerHealthBar)
        {
            return true;
        }

        /// <summary>
        /// Allows the Boss to reach different heights when chasing the player
        /// </summary>
        public void Jump()
        {
            Velocity = bossJump;

            ApplyGravity(Gravity);
        }

        /// <summary>
        /// Change the appearance of the Boss, based on what state it
        /// is in
        /// </summary>
        /// <param name="currentState">The current state of the 
        /// Boss</param>
        /// <param name="sb">The SpriteBatch with which to draw</param>
        public virtual void DrawState(BossStates currentState, SpriteBatch sb, Texture2D hitBoxTexture, int camX, Game1 game)
        {
            if (currentState == BossStates.Move || 
                currentState == BossStates.Idle)
            {
                sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
            }
        }

        /// <summary>
        /// Overrides the Entity collision detection method to 
        /// incorporate pathfinding adjustment
        /// </summary>
        /// <param name="obstacleRects"></param>
        public override void ResolveCollisions(List<Rectangle> obstacleRects)
        {
            Rectangle bossRect = Position;

            // Loop through each potential collidable object
            for (int i = 0; i < obstacleRects.Count; i++)
            {
                // Check for Boss collisions with each of the objects
                if (Position.Intersects(obstacleRects[i]))
                {
                    // Get the overlapping rectangle between their collision
                    Rectangle collisionBox = Rectangle.Intersect(Position, obstacleRects[i]);

                    // If the width is less than or equal to the height,
                    // then the Boss is colliding through the side
                    if (collisionBox.Width <= collisionBox.Height)
                    {
                        // If the difference between the x values of the colliding boxes
                        // is greater than 0, that means the platform is to the right of the 
                        // Boss - so move the character to the left
                        if (obstacleRects[i].X - bossRect.X > 0)
                        {
                            bossRect.X -= collisionBox.Width;
                        }
                        else if (obstacleRects[i].X - bossRect.X < 0)
                        {
                            // Otherwise, the Boss is to the left of the obstacle
                            // so move the Boss to the right
                            bossRect.X += collisionBox.Width;
                        }
                    }
                    else if (collisionBox.Height < collisionBox.Width)
                    {
                        // Set Boss velocity to 0 to represent collision
                        Velocity = Vector2.Zero;

                        // If the difference between the y values of the colliding boxes
                        // is less than 0, that means the platform is above the Boss
                        // so move the character down
                        if (obstacleRects[i].Y - bossRect.Y < 0)
                        {
                            bossRect.Y += collisionBox.Height;
                        }
                        else if (obstacleRects[i].Y - bossRect.Y > 0)
                        {
                            // Otherwise, the Boss is above the obstacle, so move
                            // the Boss up
                            bossRect.Y -= collisionBox.Height;
                        }
                    }
                }
            }

            // Save the Boss's position
            X = bossRect.X;
            Y = bossRect.Y;
        }

        /// <summary>
        /// Determines whether the Boss collides with an obstacle on either
        /// the left or right during pathfinding
        /// </summary>
        /// <param name="tiles">The set of environmental tiles</param>
        /// <returns>A Boolean corresponding to whether or not the Boss
        /// as collided with an obstacle</returns>
        public bool ObstacleInWay(List<Rectangle> tiles)
        {
            lineOfSightLeft = new Rectangle(this.X + position.Width, this.Y,
                position.Width / 4, position.Height);
            lineOfSightRight = new Rectangle(this.X - position.Width / 2, this.Y,
                position.Width / 4, position.Height);

            bool isColliding = false;

            // Loop through each potential collidable object
            for (int i = 0; i < tiles.Count; i++)
            {
                // Check for collisions between the line of 
                // sight rectangles and the environment
                if (lineOfSightLeft.Intersects(tiles[i]) || lineOfSightRight.Intersects(tiles[i]))
                {
                    isColliding = true;
                }
            }

            // Return the result
            return isColliding;
        }
    }
}
