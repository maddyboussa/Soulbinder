using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Soulbinder
{
    public enum StatueAction
    {
        None,
        Basic,
        Dash,
        Projectile,
        Dodge
    }


    // Ben Sultzer
    // CLass: Statue
    // Purpose: Defines the second boss
    // of the game that will be three separate
    // statues that bahave in reaction to the player
    // and each other, with a basic attack, dash attack,
    // and projectile attack, potentially happening
    // in sequence or individually
    // Restrictions: None
    public class Statue : Boss
    {
        
        // Fields
        private Random rng;
        private StatueAction actionState;
        private List<Texture2D> attackAnimations;
        private Texture2D rockTexture;
        private List<Rectangle> hitBoxes;
        private bool locked;
        private bool attacking;
        private double lockTimer;
        private double dashTimer;
        private Vector2 rockVelocity;
        private Vector2 dashVector;
        private Vector2 dashLeft;
        private Vector2 dashRight;
        private int xDifference;
        private bool hasDashed;
        private CollisionSide bossCollisionSide;
        private Vector2 statueGravity;

        // Statue States
        private BossStates statueState;
        private PathfindingStates statuePathfind;
        private double statueStateCounter;
        private double jumpCounterStatue;

        // Attacking Variables
        private Point playerPosition;
        private Point centerPosition;
        private int randomAttack;

        // Dash fields
        private bool isDashing;
        private bool alreadyHit;
        private double dashAttackDuration;
        private bool dashEnded;

        // Projectile Fields
        private Projectile rock;
        private bool isThrowing;
        private bool hasCollided;
        private double throwAttackDuration;
        private bool throwEnded;

        // Properties
        public StatueAction ActionState { get { return actionState; } set { actionState = value; } }
        public bool Locked { get { return locked; } set { locked = value; } }
        public bool Attacking { get { return attacking; } set { attacking = value; } }
        public Vector2 RockVelocity { get { return rockVelocity; } set { rockVelocity = value; } }
        public bool IsThrowing { get { return isThrowing; }}
        public Projectile Rock { get { return rock; } }

        /// <summary>
        /// Property for getting and setting which
        /// side of the Boss potentially collides with
        /// the environment
        /// </summary>
        public CollisionSide BossCollisionSide { get { return bossCollisionSide; } set { bossCollisionSide = value; } }

        /// <summary>
        /// Set-only property for making sure
        /// the Statue dashes only once
        /// </summary>
        public bool HasDashed
        {
            set
            {
                hasDashed = value;
            }
        }

        /// <summary>
        /// Constructor for initializing the Statue's fields
        /// </summary>
        /// <param name="sprite">The image texture of the
        /// Statue</param>
        /// <param name="position">The rectangle position of
        /// the Statue</param>
        /// <param name="speed">The movement speed of the 
        /// Statue</param>
        /// <param name="healthCur">The current health of
        /// the Statue</param>
        /// <param name="healthMax">The max health of the
        /// Statue</param>
        public Statue(Texture2D sprite, Rectangle position, int speed,
            int healthCur, int healthMax, List<Texture2D> attackAnimations, Texture2D rockTexture) : base(sprite, position, speed, healthCur, healthMax)
        {
            Name = "Statue";

            rng = new Random();
            actionState = StatueAction.None;
            this.attackAnimations = attackAnimations;
            hitBoxes = new List<Rectangle>();
            locked = false;
            lockTimer = 0;
            dashTimer = 0;
            dashLeft = new Vector2(-5, -10f);
            dashRight = new Vector2(5, -10f);
            statueGravity = new Vector2(0, 0.001f);
            hasDashed = false;
            randomAttack = 0;

            // Statue States
            statueStateCounter = 0;
            jumpCounterStatue = 0;

            // Point fields
            playerPosition = new Point();
            centerPosition = new Point(X + Position.Width / 2, Y + Position.Height / 2);

            // Dash fields
            dashVector = Vector2.Zero;
            dashEnded = false;
            isDashing = false;
            dashAttackDuration = 250;

            // Projectile fields
            this.rockTexture = rockTexture;
            rock = new Projectile(rockTexture, new Rectangle(X, Y, rockTexture.Width / 4, rockTexture.Height / 4), 10, 5, 5, 5);
            rockVelocity = Vector2.Zero;
            isThrowing = false;
            throwEnded = false;
            hasCollided = false;
            throwAttackDuration = 1500;
        }

        // Methods
        public override void Update(Game1 game)
        {
            if (!dead)
            {
                ApplyGravity(Gravity);
                ResolveCollisions(game.CurrentLevel.Collisions);

                switch (statueState)
                {
                    case BossStates.Move:
                        // Reset the choosing of an attack
                        ResetAttack();

                        // Gravity will control the Boss' vertical movement
                        ApplyGravity(Gravity);

                        // Change the state to idle after 10 seconds
                        if (statueStateCounter > 10)
                        {
                            statueState = BossStates.Idle;
                            statueStateCounter = 0;
                        }
                        else
                        {
                            // Add to the jump counter the current elapsed game time
                            // to create a steady pathfinding jump
                            jumpCounterStatue += game.ElapsedSeconds;

                            // Add to the statue state counter the current elapsed
                            // game time to determine when to change states
                            statueStateCounter +=
                                game.ElapsedSeconds;

                            // Determine whether the Bosses need to find a route to the
                            // player

                            switch (statuePathfind)
                            {
                                case PathfindingStates.Move:
                                    // Check if the Bosses have collided with something 
                                    // from the right or left
                                    if (BossCollisionSide == CollisionSide.Right ||
                                        BossCollisionSide == CollisionSide.Left)
                                    {
                                        // Transition the state to check the Boss' vertical
                                        // position with the player's
                                        statuePathfind = PathfindingStates.CheckVertical;
                                    }

                                    // Move the Bosses at the pathfinding speed
                                    X += Speed;
                                    break;
                                case PathfindingStates.CheckVertical:
                                    // Check the Boss' vertical position against the player's
                                    // and jump if needed
                                    if (Y + Position.Height !=
                                        game.Player.Y && jumpCounterStatue > 1)
                                    {
                                        Jump();
                                        jumpCounterStatue = 0;
                                    }

                                    // Transition the state to check the Boss' horizontal
                                    // position against the player's
                                    statuePathfind = PathfindingStates.CheckHorizontal;
                                    break;
                                case PathfindingStates.CheckHorizontal:
                                    // Check the Boss' horizontal position against the player's
                                    // and change direction if needed
                                    if (X > game.Player.X)
                                    {
                                        Speed = -3;
                                    }
                                    else
                                    {
                                        Speed = 3;
                                    }

                                    // Transition the state back to continue moving
                                    statuePathfind = PathfindingStates.Move;
                                    break;
                            }
                        }
                        break;
                    case BossStates.Idle:
                        // Change the state to Attack after 1 second
                        if (statueStateCounter > 1)
                        {
                            statueState = BossStates.Attack;
                            // Increase the Boss' speed for an attack
                            Speed = 6;
                            statueStateCounter = 0;
                        }
                        else
                        {
                            // Prevent the Bosses from freezing in mid-air
                            // and continue to count the time until
                            // the state needs to be changed
                            ApplyGravity(Gravity);
                            statueStateCounter +=
                                game.ElapsedSeconds;
                        }
                        break;

                    case BossStates.Attack:
                        // Gravity will control the Boss' vertical movement
                        ApplyGravity(Gravity);

                        double distanceFromBossX = game.GameManager.CalcDistanceFromCenterX(game.Player, this);
                        double distanceFromBossY = game.GameManager.CalcDistanceFromCenterY(game.Player, this);
                        double requiredDistanceX = Position.Width / 2.0;
                        double requiredDistanceY = Position.Height / 2.0;

                        // Allow the Bosses to attack
                        if (distanceFromBossX > requiredDistanceX || distanceFromBossY > requiredDistanceY)
                        {
                            // Get the random attack for the player being out of range
                            if (!Attacking)
                            {
                                randomAttack = rng.Next(3, 5);
                                ChooseAttack(game.Player, randomAttack);
                            }

                        }
                        else if (distanceFromBossX <= requiredDistanceX && distanceFromBossY <= requiredDistanceY)
                        {
                            // Get the random attack for the player being in range
                            if (!Attacking)
                            {
                                randomAttack = rng.Next(1, 3);
                                ChooseAttack(game.Player, randomAttack);
                            }
                        }

                        // Perform the attack
                        Attack(game.Player, game.UIManager.HealthBar, game.ElapsedMilliseconds, game.Camera);

                        // Change back to the Move state after 2 seconds
                        if (statueStateCounter > 5)
                        {
                            // Reset the Boss state counter and Boss speed
                            // variables and whether or the Boss has dashed
                            // a single time this attack phase
                            statueState = BossStates.Move;
                            statueStateCounter = 0;
                            Speed = 3;
                            HasDashed = false;
                        }
                        else
                        {
                            // Continue to move the Boss and count the time
                            // until the state needs to be changed
                            statueStateCounter +=
                            game.ElapsedSeconds;

                            // Reset the speed and continue to move 
                            // the Boss
                            Move(game.Player.X, game.Player.Y);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Reset the boss
        /// </summary>
        public override void Reset()
        {
            // Call base reset
            base.Reset();

            // Reset other values
            attacking = false;
            actionState = StatueAction.None;
            locked = false;
            ResetAttack();
        }

        /// <summary>
        /// Reset necessary attack variables
        /// </summary>
        public void ResetAttack()
        {
            attacking = false;
            dashEnded = false;
            throwEnded = false;
            alreadyHit = false;
            hasCollided = false;
        }

        public override void Draw(Game1 game)
        {
            if (!dead)
            {
                DrawState(statueState, game.SpriteBatch, game.SpriteManager.Pixel, game.Camera, game);
            }
        }

        public override void DrawState(BossStates currentState, SpriteBatch sb, Texture2D hitboxTexture, int camX, Game1 game)
        {
            if (currentState == BossStates.Move ||
                currentState == BossStates.Idle)
            {
                sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
            }
            else
            {
                switch (actionState)
                {
                    case StatueAction.None:
                        sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
                        break;

                    case StatueAction.Basic:
                        sb.Draw(attackAnimations[0], new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
                        sb.Draw(hitboxTexture, new Rectangle(hitBoxes[0].X - camX, hitBoxes[0].Y, hitBoxes[0].Width, hitBoxes[0].Height), Color.Transparent);
                        break;

                    case StatueAction.Dash:
                        sb.Draw(attackAnimations[1], new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
                        sb.Draw(hitboxTexture, new Rectangle(hitBoxes[1].X - camX, hitBoxes[1].Y, hitBoxes[1].Width, hitBoxes[1].Height), Color.Transparent);
                        break;

                    case StatueAction.Projectile:
                        sb.Draw(attackAnimations[2], new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
                        if (isThrowing)
                        {
                            rock.Draw(sb, game.Camera);
                        }
                        break;

                    case StatueAction.Dodge:
                        sb.Draw(attackAnimations[3], new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
                        break;
                }
                ApplyGravity(statueGravity);
            }
        }

        public void ChooseAttack(Player player, int attackChosen)
        {
            // Set attacking to true
            attacking = true;

            // Determine which attack will be used
            switch (attackChosen)
            {
                case 0:
                    actionState = StatueAction.None;
                    break;

                case 1:
                    actionState = StatueAction.Basic;
                    break;

                case 2:
                    actionState = StatueAction.Dodge;
                    break;

                case 3:
                    actionState = StatueAction.Dash;
                    break;

                case 4:
                    actionState = StatueAction.Projectile;
                    break;
            }
        }

        /// <summary>
        /// Determine the attack to throw at the player
        /// </summary>
        /// <param name="player">The player to affect</param>
        /// <param name="playerHealthBar">The player healthbar</param>
        /// <param name="gravity">The vector of gravity</param>
        /// <param name="attackChosen">The number to represent the attack chosen</param>
        public void Attack(Player player, StatusBar playerHealthBar, double timeElapsed, int camX)
        {
            // Clear the hitboxes
            hitBoxes.Clear();

            // Draw hitboxes
            Rectangle basicStab = new Rectangle(Position.X, Position.Y + 100, Position.Width, Position.Y + 56);
            Rectangle dashStab = new Rectangle(Position.X, Position.Y, Position.Width, Position.Y);

            // Add them to the list of hitboxes
            hitBoxes.Add(basicStab);
            hitBoxes.Add(dashStab);

            // Set damage parameters
            int basicDamage = 4;
            int dashDamage = 6;

            // Set appropriate vectors
            playerPosition.X = player.X + camX;
            playerPosition.Y = player.Y;

            // Set statue position
            centerPosition.X = Position.X + Position.Width / 2;
            centerPosition.Y = Position.Y + Position.Height / 2;

            // Depending on the actionState, perform an action
            switch (actionState)
            {
                case StatueAction.None:
                    // Do nothing
                    break;

                case StatueAction.Basic:
                    // Perform the stab attack
                    if (player.Collides(hitBoxes[0]))
                    {
                        player.DealDamage(basicDamage);
                    }
                    break;

                case StatueAction.Dodge:
                    StepBack(player.X - X);
                    hasDashed = true;
                    break;

                case StatueAction.Dash:
                    if (player.Collides(hitBoxes[1]) && !alreadyHit)
                    {
                        player.DealDamage(dashDamage);
                        alreadyHit = true;
                    }

                    if (!isDashing && !dashEnded)
                    {
                        BeginDash(player);

                        // Set dash vector dash
                        dashVector = new Vector2(playerPosition.X - centerPosition.X, playerPosition.Y - centerPosition.Y);

                        // Normalize the dash vector so its length doesn't matter
                        double magnitude = Math.Sqrt(dashVector.X * dashVector.X + dashVector.Y * dashVector.Y);
                        dashVector.X /= (float)magnitude;
                        dashVector.Y /= (float)magnitude;

                        // Remove any current speed before the dash
                        Velocity = Vector2.Zero;
                    }
                    else if (isDashing)
                    {
                        ContinueDash(timeElapsed);

                        // update statue location based on dash vector
                        position.X += (int)(20 * dashVector.X);
                        position.Y += (int)(20 * dashVector.Y);
                    }
                    else if (dashEnded)
                    {
                        actionState = StatueAction.None;
                    }
                    break;

                case StatueAction.Projectile:
                    if (!isThrowing && !throwEnded)
                    {
                        rock.X = position.X + position.Width / 2;
                        rock.Y = position.Y + position.Height / 2;

                        // Begin the throw
                        BeginThrow(player);

                        // Set the rock's velocity
                        rockVelocity = new Vector2(playerPosition.X - centerPosition.X, playerPosition.Y - centerPosition.Y);

                        // Normalize the rock velocity vector so its length doesn't matter
                        double magnitude = Math.Sqrt(rockVelocity.X * rockVelocity.X + rockVelocity.Y * rockVelocity.Y);
                        rockVelocity.X /= (float)magnitude;
                        rockVelocity.Y /= (float)magnitude;
                    }
                    else if (isThrowing)
                    {
                        // Continue the throw
                        ContinueThrow(player, playerHealthBar, timeElapsed);

                        // Update the rock position
                        rock.X += (int)(20 * rockVelocity.X);
                        rock.Y += (int)(20 * rockVelocity.Y);
                    }
                    else if (throwEnded)
                    {
                        // Put the rock off screen
                        rock.X = -5000;
                        rock.Y = -5000;
                        actionState = StatueAction.None;
                    }
                    break;
            }
        }

        /// <summary>
        /// Begin the dash by locking the statue
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void BeginDash(Player player)
        {
            // Store player position
            playerPosition.X = player.X + player.Position.Width / 2;
            playerPosition.Y = player.Y + player.Position.Height / 2;

            // Update center position
            centerPosition.X = X + position.Width / 2;
            centerPosition.Y = Y + position.Width / 2;

            // Update dashing variables
            dashEnded = false;
            isDashing = true;
            alreadyHit = false;
            dashAttackDuration = 250;
        }

        /// <summary>
        /// Continue the dash until time runs out
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void ContinueDash(double elapsedTime)
        {
            // Tick dash attack duration
            dashAttackDuration -= elapsedTime;

            if (dashAttackDuration <= 0 || alreadyHit)
            {
                dashEnded = true;
                isDashing = false;
            }
        }

        /// <summary>
        /// Begin the throwing of a projectile
        /// </summary>
        public void BeginThrow(Player player)
        {
            // Store player position
            playerPosition.X = player.X + player.Position.Width / 2;
            playerPosition.Y = player.Y + player.Position.Height / 2;

            // Update center position
            centerPosition.X = X + position.Width / 2;
            centerPosition.Y = Y + position.Width / 2;

            // Update throwing variables
            throwEnded = false;
            isThrowing = true;
            hasCollided = false;
            throwAttackDuration = 1500;
        }

        /// <summary>
        /// Continue the throwing of a projectile
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="playerHealthBar">The player's healthbar</param>
        /// <param name="elapsedTime">The time since last frame</param>
        public void ContinueThrow(Player player, StatusBar playerHealthBar, double elapsedTime)
        {
            // Check if the rock collides
            if (rock.Collides(player))
            {
                player.DealDamage(rock.Damage);
                hasCollided = true;
            }

            // Tick the attack duration
            throwAttackDuration -= elapsedTime;

            // If the duration runs out, or the rock has collided, end the throw
            if (throwAttackDuration <= 0 || hasCollided)
            {
                isThrowing = false;
                throwEnded = true;
            }
        }

        /// <summary>
        /// Begin the throw process for the projectile
        /// </summary>
        public void BeginThrow()
        {
            // Set variables
            isThrowing = true;
            hasCollided = false;

            // Set attack duration
            throwAttackDuration = 2000;
        }

        /// <summary>
        /// Allows the boss to quickly move backward after
        /// an attack
        /// </summary>
        public void StepBack(int distance)
        {
            if (!hasDashed)
            {
                xDifference = distance;

                // Adjust the horizontal position of the Statue to
                // jump back after an attack
                if (xDifference <= 0)
                {
                    Velocity = dashRight;
                }
                else
                {
                    Velocity = dashLeft;
                }

                ApplyGravity(statueGravity);
            }
        }

        /// <summary>
        /// Sets the collision side of the boss depending on which
        /// tile the boss is colliding with
        /// </summary>
        /// <param name="tiles">The set of environmental tiles</param>
        public void CollidesWithEnvironment(List<Rectangle> tiles)
        {
            // Loop through each potential collidable object
            for (int i = 0; i < tiles.Count; i++)
            {
                // Check for Boss collisions with each of the objects
                if (Position.Intersects(tiles[i]))
                {
                    // Get the overlapping rectangle between their collision
                    Rectangle collisionBox = Rectangle.Intersect(Position, tiles[i]);

                    // If the width is less than or equal to the height,
                    // then the Boss is colliding through the side
                    if (collisionBox.Width <= collisionBox.Height)
                    {
                        // If the difference between the x values of the colliding boxes
                        // is greater than 0, that means the platform is to the right of the 
                        // Boss - so set the collision side to the left
                        if (tiles[i].X - position.X > 0)
                        {
                            bossCollisionSide = CollisionSide.Right;
                        }
                        else if (tiles[i].X - position.X < 0)
                        {
                            // Otherwise, the Boss is to the right of the obstacle
                            // so set the collision side to the right
                            bossCollisionSide = CollisionSide.Left;
                        }
                    }
                }
            }
        }
    }
}