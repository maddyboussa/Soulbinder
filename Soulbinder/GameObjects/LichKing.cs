using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soulbinder
{
    // Lich King state enum
    public enum LichKingStates
    {
        Idle,
        Attack,
    }

    // Ben Sultzer
    // Class: LichKing
    // Purpose: Represents the final boss
    // with much more varied attacks and the
    // ability to spawn other enemies
    // Restrictions: None
    class LichKing : Boss
    {
        // Fields
        private int handOriginY;
        private int rightHandOriginX;
        private int leftHandOriginX;
        private Entity head;
        private Entity rightHand;
        private Entity leftHand;
        private List<Entity> hands;
        private Vector2 slamGravity;
        private bool keepSwiping;
        private int randomHand;
        private Texture2D orbSprite;
        private Texture2D enemySprite;
        private List<Projectile> orbs;
        private int[] orbDirections;
        private List<Skeleton> enemies;
        private int[] enemyDirections;
        private Random random;
        private List<Vector2> orbTrajectories;

        // Lich king states and state counter/
        // data tracking variables
        private LichKingStates lichKingState;
        private double lichKingStateCounter;
        private double raiseHandsCounter;
        private bool lichKingSlam;
        private int randomHandUpdate;
        private int randomAttackLichKing;
        private bool dealtDamage;

        /// <summary>
        /// Get only property for accessing the head of the Lich
        /// King
        /// </summary>
        public Entity Head
        {
            get
            {
                return head;
            }
        }

        /// <summary>
        /// Get only property for accessing the right hand of the
        /// Lich King
        /// </summary>
        public Entity RightHand
        {
            get
            {
                return rightHand;
            }
        }

        /// <summary>
        /// Get only property for accessing the left hand of the
        /// Lich King
        /// </summary>
        public Entity LeftHand
        {
            get
            {
                return leftHand;
            }
        }

        /// <summary>
        /// Property for getting the slam speed of the Lich King
        /// </summary>
        public Vector2 SlamGravity
        {
            get
            {
                return slamGravity;
            }
        }

        /// <summary>
        /// Property for getting the original y-position of the
        /// hands before a slam attack
        /// </summary>
        public int HandOriginY
        {
            get
            {
                return handOriginY;
            }
        }

        /// <summary>
        /// Property for getting the original x-position
        /// of the right hand
        /// </summary>
        public int RightHandOriginX
        {
            get
            {
                return rightHandOriginX;
            }
        }

        /// <summary>
        /// Property for getting the original x-position
        /// of the left hand
        /// </summary>
        public int LeftHandOriginX
        {
            get
            {
                return leftHandOriginX;
            }
        }

        /// <summary>
        /// Property for getting the Lich King's list
        /// of projectiles
        /// </summary>
        public List<Projectile> Orbs
        {
            get
            {
                return orbs;
            }
        }

        /// <summary>
        /// Property for getting the array of 
        /// directions at which to shoot the
        /// orbs (diagonally right or diagonally
        /// left)
        /// </summary>
        public int[] OrbDirections
        {
            get
            {
                return orbDirections;
            }
        }

        /// <summary>
        /// Property for getting the Lich King's list
        /// of Skeletons
        /// </summary>
        public List<Skeleton> Enemies
        {
            get
            {
                return enemies;
            }
        }

        /// <summary>
        /// Property for getting the array of 
        /// directions at which to spawn and move
        /// the the enemies (right or left)
        /// </summary>
        public int[] EnemyDirections
        {
            get
            {
                return enemyDirections;
            }
        }

        /// <summary>
        /// Property for getting and setting whether 
        /// or not the Lich King should keep swiping
        /// </summary>
        public bool KeepSwiping
        {
            get
            {
                return keepSwiping;
            }

            set
            {
                keepSwiping = value;
            }
        }

        /// <summary>
        /// Property for getting the set of launch directions
        /// for each orb projectile
        /// </summary>
        public List<Vector2> OrbTrajectories
        {
            get
            {
                return orbTrajectories;
            }
        }

        /// <summary>
        /// Constructor for initializing the Lich King's fields
        /// </summary>
        /// <param name="sprite">The image texture of the
        /// Lich King's head</param>
        /// <param name="position">The rectangle position of
        /// the Lich King</param>
        /// <param name="speed">The movement speed of the 
        /// Lich King</param>
        /// <param name="healthCur">The current health of
        /// the Lich King</param>
        /// <param name="healthMax">The max health of the
        /// Lich King</param>
        /// <param name="handSprite">The image texture of the
        /// Lich King's hand</param>
        /// <param name="orbSprite">The image texture of the
        /// Lich King's projectiles</param>
        /// <param name="head">The Lich King's head 
        /// entity</param>
        /// <param name="rightHand">The Lich King's right hand
        /// entity</param>
        /// <param name="leftHand">The Lich King's left hand 
        /// entity</param>
        public LichKing(Texture2D headSprite, Rectangle position, int speed,
            int healthCur, int healthMax, Texture2D handSprite, Texture2D orbSprite,
            Texture2D enemySprite, Entity head, Entity rightHand, Entity leftHand)
            : base(headSprite, position, speed, healthCur, healthMax)
        {
            this.head = head;
            Position = head.Position;
            this.rightHand = rightHand;
            this.leftHand = leftHand;
            handOriginY = rightHand.Y;
            rightHandOriginX = rightHand.X;
            leftHandOriginX = leftHand.X;
            slamGravity = new Vector2(0, 2.5f);
            keepSwiping = true;
            this.orbSprite = orbSprite;
            this.enemySprite = enemySprite;
            orbs = new List<Projectile>();
            enemies = new List<Skeleton>();
            random = new Random();
            hands = new List<Entity> { rightHand, leftHand };
            orbTrajectories = new List<Vector2>();
            Name = "Lich King";

            // Set the time for the lich king states and the time for the
            // lich king to raise its hands to 0, as well as set the Boolean
            // to reset the raise hands counter to false
            lichKingStateCounter = 0;
            raiseHandsCounter = 0;
            lichKingSlam = false;

            // Initialize the random number generator
            random = new Random();
        }

        /// <summary>
        /// Allows the Lich King to slam its hands on the ground
        /// and raise them back up after a slight delay
        /// </summary>
        /// <param name="raiseHandsCounter">The amount of time
        /// used to determine if the Lich King can raise its
        /// hands</param>
        /// <returns>Returns true if the hands have been raised to 
        /// their original height and thus the counter can be reset 
        /// for the next attack. False otherwise</returns>
        public bool Slam(double raiseHandsCounter)
        {
            if (raiseHandsCounter > 0.5 && leftHand.Y >= handOriginY && rightHand.Y >= handOriginY)
            {
                leftHand.Y -= speed;
                rightHand.Y -= speed;
                return true;
            }
            else if (raiseHandsCounter < 0.5)
            {
                rightHand.ApplyGravity(slamGravity);
                leftHand.ApplyGravity(slamGravity);
            }

            return false;
        }

        /// <summary>
        /// Allows the Lich King to slam his hands on the ground,
        /// drag them across the stage, and then raise them again
        /// </summary>
        /// <param name="lowerHand"></param>
        /// <returns></returns>
        public void Swipe(int lowerHand)
        {
            // Track the previous x of the lower
            // swiping hand
            int previousSwipeX;

            // Determine which hand should swipe lower
            if (lowerHand == 0)
            {
                previousSwipeX = rightHand.X;

                // Continue swiping until both hands have reached
                // their end points
                if (keepSwiping)
                {
                    // Have the right hand slam down to the ground
                    rightHand.ApplyGravity(slamGravity);

                    // When the right hand hits the ground, move it
                    // to the left
                    if (rightHand.CollisionSide == CollisionSide.Down)
                    {
                        rightHand.X -= speed * 4;
                    }

                    // Once the right hand starts moving, move the
                    // left hand the opposite direction
                    if (rightHand.X < previousSwipeX)
                    {
                        leftHand.X += speed * 4;
                    }

                    // Once the left hand hits the opposite wall, both
                    // hands have reached their endpoints, so stop
                    // swiping
                    if (leftHand.CollisionSide == CollisionSide.Right)
                    {
                        keepSwiping = false;
                    }
                }
                else
                {
                    // Move the right hand back to its original
                    // y-position
                    if (rightHand.Y > handOriginY)
                    {
                        rightHand.Y -= speed;
                    }

                    // Move the right hand back to its original
                    // x-position
                    if (rightHand.X < rightHandOriginX)
                    {
                        rightHand.X += speed * 4;
                    }

                    // Move the left hand back to its original 
                    // x-position
                    if (leftHand.X > leftHandOriginX)
                    {
                        leftHand.X -= speed * 4;
                    }
                }
            }
            else
            {
                previousSwipeX = leftHand.X;

                // Continue swiping until both hands have reached
                // their end points
                if (keepSwiping)
                {
                    // Have the left hand slam down to the ground
                    leftHand.ApplyGravity(slamGravity);

                    // When the left hand hits the ground, move it
                    // to the right
                    if (leftHand.CollisionSide == CollisionSide.Down)
                    {
                        leftHand.X += speed * 4;
                    }

                    // Once the left hand starts moving, move the
                    // right hand the opposite direction
                    if (leftHand.X > previousSwipeX)
                    {
                        rightHand.X -= speed * 4;
                    }

                    // Once the right hand hits the opposite wall, both
                    // hands have reached their endpoints, so stop
                    // swiping
                    if (rightHand.CollisionSide == CollisionSide.Left)
                    {
                        keepSwiping = false;
                    }
                }
                else
                {
                    // Move the left hand back to its original
                    // y-position
                    if (leftHand.Y > handOriginY)
                    {
                        leftHand.Y -= speed;
                    }

                    // Move the left hand back to its original
                    // x-position
                    if (leftHand.X > leftHandOriginX)
                    {
                        leftHand.X -= speed * 4;
                    }

                    // Move the right hand back to its original 
                    // x-position
                    if (rightHand.X < rightHandOriginX)
                    {
                        rightHand.X += speed * 4;
                    }
                }
            }
        }

        /// <summary>
        /// Generate a random number of orb projectiles
        /// to be fired from a random hand
        /// </summary>
        public void FireOrbs(Game1 game)
        {
            // Randomly generate the number of orbs
            int numOrbs = random.Next(5, 11);
            orbDirections = new int[numOrbs];
            int shootingHand;

            // Populate the List with the desired number of orbs
            for (int i = 0; i < numOrbs; i++)
            {
                shootingHand = random.Next(0, 2);
                if (shootingHand == 0)
                {
                    orbDirections[i] = -1;
                }
                else
                {
                    orbDirections[i] = 1;
                }
                orbs.Add(new Projectile(orbSprite, new Rectangle(
                    hands[shootingHand].X + random.Next(0, 200),
                    hands[shootingHand].Y + random.Next(0, 200),
                    50, 50), 5, 0, 0, 4));               
            }
        }

        /// <summary>
        /// Sets the trajetory direction for each orb projectile
        /// </summary>
        /// <param name="player">The Player will be the Entity at which the direction
        /// points</param>
        public void GetOrbTrajectory(int playerX, int playerY)
        {
            // Loop through the list of orb directions and create a 
            // trajectory for each orb
            for (int i = 0; i < orbDirections.Length; i++)
            {
                // The orb is coming out of the right hand so the orb
                // must travel left, otherwise it is coming out of the left 
                // hand and must travel right
                if (orbDirections[i] == -1)
                {
                    // Get a vector from the in front of the player to the
                    // current orb and add it to the list of trajectories
                    orbTrajectories.Add(new Vector2(orbs[i].X - (playerX + 10),
                        playerY - orbs[i].Y));
                }
                else
                {
                    // Get a vector from the in front of the player to the
                    // current orb and add it to the list of trajectories
                    orbTrajectories.Add(new Vector2((playerX + 10) - orbs[i].X,
                        playerY - orbs[i].Y));
                }

                // Normalize the vector
                double trajectoryMagnitude = Math.Sqrt((Math.Pow(orbTrajectories[i].X, 2)) +
                    (Math.Pow(orbTrajectories[i].Y, 2)));

                // Store the new components in temporary x and y variables
                float tempX = orbTrajectories[i].X / (float)trajectoryMagnitude;
                float tempY = orbTrajectories[i].Y / (float)trajectoryMagnitude;

                // Store the new vector
                orbTrajectories[i] = new Vector2(tempX, tempY);
            }
        }

        /// <summary>
        /// Functions exactly like FireOrbs() except
        /// a random number of enemies are spawned from
        /// each hand
        /// </summary>
        public void SpawnEnemies()
        {
            // Randomly generate the number of enemies
            int numEnemies = random.Next(3, 6);
            enemyDirections = new int[numEnemies];
            int spawningHand;

            // Populate the List with the desired number of enemies
            for (int i = 0; i < numEnemies; i++)
            {
                spawningHand = random.Next(0, 2);
                if (spawningHand == 0)
                {
                    enemyDirections[i] = -1;
                }
                else
                {
                    enemyDirections[i] = 1;
                }
                enemies.Add(new Skeleton(enemySprite, new Rectangle(
                    hands[spawningHand].X + random.Next(0, 200),
                    hands[spawningHand].Y + random.Next(0, 200),
                    32, 64), 2, 0, 0, 100));
            }
        }

        /// <summary>
        /// Updates the Lich King states
        /// </summary>
        /// <param name="game">The game variable for accessing
        /// all the necessary data</param>
        public override void Update(Game1 game)
        {
            // Set player projectile list
            game.Player.ProjectileList = orbs;

            // Resolve collisions for the lich king's hands
            rightHand.ResolveCollisions(game.CurrentLevel.Collisions);
            leftHand.ResolveCollisions(game.CurrentLevel.Collisions);

            switch (lichKingState)
            {
                case LichKingStates.Idle:
                    // Reset the list of orbs for the next time the orbs attack
                    // is chosen
                    orbs.Clear();

                    // Transition the state to Attack
                    if (lichKingStateCounter > 1.5)
                    {
                        lichKingState = LichKingStates.Attack;

                        // Test to see if the lich king needs to spawn enemies 
                        // because it is at half health, in addition to its other
                        // attacks, otherwise choose a random attack
                        if (CurrentHealth < MaximumHealth / 2)
                        {
                            randomAttackLichKing = random.Next(0, 4);
                        }
                        else
                        {
                            randomAttackLichKing = random.Next(0, 3);
                        }

                        // Choose a random hand for the swipe attack, reset the 
                        // state transition counter and populate the orb list and
                        // get the projectile trajectory for if the random attack
                        // is a projectile attack
                        randomHand = random.Next(0, 2);
                        lichKingStateCounter = 0;
                        FireOrbs(game);
                        GetOrbTrajectory(game.Player.X, game.Player.Y);
                        SpawnEnemies();
                    }
                    else
                    {
                        // Add the currently elapsed game time to count down
                        // until the state needs to be changed to Attack
                        lichKingStateCounter += game.ElapsedSeconds;
                    }
                    break;
                case LichKingStates.Attack:
                    // Perform a slam attack
                    if (randomAttackLichKing == 0)
                    {
                        // Add the current game time to the counter for raising
                        // the lich king's hands
                        raiseHandsCounter += game.ElapsedSeconds;

                        lichKingSlam = Slam(raiseHandsCounter);

                        // If the player is on the ground during a slam, deal
                        // damage to the player
                        if (!dealtDamage)
                        {
                            if (game.Player.CollisionSide == CollisionSide.Down &&
                                rightHand.CollisionSide == CollisionSide.Down &&
                                leftHand.CollisionSide == CollisionSide.Down)
                            {
                                game.Player.DealDamage(4);
                                game.Player.StartInvincibility();
                                dealtDamage = true;
                            }
                        }
                        // Perform a swipe attack
                    }
                    else if (randomAttackLichKing == 1)
                    {
                        Swipe(randomHand);

                        // If the player overlaps a hand, deal damage to the player
                        if (!dealtDamage)
                        {
                            if (game.Player.Position.Intersects(rightHand.Position) ||
                                game.Player.Position.Intersects(leftHand.Position))
                            {
                                game.Player.DealDamage(4);
                                game.Player.StartInvincibility();
                                dealtDamage = true;
                            }
                        }
                        // Perform a projectile attack
                    }
                    else if (randomAttackLichKing == 2)
                    {
                        // Loop through the list of orbs and fire them at the 
                        // player
                        for (int i = 0; i < orbs.Count; i++)
                        {
                            // If the current orbs direction is -1, this means
                            // it is coming out of the right hand and should travel
                            // left
                            if (orbDirections[i] == -1)
                            {
                                orbs[i].X -= (int)(orbTrajectories[i].X * orbs[i].Speed);
                                orbs[i].Y += (int)(orbTrajectories[i].Y * orbs[i].Speed);
                            }
                            // Otherwise the current orb is coming out of the
                            // left hand and should travel right
                            else
                            {
                                orbs[i].X += (int)(orbTrajectories[i].X * orbs[i].Speed);
                                orbs[i].Y += (int)(orbTrajectories[i].Y * orbs[i].Speed);
                            }

                            // Test if any of the orbs collide with the player and
                            // if they do, deal damage to the player, start the
                            // player's invincibility recovery phase, and despawn the
                            // projectile
                            if (orbs[i].Collides(game.Player))
                            {
                                game.Player.DealDamage(orbs[i].Damage);
                                game.Player.StartInvincibility();

                                orbs.Remove(orbs[i]);
                            }
                        }
                        // Spawn enemies at less than half health
                    }
                    else
                    {
                        // Loop through the list of enemies and spawn them
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            // Resolve collisions for each enemy
                            enemies[i].ResolveCollisions(game.CurrentLevel.Collisions);

                            // Gravity will control the enemy's vertical movement
                            enemies[i].ApplyGravity(game.Gravity);

                            // If the player is to the right of the enemies, move
                            // the enemies to the right, otherwise move the enemies
                            // to the left
                            if (game.Player.X > enemies[i].X)
                            {
                                enemies[i].X += enemies[i].Speed;
                            }
                            else
                            {
                                enemies[i].X -= enemies[i].Speed;
                            }

                            // Test if any of the enemies collide with the player and
                            // if they do, deal damage to the player, start the
                            // player's invincibility recovery phase, and despawn the
                            // enemy
                            if (enemies[i].Collides(game.Player))
                            {
                                game.Player.DealDamage(4);
                                game.Player.StartInvincibility();

                                enemies.Remove(enemies[i]);
                            }
                        }
                    }

                    // Add the currently elapsed game time to count down until
                    // the state needs to be changed back to Idle
                    lichKingStateCounter += game.ElapsedSeconds;

                    // Transition the state back to Idle
                    if (lichKingStateCounter > 6)
                    {
                        // Reset the counter for raising the lich king's hands,
                        // whether or not the lich king should keep swiping, and 
                        // whether or not the lich king has dealt damage so it only
                        // deals damage once when slamming or swiping
                        lichKingState = LichKingStates.Idle;
                        raiseHandsCounter = 0;
                        keepSwiping = true;
                        dealtDamage = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws the Lich King and its projectiles and enemies
        /// to the screen
        /// </summary>
        /// <param name="game">The game variable to access the
        /// needed data</param>
        public override void Draw(Game1 game)
        {
            switch (lichKingState)
            {
                case LichKingStates.Idle:
                    game.SpriteBatch.Draw(game.SpriteManager.LichKingSprite,
                        new Rectangle(head.Position.X - game.Camera, head.Position.Y, 
                        head.Position.Width, head.Position.Height), Color.White);
                    game.SpriteBatch.Draw(game.SpriteManager.LichKingHandRightFist, 
                        new Rectangle(rightHand.Position.X - game.Camera, rightHand.Position.Y, 
                        rightHand.Position.Width, rightHand.Position.Height), Color.White);
                    game.SpriteBatch.Draw(game.SpriteManager.LichKingHandLeftFist, 
                        new Rectangle(leftHand.Position.X - game.Camera, leftHand.Position.Y, 
                        leftHand.Position.Width, leftHand.Position.Height), Color.White);
                    break;
                case LichKingStates.Attack:
                    game.SpriteBatch.Draw(game.SpriteManager.LichKingSprite, 
                        new Rectangle(head.Position.X - game.Camera, head.Position.Y, 
                        head.Position.Width, head.Position.Height), Color.White);
                    if (randomAttackLichKing == 1)
                    {
                        game.SpriteBatch.Draw(game.SpriteManager.LichKingHandRightOpen,
                            new Rectangle(rightHand.Position.X - game.Camera, rightHand.Position.Y,
                            rightHand.Position.Width, rightHand.Position.Height), Color.White);
                        game.SpriteBatch.Draw(game.SpriteManager.LichKingHandLeftOpen,
                            new Rectangle(leftHand.Position.X - game.Camera, leftHand.Position.Y,
                            leftHand.Position.Width, leftHand.Position.Height), Color.White);
                    }
                    else if (randomAttackLichKing == 2)
                    {
                        game.SpriteBatch.Draw(game.SpriteManager.LichKingHandRightOpen,
                            new Rectangle(rightHand.Position.X - game.Camera, rightHand.Position.Y,
                            rightHand.Position.Width, rightHand.Position.Height), Color.White);
                        game.SpriteBatch.Draw(game.SpriteManager.LichKingHandLeftOpen,
                            new Rectangle(leftHand.Position.X - game.Camera, leftHand.Position.Y,
                            leftHand.Position.Width, leftHand.Position.Height), Color.White);
                        for (int i = 0; i < orbs.Count; i++)
                        {
                            orbs[i].Draw(game.SpriteBatch, game.Camera, Color.White);
                        }
                    }
                    else if (randomAttackLichKing == 3)
                    {
                        game.SpriteBatch.Draw(game.SpriteManager.LichKingHandRightOpen,
                            new Rectangle(rightHand.Position.X - game.Camera, rightHand.Position.Y,
                            rightHand.Position.Width, rightHand.Position.Height), Color.White);
                        game.SpriteBatch.Draw(game.SpriteManager.LichKingHandLeftOpen,
                            new Rectangle(leftHand.Position.X - game.Camera, leftHand.Position.Y,
                            leftHand.Position.Width, leftHand.Position.Height), Color.White);
                        for (int i = 0; i < enemies.Count; i++)
                        {
                            enemies[i].Draw(game.SpriteBatch, game.Camera, Color.White);
                        }
                    }
                    else
                    {
                        game.SpriteBatch.Draw(game.SpriteManager.LichKingHandRightFist,
                            new Rectangle(rightHand.Position.X - game.Camera, rightHand.Position.Y,
                            rightHand.Position.Width, rightHand.Position.Height), Color.White);
                        game.SpriteBatch.Draw(game.SpriteManager.LichKingHandLeftFist,
                            new Rectangle(leftHand.Position.X - game.Camera, leftHand.Position.Y,
                            leftHand.Position.Width, leftHand.Position.Height), Color.White);
                    }
                    break;
            }
        }
    }
}
