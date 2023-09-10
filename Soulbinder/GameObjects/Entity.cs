using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soulbinder
{
    public enum CollisionSide
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public class Entity : GameObject
    {
        // Fields
        protected int speed;
        protected int healthCur;
        protected int healthMax;
        protected CollisionSide collisionSide;
        private Vector2 velocity;
        private Vector2 jumpVelocity;

        // Properties
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int CurrentHealth
        {
            get { return healthCur; }
            set { healthCur = value; }
        }

        public int MaximumHealth
        {
            get { return healthMax; }
            set { healthMax = value; }
        }

        public CollisionSide CollisionSide
        {
            get { return collisionSide; }
            set { collisionSide = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 JumpVelocity
        {
            get { return jumpVelocity; }
            set { jumpVelocity = value; }
        }

        // Constructor
        public Entity(Texture2D sprite, Rectangle position, int speed, int healthCur, int healthMax) : base(sprite, position)
        {
            this.speed = speed;
            this.healthCur = healthCur;
            this.healthMax = healthMax;
            velocity = Vector2.Zero;
            jumpVelocity = new Vector2(0, -15.0f);
        }

        // Methods
        /// <summary>
        /// Allow the entity to reset to it's original position with maximum health
        /// </summary>
        public virtual void Reset()
        {
            healthCur = healthMax;
            position = originalPosition;
        }

        /// <summary>
        /// Checks if the Entity collides with a GameObject
        /// </summary>
        /// <param name="obj">The GameObject to check a collision with</param>
        /// <returns>True if the Entity's Rectangle collides with the GameObject's, false if not</returns>
        public bool Collides(GameObject obj)
        {
            if(obj == null)
            {
                return false;
            }

            if (Position.Intersects(obj.Position))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Checks if the Entity collides with another Entity
        /// </summary>
        /// <param name="ent">The Entity to check a collision with</param>
        /// <returns>True if the Entity's Rectangle collides with the Entity's, false if not</returns>
        public virtual bool Collides(Entity ent)
        {
            if(ent == null)
            {
                return false;
            }

            if (Position.Intersects(ent.Position))
            {
                return true;
            }

            else return false;
        }

        /// <summary>
        /// checks the collision between a circle and a this entity's rectangle
        /// does this by making a square based off the circle, and checking if the square collides with the rectangle
        /// </summary>
        /// <param name="circleCenter"></param>
        /// <param name="radius"></param>
        /// <returns>true if collision, false otherwise</returns>
        public bool CircleRectCollides(Vector2 circleCenter, float radius)
        {
            // turn the circle hit box into a rectangle one
            Rectangle circleSquare = new Rectangle((int)(circleCenter.X - radius), (int)(circleCenter.Y - radius), 
                (int)radius * 2, (int)radius * 2);

            // check the intersection between rects
            return position.Intersects(circleSquare);

        }

        /// <summary>
        /// Checks if the Entity is colliding with a certain hitbox
        /// </summary>
        /// <param name="hitBox">The hitbox that the player might collide with</param>
        /// <returns>True if the player collides with the hitbox, false if not</returns>
        public virtual bool Collides(Rectangle hitBox)
        {
            if (Position.Intersects(hitBox))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Checks if the Entity is colliding with a tile
        /// </summary>
        /// <param name="tile">The Tile to check a collision with</param>
        /// <returns>True if the Entity's Rectangle collides with the tile, false if not</returns>
        public virtual bool Collides(Rectangle tile, Vector2 movement)
        {
            // Create a new Rectangle encapsulates all the space inbetween the player's current rectangle and their future position
            Rectangle futurePosition = new Rectangle(Position.X, Position.Y, Position.Width + (int)movement.X, Position.Height + (int)movement.Y);

            if (futurePosition.Intersects(tile))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Applies gravity to the Entity
        /// </summary>
        /// <param name="gravity">The Vector2 of gravity to apply</param>
        public virtual void ApplyGravity(Vector2 gravity)
        {
            Velocity += gravity;
            Y += (int)Velocity.Y;
            X += (int)Velocity.X;
        }

        /// <summary>
        /// Resolves an Entity's collisions with a list of Rectangles
        /// </summary>
        /// <param name="obstacleRects">A list of Rectangles to check for collisions with</param>
        public virtual void ResolveCollisions(List<Rectangle> obstacleRects)
        {
            CollisionSide collision = CollisionSide.None;
            Rectangle playerRect = Position;

            // Loop through each potential collidable object
            for (int i = 0; i < obstacleRects.Count; i++)
            {
                // Check for player collisions with each of the objects
                if (Collides(obstacleRects[i], Velocity))
                {
                    // Get the overlapping rectangle between their collision
                    Rectangle collisionBox = Rectangle.Intersect(Position, obstacleRects[i]);

                    // If the width is less than or equal to the height,
                    // then the character is colliding through the side
                    if (collisionBox.Width <= collisionBox.Height)
                    {
                        // If the difference between the x values of the colliding boxes
                        // is greater than 0, that means the platform is to the right of the 
                        // player - so move the character to the left
                        if (obstacleRects[i].X - playerRect.X > 0)
                        {
                            playerRect.X -= collisionBox.Width;
                            collision = CollisionSide.Right;
                        }
                        else if (obstacleRects[i].X - playerRect.X < 0)
                        {
                            // Otherwise, the player is to the left of the obstacle
                            // so move the character to the right
                            playerRect.X += collisionBox.Width;
                            collision = CollisionSide.Left;
                        }
                    }
                    else if (collisionBox.Height < collisionBox.Width)
                    {
                        // Set player velocity to 0 to represent collision
                        Velocity = Vector2.Zero;

                        // If the difference between the y values of the colliding boxes
                        // is less than 0, that means the platform is above the player
                        // so move the character down
                        if (obstacleRects[i].Y - playerRect.Y < 0)
                        {
                            playerRect.Y += collisionBox.Height;
                            collision = CollisionSide.Up;
                        }
                        else if (obstacleRects[i].Y - playerRect.Y > 0)
                        {
                            // Otherwise, the player is above the obstacle, so move
                            // the character up
                            playerRect.Y -= collisionBox.Height;
                            collision = CollisionSide.Down;
                        }
                    }
                } else
                {
                    // Otherwise, set CollisionSide to none
                    collisionSide = CollisionSide.None;
                }
            }

            // Save the player's position
            X = playerRect.X;
            Y = playerRect.Y;

            collisionSide = collision;
        }
    }
}
