using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Soulbinder
{
    public enum PlayerState
    {
        StandRight,
        StandLeft,
        MoveRight,
        MoveLeft
    }

    /// <summary>
    /// represents the potential states of spells the player could cast
    /// </summary>
    public enum SpellState
    {
        None,
        BasicAttack,
        Deflect,
        FireDash,
        Projectile
    }

    public class Player : Entity
    {
        // Fields
        private bool godMode;
        private bool locked;
        private bool jump;
        private bool ableToDeflect;
        private bool ableToDash;
        private bool ableToProjectile;
        private int currentMana;
        private int maximumMana;
        private PlayerState state;
        private PlayerState prevState;
        private SpriteManager sm;

        private bool dead;
        private int deathCounter;

        // stores the states of spells
        private SpellState spellState;
        private SpellState prevSpellState;

        private double currentManaTime;

        // stores the spell objects
        private BasicAttack basicAttack;
        private Texture2D basicAttackSprite;

        private FireDash fireDash;
        private Texture2D fireDashSprite;

        private Deflect deflect;
        private Texture2D deflectSprite;

        private ProjectileSpell projectileSpell;
        private Texture2D projectileSpellSprite;

        private Vector2 spellVector;

        // list of entities and tiles
        private List<Skeleton> enemyList;
        private List<Rectangle> tileList;
        private List<Projectile> projectileList;

        // Vectors used for testing/UI
        private Vector2 center;
        private Vector2 mousePosition;

        // player invincibility related fields
        private double invincibilityCountdown;

        // used to implement a cooldown between casts of basic attack
        private double cooldownTime;


        // Properties
        public bool GodMode { get { return godMode; } set { godMode = value; } }
        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        public bool Jump
        {
            get { return jump; }
            set { jump = value; }
        }
        public int CurrentMana
        {
            get { return currentMana; }
            set { currentMana = value; }
        }

        public int MaximumMana
        {
            get { return maximumMana; }
            set { maximumMana = value; }
        }

        public PlayerState State
        {
            get { return state; }
            set { state = value; }
        }

        public PlayerState PrevState
        {
            get { return prevState; }
            set { prevState = value; }
        }

        
        public bool IsInvincible 
        { 
            get { return invincibilityCountdown > 0; }
        }

        // Properties involving player spells
        public SpellState SpellState
        {
            get { return spellState; }
            set { spellState = value; }
        }

        public SpellState PrevSpellState
        {
            get { return prevSpellState; }
            set { prevSpellState = value; }
        }

        public BasicAttack BasicAttack { get { return basicAttack; } set { basicAttack = value; } }

        public FireDash FireDash { get { return fireDash; } set { fireDash = value; } }

        public Deflect Deflect { get { return deflect; } set { deflect = value; } }

        public ProjectileSpell ProjectileSpell { get { return projectileSpell; } set { projectileSpell = value; } }
        

        // Properties involving player position
        public Rectangle PrevPosition { get; set; }

        /// <summary>
        /// Represents whether the player is falling
        /// </summary>
        public bool IsFalling
        {
            // If there's a bigger difference than two between the current and previous position
            // then it is falling (it might be off by 1 just from collision correction)
            get { return Position.Y - PrevPosition.Y > 3; }
        }


        public List<Projectile> ProjectileList
        {
            get { return projectileList; }
            set { projectileList = value; }
        }

        public bool AbleToDeflect { get => ableToDeflect; set => ableToDeflect = value; }
        public bool AbleToDash { get => ableToDash; set => ableToDash = value; }
        public bool AbleToProjectile { get => ableToProjectile; set => ableToProjectile = value; }
        public bool Dead { get => dead; set => dead = value; }
        public int DeathCounter { get => deathCounter; set => deathCounter = value; }

        // Constructor
        public Player(Texture2D sprite, Rectangle position, int speed, int healthCur, int healthMax, int currentMana, int maximumMana, 
            Texture2D basicAttackSprite, Texture2D fireDashSprite, Texture2D deflectSprite, Texture2D projectileSpellSprite, SpriteManager sm) :
            base(sprite, position, speed, healthCur, healthMax)
        {
            godMode = false;
            locked = false;
            this.currentMana = currentMana;
            this.maximumMana = maximumMana;
            state = PlayerState.StandRight;
            prevState = state;
            spellState = SpellState.None;
            prevSpellState = spellState;
            this.sm = sm;
            jump = false;

            dead = false;
            deathCounter = 0;

            currentManaTime = 0;

            this.enemyList = new List<Skeleton>(); ;
            this.tileList = new List<Rectangle>(); ;

            this.basicAttackSprite = basicAttackSprite;
            basicAttack = new BasicAttack(basicAttackSprite, position.X, position.Y, 96, 64, this);

            this.fireDashSprite = fireDashSprite;
            fireDash = new FireDash(fireDashSprite, position.X, position.Y, 96, 64, this);

            this.deflectSprite = deflectSprite;
            deflect = new Deflect(deflectSprite, position.X, position.Y, 128, 128, this, sm);

            this.projectileSpellSprite = projectileSpellSprite;
            projectileSpell = new ProjectileSpell(projectileSpellSprite, position.X, position.Y, 96, 64, this);

            // Make the player able to use moves or not
            AbleToDeflect = true;
            AbleToDash = true;
            AbleToProjectile = true;

            center = new Vector2();
            mousePosition = new Vector2();

            spellVector = new Vector2(0, 0);

            cooldownTime = 0;
        }

        // Methods
        public void Update(Game1 game)
        {
            game.Camera = position.X - (game.GraphicsManager.PreferredBackBufferWidth / 2);

            this.enemyList = game.CurrentLevel.Enemies;

            // PRESS G TO ENTER GOD MODE
            if (game.KeyboardState.IsKeyDown(Keys.G) && game.PreviousKeyboardState.IsKeyUp(Keys.G))
            {
                if (!GodMode)
                {
                    GodMode = true;
                }
                else GodMode = false;
            }

            // Movement updates
            if (!Locked)
            {
                ApplyGravity(game.Gravity);
                Move(game.KeyboardState, 
                    game.PreviousKeyboardState, 
                    Keys.Space, Keys.A, Keys.D, 
                    game.ElapsedMilliseconds);

                ResolveCollisions(game.CurrentLevel.Collisions);

                //KillOutOfBounds(game.HealthBar, game.GraphicsManager.PreferredBackBufferHeight);

                // update spell stuff
                CastSpell(
                    game.KeyboardState, 
                    game.PreviousKeyboardState, 
                    game.MouseState, 
                    game.PreviousMouseState, 
                    game.ElapsedMilliseconds, 
                    (int) game.Camera,
                    game);

                // Update player's previous position
                PrevPosition = Position;
            }

            // regain 1 point of mana every 3 seconds
            currentManaTime += game.ElapsedMilliseconds;

            if (currentManaTime > 3000)
            {
                if (currentMana < maximumMana)
                {
                    currentMana += 1;
                }
                currentManaTime = 0;
            }

            // StatusBar updates
            //UseMana(
            //    game.KeyboardState, 
            //    game.PreviousKeyboardState, 
            //    game.ManaBar, 
             //   1);

            if(godMode)
            {
                jump = false;
                currentMana = maximumMana;
                CurrentHealth = MaximumHealth;
            }

            if(CurrentHealth <= 0 || Position.Y > game.GraphicsManager.PreferredBackBufferHeight + 200)
            {
                dead = true;
                deathCounter++;
            }
        }

        public void Draw(Game1 game)
        {
            switch (state)
            {
                case PlayerState.StandRight:
                    Draw(game.SpriteBatch, SpriteEffects.None, game.Camera);
                    break;
                case PlayerState.StandLeft:
                    Draw(game.SpriteBatch, SpriteEffects.FlipHorizontally, game.Camera);
                    break;
                case PlayerState.MoveRight:
                    Draw(game.SpriteBatch, SpriteEffects.None, game.Camera);
                    break;
                case PlayerState.MoveLeft:
                    Draw(game.SpriteBatch, SpriteEffects.FlipHorizontally, game.Camera);
                    break;
            }

            DrawSpell(game);

            if(godMode)
            {
                DrawPlayerShapes(game.Camera);
            }

            // Drawing Reticle/Cursor
            ShapeBatch.Line(
                new Vector2(center.X - game.Camera, center.Y), 
                new Vector2(mousePosition.X - game.Camera, mousePosition.Y), 3, Color.White);

            game.SpriteBatch.Draw(
                game.SpriteManager.Pixel,
                new Rectangle(
                    game.MouseState.X - 5,
                    game.MouseState.Y - 5,
                    10, 10),
                Color.White);
        }

        /// <summary>
        /// Reset the player
        /// </summary>
        public override void Reset()
        {
            // Call base reset
            base.Reset();

            // Reset other values
            dead = false;
            locked = false;
            state = PlayerState.StandRight;
            prevState = state;
            spellState = SpellState.None;
            prevSpellState = spellState;
            jump = false;
            currentMana = maximumMana;
            invincibilityCountdown = 0;
            MaximumHealth = 20;
            CurrentHealth = 20;
        }

        public override void ApplyGravity(Vector2 gravity)
        {
            if (spellState != SpellState.FireDash)
            {
                Velocity += gravity;
                Y += (int)Velocity.Y;
            }
        }

        /// <summary>
        /// See if the Player can move in an inputted direction, if so, move the Player in that direction
        /// </summary>
        /// <param name="kb">The KeyboardState to read Player input from</param>
        /// <param name="tiles">The list of tiles to check for collisions</param>
        /// <returns>True if the Player can move in the inputted direction, false if not</returns>
        //public bool Move(KeyboardState kb, List<GameObject> tiles)
        public bool Move(KeyboardState kb, KeyboardState prevKB, Keys jumpKey, 
            Keys moveLeft, Keys moveRight, double ellapsedMilliseconds)
        {
            // update invincibility
            if (IsInvincible)
            {
                invincibilityCountdown -= ellapsedMilliseconds;
            }
            
            
            bool canMove = true;
            Rectangle comparison = position;

            // Check if the player landed
            if(collisionSide == CollisionSide.Down)
            {
                // If so, let the player be able to jump again
                jump = false;
            }

            switch (state)
            {
                case PlayerState.StandRight:
                    DoJump(kb, prevKB, jumpKey);
                    if (kb.IsKeyDown(moveRight) && kb.IsKeyUp(moveLeft))
                    {
                        state = PlayerState.MoveRight;
                    }

                    if (kb.IsKeyDown(moveLeft) && kb.IsKeyUp(moveRight))
                    {
                        state = PlayerState.MoveLeft;
                    }
                    break;

                case PlayerState.StandLeft:
                    DoJump(kb, prevKB, jumpKey);

                    if (kb.IsKeyDown(moveRight) && kb.IsKeyUp(moveLeft))
                    {
                        state = PlayerState.MoveRight;
                    }

                    if (kb.IsKeyDown(moveLeft) && kb.IsKeyUp(moveRight))
                    {
                        state = PlayerState.MoveLeft;
                    }
                    break;

                case PlayerState.MoveRight:
                    comparison.X += speed;
                    //foreach(GameObject o in tiles)
                    //{
                    //    if (comparison.Intersects(o.Position))
                    //    {
                    //        canMove = false;
                    //    }
                    //}
                    if (canMove)
                    {
                        position.X += speed;
                    }

                    DoJump(kb, prevKB, jumpKey);

                    if (kb.IsKeyDown(moveLeft) && kb.IsKeyUp(moveRight))
                    {
                        state = PlayerState.MoveLeft;
                    }

                    if (prevKB.IsKeyUp(moveRight))
                    {
                        state = PlayerState.StandRight;
                    }
                    break;

                case PlayerState.MoveLeft:
                    comparison.X -= speed;
                    //foreach (GameObject o in tiles)
                    //{
                    //    if (comparison.Intersects(o.Position))
                    //    {
                    //        canMove = false;
                    //    }
                    //}
                    if (canMove)
                    {
                        position.X -= speed;
                    }

                    DoJump(kb, prevKB, jumpKey);

                    if (kb.IsKeyDown(moveRight) && kb.IsKeyUp(moveLeft))
                    {
                        state = PlayerState.MoveRight;
                    }


                    if (prevKB.IsKeyUp(moveLeft))
                    {
                        state = PlayerState.StandLeft;
                    }
                    break;
            }

            prevState = state;

            return canMove;
        }

        /// <summary>
        /// runs when the player is within the casting state,
        /// determines which spell to cast based on the key that was pressed
        /// </summary>
        /// <param name="kb"></param>
        /// <param name="prevKb"></param>
        public void CastSpell(KeyboardState kb, KeyboardState prevKb, MouseState ms, MouseState prevMs, double elapsedMilliseconds, int camX, Game1 game)
        {
            // decrement cooldown time
            cooldownTime -= elapsedMilliseconds;

            // Store the player's center for ui purposes
            center.X = Position.X + Position.Width / 2;
            center.Y = Position.Y + Position.Height / 2;

            // Store the mouse's position 
            mousePosition.X = ms.X + camX;
            mousePosition.Y = ms.Y;


            // Check for spell input and change state acordingly

            if ((ms.LeftButton == ButtonState.Pressed && prevMs.LeftButton == ButtonState.Released) 
                && spellState == SpellState.None)
            {
                // check if you have mana before allowing the player to cast
                if (currentMana < basicAttack.ManaCost)
                {
                    spellState = SpellState.None;
                }
                else
                {
                    spellState = SpellState.BasicAttack;
                }
            }

            else if ((ms.RightButton == ButtonState.Pressed && prevMs.RightButton == ButtonState.Released) 
                && spellState == SpellState.None && AbleToDeflect)
            {
                // check if you have mana before allowing the player to cast
                if (currentMana < deflect.ManaCost)
                {
                    spellState = SpellState.None;
                }
                else
                {
                    spellState = SpellState.Deflect;
                }
            }

            else if ((kb.IsKeyDown(Keys.LeftShift) && prevKb.IsKeyUp(Keys.LeftShift))  
                && spellState == SpellState.None && AbleToDash)
            {
                // check if you have mana before allowing the player to cast
                if (currentMana < fireDash.ManaCost)
                {
                    spellState = SpellState.None;
                }
                else
                {
                    spellState = SpellState.FireDash;
                }
            }

            else if ((kb.IsKeyDown(Keys.Q) && prevKb.IsKeyUp(Keys.Q))  && spellState == SpellState.None)
            {
                // check if you have mana before allowing the player to cast
                if (currentMana <= 0)
                {
                    spellState = SpellState.None;
                }
                else
                {
                    spellState = SpellState.Projectile;
                }
            }
            

            // check if the current state is fireDash or projectile
            // if so, we don't want to update the spell vector (for dash movement)
            if (spellState != SpellState.FireDash && spellState != SpellState.Projectile)
            {
                // update the spell vector (except for during fireDash)
                double magnitude = Math.Sqrt(spellVector.X * spellVector.X + spellVector.Y * spellVector.Y);
                spellVector.X /= (float)magnitude;
                spellVector.Y /= (float)magnitude;
            }


            // depending on the state, call the appropriate spell method
            switch (SpellState)
            {
                case SpellState.BasicAttack:

                    // update this spells location based on player location
                    basicAttack.X = position.X + position.Width / 2;
                    basicAttack.Y = position.Y + position.Height / 2;

                    // begin the spell if the spell is not already in progress
                    if (!basicAttack.IsCasting)
                    {
                        // update direction vector for attack
                        spellVector = mousePosition - center;

                        // normalize the dash vector so its length doesn't matter
                        float magnitude = (float)Math.Sqrt(spellVector.X * spellVector.X + spellVector.Y * spellVector.Y);
                        spellVector.X /= magnitude;
                        spellVector.Y /= magnitude;

                        if (cooldownTime <= 0)
                        {
                            basicAttack.BeginCast(spellVector, center);

                            // reset cooldown time here
                            cooldownTime = 600;
                        }
                        else
                        {
                            spellState = SpellState.None;
                        }
                    }
                    // else continue the spell if it is in progress
                    else if (basicAttack.IsCasting)
                    {
                        basicAttack.ContinueCast(enemyList, tileList, elapsedMilliseconds, center, game);
                    }
                    // else stop cast if the duration has been used up
                    else if (!basicAttack.IsCasting)
                    {
                        spellState = SpellState.None;
                    }

                    break;

                case SpellState.Deflect:

                    // update this spells location based on player location
                    deflect.X = position.X + position.Width / 2;
                    deflect.Y = position.Y + position.Height / 2;

                    // begin the spell if the spell is not already in progress
                    if (!deflect.IsCasting && ((ms.RightButton == ButtonState.Pressed 
                        && prevMs.RightButton == ButtonState.Released)) && (deflect.Unlocked || godMode))
                    {
                        // update direction vector for deflect
                        spellVector = mousePosition - center;
                        deflect.BeginCast(spellVector, center);
                    }
                    // else continue the spell if it is in progress
                    else if (deflect.IsCasting)
                    {
                        deflect.ContinueCast(enemyList, tileList, elapsedMilliseconds, spellVector, game);
                    }
                    // else stop cast if the duration has been used up
                    else if (!deflect.IsCasting)
                    {
                        spellState = SpellState.None;
                    }
                    break;

                case SpellState.FireDash:

                    // update the spell's location based on player location
                    fireDash.X = position.X + position.Width / 2;
                    fireDash.Y = position.Y + position.Height / 2;

                    // being the spell if it is not already in progress
                    if (!fireDash.IsCasting && ((kb.IsKeyDown(Keys.LeftShift) && prevKb.IsKeyUp(Keys.LeftShift))) && (fireDash.Unlocked || godMode))
                    {
                        fireDash.BeginCast(spellVector, center);

                        // update direction vector for dash
                        spellVector = mousePosition - center;

                        // normalize the dash vector so its length doesn't matter
                        double magnitude = Math.Sqrt(spellVector.X * spellVector.X + spellVector.Y * spellVector.Y);
                        spellVector.X /= (float)magnitude;
                        spellVector.Y /= (float)magnitude;

                        // Make the player's regular movement vector in the Y directon 0 so they do not keep the 
                        // speed they had before/after the dash
                        Velocity = Vector2.Zero;
                    }
                    // else continue the spell if it is in progress
                    else if (fireDash.IsCasting)
                    {
                        // continue cast updates the spell duration and invincibility duration
                        fireDash.ContinueCast(enemyList, tileList, elapsedMilliseconds, game);

                        // update player location based on dash vector
                        position.X += (int) (20 * spellVector.X);
                        position.Y += (int) (20 * spellVector.Y);
                    }
                    // else stop cast if the duration has been used up
                    else if (!fireDash.IsCasting)
                    {
                        spellState = SpellState.None;
                    }
                    break;

                case SpellState.Projectile:

                    // being the spell if it is not already in progress
                    if (!projectileSpell.IsCasting && ((kb.IsKeyDown(Keys.Q) && prevKb.IsKeyUp(Keys.Q))) && (projectileSpell.Unlocked || godMode))
                    {
                        // set the spell's location based on player location at initial cast
                        projectileSpell.X = position.X + position.Width / 2;
                        projectileSpell.Y = position.Y + position.Height / 2;
                        
                        // begin the spell
                        projectileSpell.BeginCast(spellVector, center);

                        // update direction vector for spell
                        spellVector = mousePosition - center;

                        // normalize the spell vector so its length doesn't matter
                        double magnitude = Math.Sqrt(spellVector.X * spellVector.X + spellVector.Y * spellVector.Y);
                        spellVector.X /= (float)magnitude;
                        spellVector.Y /= (float)magnitude;
                    }
                    // else continue the spell if it is in progress
                    else if (projectileSpell.IsCasting)
                    {
                        // continue cast updates the spell duration and collisions
                        projectileSpell.ContinueCast(enemyList, tileList, elapsedMilliseconds, spellVector, game);

                        // update the spell's position based on the spell vector
                        projectileSpell.X += (int)(17 * spellVector.X);
                        projectileSpell.Y += (int)(17 * spellVector.Y);
                    }
                    // else stop cast if the duration has been used up
                    else if (!projectileSpell.IsCasting)
                    {
                        spellState = SpellState.None;
                    }
                    break;

                // represents the none state for spellState
                default:
                    // essentially do nothing
                    break;
            }

            // Deal with any deflected projectiles
            if (deflect.ReflectedProjectile != null)
            {
                // Do a bunch of physics on it
                deflect.ReflectedProjectile.X += (int) deflect.ReflectedProjectile.DeflectSpeed.X;
                deflect.ReflectedProjectile.Y += (int)deflect.ReflectedProjectile.DeflectSpeed.Y;

                // check reflected projectile collisions
                // also have the projectile disappear if the projectile is far enough away from the player
                if ((deflect.ReflectedProjectile.X >= position.X + 2000) || (deflect.ReflectedProjectile.Y >= position.Y + 2000))
                {
                    deflect.ReflectedProjectile = null;
                }

                // make projectile disappear if it collides with an enemy or the bounds of the screen
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (enemyList[i].Collides(deflect.ReflectedProjectile))
                    {
                        enemyList[i].CurrentHealth -= deflect.ReflectedProjectile.Damage;
                        deflect.ReflectedProjectile = null;
                        break;
                    }
                }

                // make projectile disappear if it collides with a the level's boss
                if (game.CurrentLevel.Boss != null)
                {
                    for (int i = game.CurrentLevel.Boss.Count - 1; i >= 0; i--)
                    {
                        if (game.CurrentLevel.Boss[i].Collides(deflect.ReflectedProjectile))
                        {
                            game.CurrentLevel.Boss[i].CurrentHealth -= deflect.ReflectedProjectile.Damage;
                            deflect.ReflectedProjectile = null;
                        }
                    }
                }

            }
        }

        public void KillOutOfBounds(StatusBar healthBar, int heightOfLevel)
        {
            if(Y > heightOfLevel)
            {
                locked = true;
                DealDamage(MaximumHealth);
            }
        }

        public void DoJump(KeyboardState kb, KeyboardState prevKB, Keys jumpKey)
        {
            if (kb.IsKeyDown(jumpKey) && prevKB.IsKeyUp(jumpKey) && ((!jump && !IsFalling) || (godMode)))
            {
                Velocity = JumpVelocity;
                jump = true;
            }
        }

        public void DealDamage(int amount)
        {
            // If the player is not invincible and not in God Mode
            if (invincibilityCountdown <= 0 && !godMode)
            {
                // Deal the damage
                CurrentHealth -= amount;
            }

        }


        /// <summary>
        /// For general hits, call this method which will start
        /// the player's invincibility countdown with the default value
        /// </summary>
        public void StartInvincibility()
        {
            StartInvincibility(1000);
        }

        /// <summary>
        /// causes the player to not take damage during a set amount of time
        /// by updating the value of the player's isInvinvible field
        /// </summary>
        /// <param name="duration">amount of time to be invincible IN MILLISECONDS</param>
        public void StartInvincibility(double duration)
        {
            // set the player's invincibility to the passed in milliseconds
            invincibilityCountdown = duration;

            // subtraction of the duration would occur outside this method,
            // and the new value would be passed in
        }

        /// <summary>
        /// Use Mana (FOR TESTING STATUS BARS - NOT FINAL)
        /// </summary>
        /// <param name="kb">The keyboard state</param>
        /// <param name="prevKB">The previous keyboard state</param>
        /// <param name="manaBar">The mana bar</param>
        /// <param name="amount">The amount of mana to use</param>
        public void UseMana(KeyboardState kb, KeyboardState prevKB, StatusBar manaBar, int amount)
        {
            // If the left key is pressed, gain mana
            if (kb.IsKeyDown(Keys.Left) && prevKB.IsKeyUp(Keys.Left))
            {
                int subtraction = CurrentMana - amount;

                if (subtraction < 0)
                {
                    CurrentMana = 0;
                }
                else
                {
                    CurrentMana -= amount;
                }
            }

            // If the right key is pressed, gain mana
            if (kb.IsKeyDown(Keys.Right) && prevKB.IsKeyUp(Keys.Right))
            {
                int addition = CurrentMana + amount;

                if (addition > MaximumMana)
                {
                    CurrentMana = MaximumMana;
                }
                else
                {
                    CurrentMana += amount;
                }
            }

            // Update mana
            //manaBar.Update(CurrentMana);
        }

        public void Draw(SpriteBatch sb, SpriteEffects se, int camX)
        {
            // If the player is invincible, set their sprite to flash based on the game time
            Color flash = Color.White;

            // draw the player in various ways based on their invincibility,
            // and if god mode is currently active
            if (invincibilityCountdown > 0)
            {
                float flashVal = (float)Math.Sin(invincibilityCountdown * 5) * 0.5f + 0.5f;
                flash = new Color(flashVal, flashVal, flashVal, 0.5f);
            }

            if(!godMode)
            {
                sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height),
                new Rectangle(0, 0, sprite.Width, sprite.Height), flash, 0.0f, Vector2.Zero, se, 0.0f);
            }
            else
            {
                sb.Draw(sprite, new Rectangle(position.X - camX, position.Y, position.Width, position.Height),
                new Rectangle(0, 0, sprite.Width, sprite.Height), Color.Gold, 0.0f, Vector2.Zero, se, 0.0f);
            }
            

            // draw any reflected projectiles
            if (deflect.ReflectedProjectile != null)
            {
                sb.Draw(deflect.ReflectedProjectile.Sprite, new Rectangle(deflect.ReflectedProjectile.X - camX, deflect.ReflectedProjectile.Y,
                    deflect.ReflectedProjectile.Position.Width, deflect.ReflectedProjectile.Position.Height), Color.Blue);
            }
        }

        /// <summary>
        /// draws the appropriate spell based on the active spell state
        /// </summary>
        /// <param name="sb"></param>
        public void DrawSpell(Game1 game)
        {
            // depending on the state, draw the appropriate spell
            switch (SpellState)
            {
                case SpellState.BasicAttack:
                    basicAttack.DrawSpell(game.SpriteBatch, center, game.Camera);
                    break;

                case SpellState.Deflect:
                    deflect.DrawSpell(game.SpriteBatch, center, game.Camera, spellVector);
                    break;

                case SpellState.FireDash:
                    fireDash.DrawSpell(game.SpriteBatch, center, game.Camera, spellVector);
                    break;

                case SpellState.Projectile:
                    projectileSpell.DrawSpell(game.SpriteBatch, center, game.Camera, spellVector);
                    break;

                default:
                    // do nothing
                    break;
            }
        }


        public void DrawPlayerShapes(int camX)
        {
            Vector2 camCenter = new Vector2(center.X - camX, center.Y);

            ShapeBatch.CircleOutline(camCenter, 32, Color.Red);

            ShapeBatch.Line(camCenter, new Vector2(mousePosition.X - camX, mousePosition.Y), 3, Color.Red);

            // determine which other shapes to draw based on active spell
            switch (SpellState)
            {
                case SpellState.BasicAttack:
                    basicAttack.DrawSpellShape(camCenter, mousePosition, camX);
                    break;

                case SpellState.Deflect:
                    break;

                case SpellState.FireDash:
                    fireDash.DrawSpellShape(center, camX);
                    break;

                case SpellState.Projectile:
                    projectileSpell.DrawSpellShape(center, camX);
                    break;

                default:
                    // do nothing
                    break;
            }

        }
    }
}
