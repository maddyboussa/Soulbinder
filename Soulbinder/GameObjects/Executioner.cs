using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soulbinder
{
    // Executioner attack enum
    public enum ExecutionerState
    {
        None,
        HorizontalSlashHigh,
        HorizontalSlashMid,
        HorizontalSlashLow,
        VerticalSlash,
        RockSlam
    }

    // Ben Sultzer
    // Class: Executioner
    // Purpose: Defines the first boss of the
    // game that follows the player around, attempting
    // to hit them with a vertical or multi-height
    // horizontal slash, or a ground slam that sends
    // rocks falling from the ceiling
    // Restrictions: None
    class Executioner : Boss
    {
        // Fields
        private Random random;
        private ExecutionerState attackState;
        private ExecutionerState prevAttackState;
        private List<Texture2D> attackAnimations;
        private List<Projectile> spawnPoints;
        private Texture2D rockTexture;
        private List<Rectangle> hitBoxes;
        private CollisionSide bossCollisionSide;

        List<Projectile> rocks;

        private BossStates executionerState;
        private PathfindingStates executionerPathfind;
        private double executionerStateCounter;
        private int attackChancesExecutioner;
        private double jumpCounterExecutioner;
        private double chanceCounterExecutioner;

        // Properties
        public ExecutionerState AttackState
        {
            get { return attackState; }
            set { attackState = value; }
        }

        public ExecutionerState PrevAttackState
        {
            get { return prevAttackState; }
            set { prevAttackState = value; }
        }

        public bool IsActive { get; set; }

        /// <summary>
        /// Property for getting and setting which
        /// side of the Boss potentially collides with
        /// the environment
        /// </summary>
        public CollisionSide BossCollisionSide
        {
            get
            {
                return bossCollisionSide;
            }

            set
            {
                bossCollisionSide = value;
            }
        }

        /// <summary>
        /// Constructor for initializing the Executioner's fields
        /// </summary>
        /// <param name="sprite">The image texture of the
        /// Executioner</param>
        /// <param name="position">The rectangle position of
        /// the Executioner</param>
        /// <param name="speed">The movement speed of the 
        /// Executioner</param>
        /// <param name="healthCur">The current health of
        /// the Executioner</param>
        /// <param name="healthMax">The max health of the
        /// Executioner</param>
        /// <param name="attackAnimations">The list of sprites
        /// for the Executioner</param>
        /// <param name="rockTexture">The texture for the rock
        /// slam attack</param>
        public Executioner(Texture2D sprite, Rectangle position, int speed,
            int healthCur, int healthMax, List<Texture2D> attackAnimations, Texture2D rockTexture)
            : base(sprite, position, speed, healthCur, healthMax)
        {
            Name = "The Executioner";

            random = new Random();
            attackState = ExecutionerState.None;
            prevAttackState = ExecutionerState.None;
            this.attackAnimations = attackAnimations;
            spawnPoints = new List<Projectile>();
            this.rockTexture = rockTexture;
            hitBoxes = new List<Rectangle>();
            IsActive = false;

            rocks = new List<Projectile>();

            executionerStateCounter = 0;
            attackChancesExecutioner = 4;
            jumpCounterExecutioner = 0;

        }

        // Methods
        public override void Update(Game1 game)
        {
            if (!dead)
            {
                // set the player's projectile list to rocks
                game.Player.ProjectileList = rocks;

                // Get distance measurements from the center of the Executioner to the player in the
                // x- and y- directions for attack attempts evaluation
                double distanceFromExecutionerX = game.GameManager.CalcDistanceFromCenterX(game.Player, this);
                double distanceFromExecutionerY = game.GameManager.CalcDistanceFromCenterY(game.Player, this);
                double requiredDistanceExecutionerX = Position.Width;
                double requiredDistanceExecutionerY = Position.Height;

                // Get the collision side of the boss
                CollidesWithEnvironment(game.CurrentLevel.Collisions);

                ResolveCollisions(game.CurrentLevel.Collisions);

                switch (executionerState)
                {
                    case BossStates.Move:
                        // Reset the rocks
                        rocks.Clear();

                        // Gravity will control the Boss's vertical movement
                        ApplyGravity(game.Gravity);

                        // Change the state to idle after 10 seconds
                        if (executionerStateCounter > 1.5)
                        {
                            executionerState = BossStates.Idle;
                            executionerStateCounter = 0;
                        }
                        else
                        {
                            // Add to the jump counter the current elapsed game time
                            // to create a steady pathfinding jump
                            jumpCounterExecutioner += game.ElapsedSeconds;

                            // Add to the executioner state counter the current elapsed
                            // game time to determine when to change states
                            executionerStateCounter += game.ElapsedSeconds;

                            // Determine whether the Boss needs to find a route to the
                            // player                                
                            switch (executionerPathfind)
                            {
                                case PathfindingStates.Move:
                                    // Check if the Boss has collided with something 
                                    // from the right or left
                                    if (BossCollisionSide == CollisionSide.Right ||
                                        BossCollisionSide == CollisionSide.Left)
                                    {
                                        // Transition the state to check the Boss's location
                                        // with the player's
                                        executionerPathfind = PathfindingStates.CheckVertical;
                                    }

                                    // Move the Boss at the pathfinding speed
                                    X += speed;
                                    break;
                                case PathfindingStates.CheckVertical:
                                    // Check the Boss's vertical position against the player's
                                    // and jump if needed
                                    if (Y > game.Player.Y && jumpCounterExecutioner > 1)
                                    {
                                        Jump();
                                        jumpCounterExecutioner = 0;
                                    }

                                    // Transition the state to check the Boss's horizontal
                                    // position against the player's
                                    executionerPathfind = PathfindingStates.CheckHorizontal;
                                    break;
                                case PathfindingStates.CheckHorizontal:
                                    // Check the Boss's horizontal position against the player's
                                    // and chnage direction if needed
                                    if (X > game.Player.X)
                                    {
                                        speed = -3;
                                    }
                                    else
                                    {
                                        speed = 3;
                                    }

                                    // Transition the state back to continue moving
                                    executionerPathfind = PathfindingStates.Move;
                                    break;
                            }
                        }
                        break;

                    case BossStates.Idle:
                        // Change the state to Attack after 1 second
                        if (executionerStateCounter > 1)
                        {
                            // Increase the speed of the Boss during attack
                            speed = 6;
                            executionerState = BossStates.Attack;
                            executionerStateCounter = 0;
                        }
                        else
                        {
                            // Prevent the Boss from freezing in mid-air
                            // and continue to count the time until
                            // the state needs to be changed
                            ApplyGravity(game.Gravity);
                            executionerStateCounter += game.ElapsedSeconds;
                        }
                        break;

                    case BossStates.Attack:
                        // Gravity will control the Boss's vertical movement
                        ApplyGravity(game.Gravity);

                        for (int i = 0; i < rocks.Count; i++)
                        {
                            rocks[i].ApplyGravity(game.Gravity / 2);
                            //rocks[i].ResolveCollisions(tiles);

                            if (rocks[i].Collides(game.Player) && game.Player.SpellState != SpellState.Deflect)
                            {
                                if (!game.Player.IsInvincible)
                                {
                                    game.Player.DealDamage(rocks[i].Damage);
                                    game.Player.StartInvincibility();
                                    rocks.RemoveAt(i--);
                                }
                            }
                        }

                        if (attackChancesExecutioner > 0)
                        {
                            chanceCounterExecutioner += game.ElapsedSeconds;
                            if (chanceCounterExecutioner > 1)
                            {
                                // Choose attacks based on range
                                if (distanceFromExecutionerX <= requiredDistanceExecutionerX &&
                                    distanceFromExecutionerY <= requiredDistanceExecutionerY)
                                {
                                    // If the player is within melee, do a melee attack
                                    Random rng = new Random();
                                    int attackChosenClose = rng.Next(1, 5);
                                    Attack(
                                        game.Player, 
                                        game.Gravity, 
                                        rocks, 
                                        attackChosenClose, 
                                        game.Camera);
                                }
                                else
                                {
                                    // Otherwise, do a ranged attack by making rocks fall
                                    Attack(
                                        game.Player,
                                        game.Gravity,
                                        rocks,
                                        5,
                                        game.Camera);
                                }
                                attackChancesExecutioner--;
                                chanceCounterExecutioner = 0;
                            }
                        }

                        // Change back to the Move state after 0.5 seconds
                        if (executionerStateCounter > 0.5 && attackChancesExecutioner <= 0)
                        {
                            // Reset the Boss's speed, attack chance counter, attack state, 
                            // and boss state counter variables
                            speed = 3;
                            executionerState = BossStates.Move;
                            executionerStateCounter = 0;
                            attackChancesExecutioner = 4;
                            AttackState = ExecutionerState.None;
                            PrevAttackState = ExecutionerState.None;
                        }
                        else
                        {
                            // Continue to move the Boss and count the time
                            // until the state needs to be changed
                            executionerStateCounter += game.ElapsedSeconds;

                            // COMMENTED THIS OUT FOR NOW BECAUSE I WANTED TO HAVE THE BOSS NOT MOVE - Abby
                            //executioner.Move(player.X, player.Y);
                        }
                        break;
                }
            }

            // Reset the collision side of the Boss
            BossCollisionSide = CollisionSide.None;
        }

        public override void Draw(Game1 game)
        {
            if (!dead)
            {
                // Draw the Boss
                DrawState(executionerState, game.SpriteBatch, game.SpriteManager.Pixel, game.Camera, game);

                // Draw the rocks
                foreach (Projectile p in rocks)
                {
                    game.SpriteBatch.Draw(
                        game.SpriteManager.RockSprite, 
                        new Rectangle(
                            p.Position.X - game.Camera, 
                            p.Position.Y, 
                            p.Position.Width, 
                            p.Position.Height), 
                        Color.White);
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
            attackState = ExecutionerState.None;
            prevAttackState = ExecutionerState.None;
            spawnPoints.Clear();
        }

        /// <summary>
        /// Draw the Executioner depending on it's state
        /// </summary>
        /// <param name="currentState">The current state it is in</param>
        /// <param name="sb">The SpriteBatch to draw from</param>
        /// <param name="hitboxTexture">The texture of the hitboxes</param>
        /// <param name="camX">The current camera position</param>
        public override void DrawState(BossStates currentState, SpriteBatch sb, Texture2D hitboxTexture, int camX, Game1 game)
        {

            if (currentState == BossStates.Move ||
                currentState == BossStates.Idle)
            {
                sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
            }
            else
            {
                Rectangle drawRect;
                switch (attackState)
                {
                    case ExecutionerState.None:
                        sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height), Color.White);
                        break;

                    case ExecutionerState.HorizontalSlashHigh:
                        drawRect = attackAnimations[0].Bounds;

                        sb.Draw(attackAnimations[0], new Rectangle(position.X - camX - 104, position.Y - 1, drawRect.Width / 2, drawRect.Height / 2), Color.White);
                        sb.Draw(hitboxTexture, new Rectangle(hitBoxes[0].X - camX, hitBoxes[0].Y, hitBoxes[0].Width, hitBoxes[0].Height), Color.Transparent);
                        break;

                    case ExecutionerState.HorizontalSlashMid:
                        drawRect = attackAnimations[1].Bounds;

                        sb.Draw(attackAnimations[1], new Rectangle(position.X - camX - 104, position.Y - 1, drawRect.Width / 2, drawRect.Height / 2), Color.White);
                        sb.Draw(hitboxTexture, new Rectangle(hitBoxes[1].X - camX, hitBoxes[1].Y, hitBoxes[1].Width, hitBoxes[1].Height), Color.Transparent);
                        break;

                    case ExecutionerState.HorizontalSlashLow:
                        drawRect = attackAnimations[2].Bounds;

                        sb.Draw(attackAnimations[2], new Rectangle(position.X - camX - 104, position.Y - 1, drawRect.Width / 2, drawRect.Height / 2), Color.White);
                        sb.Draw(hitboxTexture, new Rectangle(hitBoxes[2].X - camX, hitBoxes[2].Y, hitBoxes[2].Width, hitBoxes[2].Height), Color.Transparent);
                        break;

                    case ExecutionerState.VerticalSlash:
                        drawRect = attackAnimations[3].Bounds;
                        SpriteEffects flip = SpriteEffects.None;
                        if (game.Player.Position.X > this.X)
                        {
                            flip = SpriteEffects.FlipHorizontally;
                        }

                        sb.Draw(attackAnimations[3], new Rectangle(position.X - camX - 98, position.Y - 58, drawRect.Width / 2, drawRect.Height / 2), 
                            null, Color.White, 0.0f, Vector2.Zero, flip, 0.0f);
                        sb.Draw(hitboxTexture, new Rectangle(hitBoxes[3].X - camX, hitBoxes[3].Y, hitBoxes[3].Width, hitBoxes[3].Height), Color.Transparent);
                        break;
                    case ExecutionerState.RockSlam:
                        drawRect = attackAnimations[4].Bounds;

                        sb.Draw(attackAnimations[4], new Rectangle(position.X - camX, position.Y - 58, drawRect.Width / 2, drawRect.Height / 2), Color.White);
                        break;
                }
            }
        }

        /// <summary>
        /// Determine from the attack set a random attack to throw at the 
        /// player
        /// </summary>
        public void Attack(Player player, Vector2 gravity, List<Projectile> rocks, int attackChosen, int camX)
        {
            // Clear the hitboxes
            hitBoxes.Clear();

            // Determine which attack will be used
            switch (attackChosen)
            {
                case 0:
                    attackState = ExecutionerState.None;
                    break;

                case 1:
                    attackState = ExecutionerState.HorizontalSlashHigh;
                    break;

                case 2:
                    attackState = ExecutionerState.HorizontalSlashMid;
                    break;

                case 3:
                    attackState = ExecutionerState.HorizontalSlashLow;
                    break;

                case 4:
                    attackState = ExecutionerState.VerticalSlash;
                    break;

                case 5:
                    attackState = ExecutionerState.RockSlam;
                    break;
            }

            // Perform the attack

            // Draw hitboxes
            Rectangle horizontalHigh = new Rectangle(Position.X - 104, Position.Y, 400, 110);
            Rectangle horizontalMid = new Rectangle(Position.X - 104, Position.Y + 45, 400, 110);
            Rectangle horizontalLow = new Rectangle(Position.X - 104, (Position.Y + Position.Height) - 100, 400, 110);
            Rectangle vertical = new Rectangle(Position.X - 98, Position.Y - 60, 290, Position.Height + 60);

            hitBoxes.Add(horizontalHigh);
            hitBoxes.Add(horizontalMid);
            hitBoxes.Add(horizontalLow);
            hitBoxes.Add(vertical);

            int verticalDamage = 4;
            int horizontalDamage = 3;

            switch (attackState)
            {

                case ExecutionerState.None:
                    // Do nothing
                    break;

                case ExecutionerState.HorizontalSlashHigh:

                    if (player.Collides(hitBoxes[0]))
                    {
                        player.DealDamage(horizontalDamage);
                        player.StartInvincibility();
                    }
                    
                    break;

                case ExecutionerState.HorizontalSlashMid:
                    if (player.Collides(hitBoxes[1]))
                    {
                        player.DealDamage(horizontalDamage);
                        player.StartInvincibility();
                    }
                    break;

                case ExecutionerState.HorizontalSlashLow:
                    if (player.Collides(hitBoxes[2]))
                    {
                        player.DealDamage(horizontalDamage);
                        player.StartInvincibility();
                    }
                    break;

                case ExecutionerState.VerticalSlash:
                    if (player.Collides(hitBoxes[3]))
                    {
                        player.DealDamage(verticalDamage);
                        player.StartInvincibility();
                    }
                    break;

                case ExecutionerState.RockSlam:
                    if (prevAttackState != ExecutionerState.RockSlam)
                    {
                        // Create rocks around the player
                        int numberOfSpawns = random.Next(3, 6);
                        for (int i = 0; i < numberOfSpawns; i++)
                        {
                            Point spawn = new Point(random.Next(player.X - 200, player.X + player.Position.Width + 200), 0);
                            rocks.Add(new Projectile(rockTexture, new Rectangle(spawn.X, spawn.Y, 50, 50), 5, 10, 10, 5));
                        }
                    }
                    break;
            }

            prevAttackState = attackState;
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


        public override void DrawShapes(Game1 game)
        {
            Rectangle hitbox = new Rectangle();

            // Draw the rectangles based on the attack state
            switch (attackState)
            {

                case ExecutionerState.None:
                    return;


                case ExecutionerState.HorizontalSlashHigh:
                    hitbox = hitBoxes[0];
                    break;

                case ExecutionerState.HorizontalSlashMid:
                    hitbox = hitBoxes[1];
                    break;

                case ExecutionerState.HorizontalSlashLow:
                    hitbox = hitBoxes[2];
                    break;

                case ExecutionerState.VerticalSlash:
                    hitbox = hitBoxes[3];
                    break;
            }

            ShapeBatch.BoxOutline(hitbox.X - game.Camera, hitbox.Y, hitbox.Width, hitbox.Height, Color.Red);
        }
    }
}
