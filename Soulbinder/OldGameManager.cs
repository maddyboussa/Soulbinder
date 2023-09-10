using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soulbinder.GameObjects;

namespace Soulbinder
{
    // This class will be responsible for organizing and running
    // various objects and methods that make the game work

    class OldGameManager
    {
        /*
        // FIELDS =======================================================================
        // Game and Manager References
        Game1 game;
        SpriteManager spriteManager;
        TileManager tileManager;

        Random random;

        double elapsedMilliseconds;

        // Random value for which statue should attack and 
        // which attack should be performed
        int randomStatue;
        int randomAttack;

        

        // Gravity
        private Vector2 gravity;

        // Create Dialog Entity
        private Entity dialoger;

        // Create DialogBox
        private DialogBox dialog;

        // Create player
        private Player player;

        // Fields for regaining mana
        private double currentManaTime;

        // Create executioner
        private Executioner executioner;
        private StatusBar bossHealth;
        private List<Projectile> rocks;


        // Create the statues
        private Statue statue1;
        private Statue statue2;
        private Statue statue3;

        // Create Statue healthbars and tracking values
        private StatusBar statuesHealth;
        private List<int> statuesHealthValues;
        private List<Projectile> statueRocks;
        private int statuesDead;

        // Create the statue hivemind
        private Hivemind hivemind;

        // Create skeletons
        private Skeleton skeleton1;
        private Skeleton skeleton2;
        private Skeleton skeleton3;
        private Skeleton skeleton4;

        // Create Health Bar
        private StatusBar healthBar;
        private StatusBar manaBar;

        // Create GameStates
        private GameState gameState;
        private GameState gameStatePrev;

        // Get MouseStates
        private MouseState ms;
        private MouseState prevMS;

        // Get KeyboardStates
        private KeyboardState kb;
        private KeyboardState prevKB;

        // Executioner state and movement tracking
        // variables
        private BossStates executionerState;
        private PathfindingStates executionerPathfind;
        private double executionerStateCounter;
        private int attackChancesExecutioner;
        private double jumpCounterExecutioner;
        private double chanceCounterExecutioner;

        // Statue state and movement tracking
        // variables
        private List<BossStates> statueState;
        private BossStates statue1State;
        private BossStates statue2State;
        private BossStates statue3State;
        private List<PathfindingStates> statuePathfind;
        private PathfindingStates statue1Pathfind;            
        private PathfindingStates statue2Pathfind;            
        private PathfindingStates statue3Pathfind;
        private double statueStateCounter;
        private double jumpCounterStatue;

        // Skeleton states and state 
        // counter
        private SkeletonStates skeleton1State;
        private double skeleton1StateCounter;

        private SkeletonStates skeleton2State;
        private double skeleton2StateCounter;

        private SkeletonStates skeleton3State;
        private double skeleton3StateCounter;

        private SkeletonStates skeleton4State;
        private double skeleton4StateCounter;

        // Options
        private float brightness;
        private float musicVolume;

        // Create buttons
        private Button start;
        private Button options;
        private Button quit;
        private Button reset;
        private Button back;

        // Create Loss Menu Buttons
        private Button lossToMenu;
        private Button lossToCheckpoint;

        // Create Win Menu Buttons
        private Button winToMenu;

        // Create Sliders
        private Slider brightnessSlider;
        private Slider musicSlider;

        // Create Tiles
        private List<Rectangle> tiles;
        private List<Enemy> enemyList;

        // Loss Timer
        private double fadeTimer;
        private float loseFade;

        // PROPERTIES
        public GameState GameState
        {
            get { return gameState; }
        }

        // CONSTRUCTOR

        /// <summary>
        /// The creation of a new GameManager (done in Game1's Initialize class)
        /// will initialize all of the game's content
        /// </summary>
        public OldGameManager(Game1 game, GraphicsDeviceManager graphic, SpriteBatch sb, List<Rectangle> tiles)
        {
            // Initialize all the variables needed for the game
            _game1Variable = game;
            _graphics = graphic;
            graphicsEnemy = _graphics;
            _spriteBatch = sb;

            // Initialize bossLevel
            bossLevel = BossLevel.Executioner;

            // Gravity
            gravity = new Vector2(0, 0.5f);

            // Settings
            brightness = 1.0f;
            musicVolume = 1.0f;

            // Tiles
            this.tiles = tiles;

            enemyList = new List<Enemy>();

            // Set the time counter for executioner states, number
            // of executioner attacks, and current jump counts to 0
            executionerStateCounter = 0;
            attackChancesExecutioner = 4;
            jumpCounterExecutioner = 0;

            // Set the time counter for statue states, counter for
            // the statue sequence attack, and current jump counts to 0
            statueStateCounter = 0;
            jumpCounterStatue = 0;

            currentManaTime = 0;
            chanceCounterExecutioner = 0;

            // Set the time counter for skeleton states 
            // equal to 0
            skeleton1StateCounter = 0;
            skeleton2StateCounter = 0;
            skeleton3StateCounter = 0;
            skeleton4StateCounter = 0;

            // Set fade timer
            fadeTimer = 0;

            // Initialize the random number generator
            random = new Random();
        }



        // METHODS

        // NOTE: A lot of the stuff in the LoadContent Method below should probably be
        // changed as the project goes on, because it's mostly temporary stuff.

        /// <summary>
        /// Loads all of the content associatetd with running the game as
        /// well as the objects that rely on sprites, fonts, etc
        /// to be loaded before they can be initialized
        /// </summary>
        /// <param name="_graphics">Game1's graphics device manager</param>
        /// <param name="game">Reference to the Game1 object</param>
        public void LoadContent(GraphicsDeviceManager _graphics)
        {
            // Load textures and fonts
            arial16 = _game1Variable.Content.Load<SpriteFont>("Arial16");
            pixel = _game1Variable.Content.Load<Texture2D>("WhitePixel");
            playerSprite = _game1Variable.Content.Load<Texture2D>("mageSprite");

            // load spell sprites
            basicAttackSprite = _game1Variable.Content.Load<Texture2D>("basicAttack");
            fireDashSprite = _game1Variable.Content.Load<Texture2D>("FireDash");
            deflectSprite = _game1Variable.Content.Load<Texture2D>("Deflection");

            // Create Player
            player = new Player(playerSprite, new Rectangle(145, 350, 32, 64), 5, 20, 20, 10, 10, 
                basicAttackSprite, fireDashSprite, deflectSprite, enemyList, tiles);
            
            
            // Create Dialog Entity
            dialoger = new Entity(playerSprite, new Rectangle(50, 296, 32, 64), 5, 20, 20);

            // Create Dialog
            dialog = new DialogBox(pixel, new Rectangle(dialoger.X, dialoger.Y - 175, 500, 175),
                new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (75), dialoger.Y - 60, 150, 30)),
                arial16, 500, "This is Soulbinder, a single-player real-time metroidvania where the main character must learn a collection " +
                    "of spells to then take on an onslaught of bosses and recover their soul from the Lich King.");
            

            // Create Health Bar
            healthBar = new StatusBar(pixel, new Rectangle(10, 15, 480, 15), player.CurrentHealth, player.MaximumHealth, arial16);
            manaBar = new StatusBar(pixel, new Rectangle(10, 45, 480, 15), player.CurrentMana, player.MaximumMana, arial16);

            // Button fields
            int buttonWidth = 150;
            int buttonHeight = 30;

            // Button area
            int startHeight = _graphics.PreferredBackBufferHeight * 3 / 5;
            int marginsInBetween = 30;

            // Create buttons
            start = new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (buttonWidth / 2),
                startHeight, buttonWidth, buttonHeight));
            options = new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (buttonWidth / 2),
                startHeight + buttonHeight + marginsInBetween, buttonWidth, buttonHeight));
            quit = new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (buttonWidth / 2),
                startHeight + (2 * (buttonHeight + marginsInBetween)), buttonWidth, buttonHeight));

            // Slider fields
            int sliderWidth = 600;
            int sliderInteractWidth = 30;
            int sliderInteractHeight = 30;

            // Slider area
            int sliderStart = _graphics.PreferredBackBufferHeight * 1 / 5;
            int margin = 211;

            // Create Slider
            brightnessSlider = new Slider(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) + (sliderWidth / 2),
                sliderStart, sliderInteractWidth, sliderInteractHeight), brightness);
            musicSlider = new Slider(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) + (sliderWidth / 2),
                sliderStart + margin, sliderInteractWidth, sliderInteractHeight), musicVolume);


            reset = new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (buttonWidth / 2),
                sliderStart + (2 * margin), buttonWidth, buttonHeight));
            back = new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (buttonWidth / 2),
                sliderStart + (2 * margin) + 50, buttonWidth, buttonHeight));

            // Load executioner
            // ***
            // Note: Each boss will be referenced to as a "Boss" in comments and code to emphasize their shared behavior
            // ***
            executionerSprite = _game1Variable.Content.Load<Texture2D>("ExecutionerIdleV2");
            executionerHorizontalHigh = _game1Variable.Content.Load<Texture2D>("ExecutionerHorizontalHighV2");
            executionerHorizontalMid = _game1Variable.Content.Load<Texture2D>("ExecutionerHorizontalMidV2");
            executionerHorizontalLow = _game1Variable.Content.Load<Texture2D>("ExecutionerHorizontalLowV2");
            executionerVertical = _game1Variable.Content.Load<Texture2D>("ExecutionerVerticalV2");
            executionerSlam = _game1Variable.Content.Load<Texture2D>("ExecutionerSlamV2");
            executionerAttackAnims = new List<Texture2D>();
            executionerAttackAnims.Add(executionerHorizontalHigh);
            executionerAttackAnims.Add(executionerHorizontalMid);
            executionerAttackAnims.Add(executionerHorizontalLow);
            executionerAttackAnims.Add(executionerVertical);
            executionerAttackAnims.Add(executionerSlam);

            executioner = new Executioner(executionerSprite, new Rectangle(6100, 200, executionerSprite.Width / 2, 
                executionerSprite.Height / 2), 3, 150, 150, executionerAttackAnims, pixel);

            // rock stuff
            rockSprite = _game1Variable.Content.Load<Texture2D>("Rock");

            rocks = new List<Projectile>();

            // Load statue
            statueSprite = _game1Variable.Content.Load<Texture2D>("StatueIdle");
            statueStab = _game1Variable.Content.Load<Texture2D>("StatueStab");
            statueDash = _game1Variable.Content.Load<Texture2D>("StatueDashSlash");
            statueThrow = _game1Variable.Content.Load<Texture2D>("StatueThrow");
            statueDodge = _game1Variable.Content.Load<Texture2D>("StatueStepBack");
            statueRockTexture = _game1Variable.Content.Load<Texture2D>("Rock");
            statueAttackAnims = new List<Texture2D>();
            statueAttackAnims.Add(statueStab);
            statueAttackAnims.Add(statueDash);
            statueAttackAnims.Add(statueThrow);
            statueAttackAnims.Add(statueDodge);

            statue1 = new Statue(statueSprite, new Rectangle(200, 200,
                statueSprite.Width / 2, statueSprite.Height / 2), 3, 50, 50, statueAttackAnims, statueRockTexture);
            statue2 = new Statue(statueSprite, new Rectangle(600, 200,
                statueSprite.Width / 2, statueSprite.Height / 2), 3, 50, 50, statueAttackAnims, statueRockTexture);
            statue3 = new Statue(statueSprite, new Rectangle(1000, 200,
                statueSprite.Width / 2, statueSprite.Height / 2), 3, 50, 50, statueAttackAnims, statueRockTexture);

            // Set up the list of individual statue states
            statueState = new List<BossStates>();
            statueState.Add(statue1State);
            statueState.Add(statue2State);
            statueState.Add(statue3State);

            // Set up the list of individual statue pathfinding states
            statuePathfind = new List<PathfindingStates>();
            statuePathfind.Add(statue1Pathfind);
            statuePathfind.Add(statue2Pathfind);
            statuePathfind.Add(statue3Pathfind);

            // Initialize statue projectiles
            statueRocks = new List<Projectile>();

            // Create statues healthbars
            statuesHealth = new StatusBar(pixel, new Rectangle(200, _graphics.PreferredBackBufferHeight - 60, 880, 30),
                150, 150, arial16);

            statuesHealthValues = new List<int>();
            statuesHealthValues.Add(statue1.CurrentHealth);
            statuesHealthValues.Add(statue2.CurrentHealth);
            statuesHealthValues.Add(statue3.CurrentHealth);

            // Inititalize the hivemind
            hivemind = new Hivemind(statue1, statue2, statue3);

            // Load skeleton
            skeletonSprite = _game1Variable.Content.Load<Texture2D>("SkeletonV2");

            skeleton1 = new Skeleton(skeletonSprite, new Rectangle(1400, 450, 32, 64), 
                2, 20, 15, 200);
            skeleton2 = new Skeleton(skeletonSprite, new Rectangle(3000, 530, 32, 64),
                2, 20, 15, 120);
            skeleton3 = new Skeleton(skeletonSprite, new Rectangle(3320, 530, 32, 64),
                2, 20, 15, 240);
            skeleton4 = new Skeleton(skeletonSprite, new Rectangle(3840, 250, 32, 64),
                2, 20, 15, 200);

            bossHealth = new StatusBar(pixel, new Rectangle(200, _graphics.PreferredBackBufferHeight - 60, 880, 30),
                executioner.CurrentHealth, executioner.MaximumHealth, arial16);

            // add all enemies to the list of enemies
            enemyList.Add(executioner);
            enemyList.Add(skeleton1);
            enemyList.Add(skeleton2);
            enemyList.Add(skeleton3);
            enemyList.Add(skeleton4);
            //enemyList.Add(statue1);
            //enemyList.Add(statue2);
            //enemyList.Add(statue3);

            // Create loss screen buttons
            lossToMenu = new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (buttonWidth / 2) - 15,
                startHeight - 50, buttonWidth + 30, buttonHeight));
            lossToCheckpoint = new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (buttonWidth / 2) - 15,
                startHeight + (lossToMenu.Position.Height + 30) - 50, buttonWidth + 30, buttonHeight));

            // Create win screen buttons
            winToMenu = new Button(pixel, arial16, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (buttonWidth / 2) - 15,
                startHeight - 50, buttonWidth + 30, buttonHeight));
        }

        /// <summary>
        /// Checks for and updates the various game states
        /// (i.e. for the player, executioner, and game itself)
        /// </summary>
        /// <param name="gameTime">The currently elapsed game
        /// runtime</param>
        public void Update(GameTime gameTime)
        {
            elapsedMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;
            
            // Get MouseState
            ms = Mouse.GetState();

            // Get KeyboardState
            kb = Keyboard.GetState();

            // Game Finite State Machine
            switch (gameState)
            {
                case GameState.Menu:
                    UpdateMenu();
                    break;

                case GameState.Options:
                    UpdateOptions(); 

                    break;

                case GameState.Keybind:
                    // Left empty until keyboard things are implemented
                    break;

                case GameState.Game:
                    gameStatePrev = GameState.Game;
                    UpdatePlayer(player);
                    _game1Variable.CamX = player.X - (_graphics.PreferredBackBufferWidth / 2);

                    // Check if the player can interact with the dialoger
                    if (player.Collides(dialoger) && (kb.IsKeyDown(Keys.F) && prevKB.IsKeyUp(Keys.F)))
                    {
                        dialog.Show = true;
                    }

                    if (dialog.BoxButton.Clicked(ms) || CalcDistance(player, dialoger) > 200)
                    {
                        dialog.Show = false;
                    }

                    // If the O button is pressed, go to options
                    if (kb.IsKeyDown(Keys.O))
                    {
                        gameState = GameState.Options;
                    }

                    // Check collisions for the skeletons
                    // Skelly 1
                    skeleton1.ResolveCollisions(tiles);

                    if (!skeleton1.Dead)
                    {
                        // Check and update the skeleton state
                        switch (skeleton1State)
                        {
                            case SkeletonStates.Move:
                                
                                // Determime if the rectangle between the player and the 
                                // skeleton is within the skeleton's patrol space
                                if (skeleton1.WithinLineOfSight(player))
                                {
                                    // Change the state to Idle if the player has been
                                    // seen
                                    skeleton1State = SkeletonStates.Idle;
                                    // Otherwise, allow the skeleton to move
                                }
                                else
                                {
                                    skeleton1.Move();
                                }
                                break;
                            case SkeletonStates.Idle:
                                // After 0.4 seconds, change the state to Aggro and 
                                // increase the movement speed of the skeleton
                                if (skeleton1StateCounter > 0.4)
                                {
                                    skeleton1.Speed = 5;
                                    skeleton1State = SkeletonStates.Aggro;
                                    skeleton1StateCounter = 0;
                                    // Otherwise, continue to count the elapsed game time
                                }
                                else
                                {
                                    skeleton1StateCounter +=
                                        gameTime.ElapsedGameTime.TotalSeconds;
                                }
                                break;
                            case SkeletonStates.Aggro:
                                // If the rectangle between the player and the skeleton
                                // is no longer within the patrol space of the skeletion,
                                // change the state back to Move
                                if (skeleton1.WithinLineOfSight(player))
                                {
                                    skeleton1.Move();

                                    // If the skeleton hasn't deal damage to the player,
                                    // check for collision - then deal damage if it collides
                                    if (skeleton1.Collides(player))
                                    {
                                        if (!player.IsInvincible)
                                        {
                                            player.DealDamage(healthBar, 3);
                                            player.StartInvincibility();
                                        }

                                    }
                                    // Otherwise, allow the skeleton to move
                                }
                                else
                                {
                                    skeleton1State = SkeletonStates.Move;
                                    skeleton1.Speed = 2;
                                }
                                break;
                        }
                    }
                    skeleton1.ResolveCollisions(tiles);

                    // Skelly 2
                    if (!skeleton2.Dead)
                    {
                        // Check and update the skeleton state
                        switch (skeleton2State)
                        {
                            case SkeletonStates.Move:
                                
                                // Determime if the rectangle between the player and the 
                                // skeleton is within the skeleton's patrol space
                                if (skeleton2.WithinLineOfSight(player))
                                {
                                    // Change the state to Idle if the player has been
                                    // seen
                                    skeleton2State = SkeletonStates.Idle;
                                    // Otherwise, allow the skeleton to move
                                }
                                else
                                {
                                    skeleton2.Move();
                                }
                                break;
                            case SkeletonStates.Idle:
                                // After 0.4 seconds, change the state to Aggro and 
                                // increase the movement speed of the skeleton
                                if (skeleton2StateCounter > 0.4)
                                {
                                    skeleton2.Speed = 5;
                                    skeleton2State = SkeletonStates.Aggro;
                                    skeleton2StateCounter = 0;
                                    // Otherwise, continue to count the elapsed game time
                                }
                                else
                                {
                                    skeleton2StateCounter +=
                                        gameTime.ElapsedGameTime.TotalSeconds;
                                }
                                break;
                            case SkeletonStates.Aggro:
                                // If the rectangle between the player and the skeleton
                                // is no longer within the patrol space of the skeletion,
                                // change the state back to Move
                                if (skeleton2.WithinLineOfSight(player))
                                {
                                    skeleton2.Move();

                                    // If the skeleton hasn't deal damage to the player,
                                    // check for collision - then deal damage if it collides
                                    if (skeleton2.Collides(player))
                                    {
                                        if (!player.IsInvincible)
                                        {
                                            player.DealDamage(healthBar, 3);
                                            player.StartInvincibility();
                                        }

                                    }
                                    // Otherwise, allow the skeleton to move
                                }
                                else
                                {
                                    skeleton2State = SkeletonStates.Move;
                                    skeleton2.Speed = 2;
                                }
                                break;
                        }
                    }
                    skeleton2.ResolveCollisions(tiles);

                    if (!skeleton3.Dead)
                    {
                        // Check and update the skeleton state
                        switch (skeleton3State)
                        {
                            case SkeletonStates.Move:
                                
                                // Determime if the rectangle between the player and the 
                                // skeleton is within the skeleton's patrol space
                                if (skeleton3.WithinLineOfSight(player))
                                {
                                    // Change the state to Idle if the player has been
                                    // seen
                                    skeleton3State = SkeletonStates.Idle;
                                    // Otherwise, allow the skeleton to move
                                }
                                else
                                {
                                    skeleton3.Move();
                                }
                                break;
                            case SkeletonStates.Idle:
                                // After 0.4 seconds, change the state to Aggro and 
                                // increase the movement speed of the skeleton
                                if (skeleton3StateCounter > 0.4)
                                {
                                    skeleton3.Speed = 5;
                                    skeleton3State = SkeletonStates.Aggro;
                                    skeleton3StateCounter = 0;
                                    // Otherwise, continue to count the elapsed game time
                                }
                                else
                                {
                                    skeleton3StateCounter +=
                                        gameTime.ElapsedGameTime.TotalSeconds;
                                }
                                break;
                            case SkeletonStates.Aggro:
                                // If the rectangle between the player and the skeleton
                                // is no longer within the patrol space of the skeletion,
                                // change the state back to Move
                                if (skeleton3.WithinLineOfSight(player))
                                {
                                    skeleton3.Move();

                                    // If the skeleton hasn't deal damage to the player,
                                    // check for collision - then deal damage if it collides
                                    if (skeleton3.Collides(player))
                                    {
                                        if (!player.IsInvincible)
                                        {
                                            player.DealDamage(healthBar, 3);
                                            player.StartInvincibility();
                                        }

                                    }
                                    // Otherwise, allow the skeleton to move
                                }
                                else
                                {
                                    skeleton3State = SkeletonStates.Move;
                                    skeleton3.Speed = 2;
                                }
                                break;
                        }
                    }
                    skeleton3.ResolveCollisions(tiles);

                    // Skelly 4
                    if (!skeleton4.Dead)
                    {
                        // Check and update the skeleton state
                        switch (skeleton4State)
                        {
                            case SkeletonStates.Move:
                                
                                // Determime if the rectangle between the player and the 
                                // skeleton is within the skeleton's patrol space
                                if (skeleton4.WithinLineOfSight(player))
                                {
                                    // Change the state to Idle if the player has been
                                    // seen
                                    skeleton4State = SkeletonStates.Idle;
                                    // Otherwise, allow the skeleton to move
                                }
                                else
                                {
                                    skeleton4.Move();
                                }
                                break;
                            case SkeletonStates.Idle:
                                // After 0.4 seconds, change the state to Aggro and 
                                // increase the movement speed of the skeleton
                                if (skeleton4StateCounter > 0.4)
                                {
                                    skeleton4.Speed = 5;
                                    skeleton4State = SkeletonStates.Aggro;
                                    skeleton4StateCounter = 0;
                                    // Otherwise, continue to count the elapsed game time
                                }
                                else
                                {
                                    skeleton4StateCounter +=
                                        gameTime.ElapsedGameTime.TotalSeconds;
                                }
                                break;
                            case SkeletonStates.Aggro:
                                // If the rectangle between the player and the skeleton
                                // is no longer within the patrol space of the skeletion,
                                // change the state back to Move
                                if (skeleton4.WithinLineOfSight(player))
                                {
                                    skeleton4.Move();

                                    // If the skeleton hasn't deal damage to the player,
                                    // check for collision - then deal damage if it collides
                                    if (skeleton4.Collides(player))
                                    {
                                        if (!player.IsInvincible)
                                        {
                                            player.DealDamage(healthBar, 3);
                                            player.StartInvincibility();
                                        }

                                    }
                                    // Otherwise, allow the skeleton to move
                                }
                                else
                                {
                                    skeleton4State = SkeletonStates.Move;
                                    skeleton4.Speed = 2;
                                }
                                break;
                        }
                    }
                    skeleton4.ResolveCollisions(tiles);

                    // Update Boss
                    switch (bossLevel)
                    {
                        case BossLevel.Executioner:
                            // Check and update the executioner state
                            if (!executioner.Dead && (CalcDistance(player, executioner) < (graphicsEnemy.PreferredBackBufferWidth * 1.2)))
                            {
                                // set the player's projectile list to rocks
                                player.ProjectileList = rocks;

                                // Get distance measurements from the center of the Executioner to the player in the
                                // x- and y- directions for attack attempts evaluation
                                double distanceFromExecutionerX = CalcDistanceFromCenterX(player, executioner);
                                double distanceFromExecutionerY = CalcDistanceFromCenterY(player, executioner);
                                double requiredDistanceExecutionerX = executioner.Position.Width;
                                double requiredDistanceExecutionerY = executioner.Position.Height;

                                // Get the collision side of the boss
                                executioner.CollidesWithEnvironment(tiles);

                                executioner.ResolveCollisions(tiles);
                                switch (executionerState)
                                {
                                    case BossStates.Move:
                                        // Reset the rocks
                                        rocks.Clear();

                                        // Gravity will control the Boss's vertical movement
                                        executioner.ApplyGravity(executioner.Gravity);

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
                                            jumpCounterExecutioner += gameTime.ElapsedGameTime.TotalSeconds;

                                            // Add to the executioner state counter the current elapsed
                                            // game time to determine when to change states
                                            executionerStateCounter +=
                                                gameTime.ElapsedGameTime.TotalSeconds;

                                            // Determine whether the Boss needs to find a route to the
                                            // player                                
                                            switch (executionerPathfind)
                                            {
                                                case PathfindingStates.Move:
                                                    // Check if the Boss has collided with something 
                                                    // from the right or left
                                                    if (executioner.BossCollisionSide == CollisionSide.Right ||
                                                        executioner.BossCollisionSide == CollisionSide.Left)
                                                    {
                                                        // Transition the state to check the Boss's location
                                                        // with the player's
                                                        executionerPathfind = PathfindingStates.CheckVertical;
                                                    }

                                                    // Move the Boss at the pathfinding speed
                                                    executioner.X += executioner.Speed;
                                                    break;
                                                case PathfindingStates.CheckVertical:
                                                    // Check the Boss's vertical position against the player's
                                                    // and jump if needed
                                                    if (executioner.Y > player.Y && jumpCounterExecutioner > 1)
                                                    {
                                                        executioner.Jump();
                                                        jumpCounterExecutioner = 0;
                                                    }

                                                    // Transition the state to check the Boss's horizontal
                                                    // position against the player's
                                                    executionerPathfind = PathfindingStates.CheckHorizontal;
                                                    break;
                                                case PathfindingStates.CheckHorizontal:
                                                    // Check the Boss's horizontal position against the player's
                                                    // and chnage direction if needed
                                                    if (executioner.X > player.X)
                                                    {
                                                        executioner.Speed = -3;
                                                    }
                                                    else
                                                    {
                                                        executioner.Speed = 3;
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
                                            executioner.Speed = 6;
                                            executionerState = BossStates.Attack;
                                            executionerStateCounter = 0;
                                        }
                                        else
                                        {
                                            // Prevent the Boss from freezing in mid-air
                                            // and continue to count the time until
                                            // the state needs to be changed
                                            executioner.ApplyGravity(gravity);
                                            executionerStateCounter +=
                                                gameTime.ElapsedGameTime.TotalSeconds;
                                        }
                                        break;

                                    case BossStates.Attack:
                                        // Gravity will control the Boss's vertical movement
                                        executioner.ApplyGravity(executioner.Gravity);

                                        for (int i = 0; i < rocks.Count; i++)
                                        {
                                            rocks[i].ApplyGravity(gravity / 2);
                                            //rocks[i].ResolveCollisions(tiles);

                                            if (rocks[i].Collides(player) && player.SpellState != SpellState.Deflect)
                                            {
                                                if (!player.IsInvincible)
                                                {
                                                    player.DealDamage(healthBar, rocks[i].Damage);
                                                    player.StartInvincibility(1000);
                                                    rocks.RemoveAt(i--);
                                                }
                                            }
                                        }

                                        if (attackChancesExecutioner > 0)
                                        {
                                            chanceCounterExecutioner += gameTime.ElapsedGameTime.TotalSeconds;
                                            if (chanceCounterExecutioner > 1)
                                            {
                                                // Choose attacks based on range
                                                if (distanceFromExecutionerX <= requiredDistanceExecutionerX &&
                                                    distanceFromExecutionerY <= requiredDistanceExecutionerY)
                                                {
                                                    // If the player is within melee, do a melee attack
                                                    Random rng = new Random();
                                                    int attackChosenClose = rng.Next(1, 5);
                                                    executioner.Attack(player, healthBar, gravity, rocks, attackChosenClose, _game1Variable.CamX);
                                                }
                                                else
                                                {
                                                    // Otherwise, do a ranged attack by making rocks fall
                                                    executioner.Attack(player, healthBar, gravity, rocks, 5, _game1Variable.CamX);
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
                                            executioner.Speed = 3;
                                            executionerState = BossStates.Move;
                                            executionerStateCounter = 0;
                                            attackChancesExecutioner = 4;
                                            executioner.AttackState = ExecutionerState.None;
                                            executioner.PrevAttackState = ExecutionerState.None;
                                        }
                                        else
                                        {
                                            // Continue to move the Boss and count the time
                                            // until the state needs to be changed
                                            executionerStateCounter +=
                                                gameTime.ElapsedGameTime.TotalSeconds;

                                            // COMMENTED THIS OUT FOR NOW BECAUSE I WANTED TO HAVE THE BOSS NOT MOVE - Abby
                                            //executioner.Move(player.X, player.Y);
                                        }
                                        break;
                                } 
                            }

                            // Reset the collision side of the Boss
                            executioner.BossCollisionSide = CollisionSide.None;

                            /*
                            if (kb.IsKeyDown(Keys.D2))
                            {
                                bossLevel = BossLevel.Statue;
                            }
                            break;

                        case BossLevel.Statue:
                            // Check how many statues have died
                            statuesDead = 0;
                            foreach (Statue s in hivemind.Statues)
                            {
                                if (s.Dead)
                                {
                                    statuesDead++;
                                }
                            }

                            // Create a list of distances for the Statues
                            List<double> distancesX = new List<double>();
                            List<double> distancesY = new List<double>();

                            for (int i = 0; i < hivemind.Statues.Count; i++)
                            {
                                // Get distance measurements from the center of the second Boss to the player in the
                                // x- and y- directions for attack attempts evaluation
                                distancesX.Add(CalcDistanceFromCenterX(player, hivemind.Statues[i]));
                                distancesY.Add(CalcDistanceFromCenterY(player, hivemind.Statues[i]));

                                // Get the collision side of the Boss
                                hivemind.Statues[i].CollidesWithEnvironment(tiles);

                                // Resolve the collisions of the Boss
                                hivemind.Statues[i].ResolveCollisions(tiles);
                            }

                            // If there are statues alive, have those statues act
                            if (!(statuesDead >= 3))
                            {
                                foreach (Statue s in hivemind.Statues)
                                {
                                    s.ApplyGravity(gravity);
                                    s.ResolveCollisions(tiles);

                                    // Resolve collisions and gravity for any rocks being thrown
                                    foreach (Projectile p in statueRocks)
                                    {
                                        // Target center of player
                                        Vector2 rockVelocity = new Vector2(((player.X + player.Position.Width) / 2.0f) - s.Position.X,
                                            ((player.Y + player.Position.Height) / 2.0f) - s.Position.Y);

                                        p.Velocity += rockVelocity;
                                        p.ApplyGravity(gravity);
                                        if (p.Collides(player))
                                        {
                                            if (!p.DealtDamage)
                                            {
                                                player.DealDamage(healthBar, p.Damage);
                                                p.DealtDamage = true;
                                            }
                                        }

                                        p.ResolveCollisions(tiles);
                                    }
                                }

                                // Store whether or not the Statues has attacked
                                bool hasAttacked = false;

                                // Get the random attack and statue
                                randomAttack = random.Next(2, 5);
                                randomStatue = random.Next(0, 3);

                                for (int i = 0; i < statueState.Count; i++)
                                {
                                    // Check and update the Statue state
                                    switch (statueState[i])
                                    {
                                        case BossStates.Move:
                                            // Gravity will control the Boss' vertical movement
                                            hivemind.Statues[i].ApplyGravity(hivemind.Statues[i].Gravity);

                                            // Reposition only if the three Statues have not attacked
                                            if (!hasAttacked)
                                            {
                                                // Make sure any two Statues don't get to close to each other
                                                //hivemind.Reposition(jumpCounterStatue);
                                            }

                                            // Change the state to idle after 10 seconds
                                            if (statueStateCounter > 10)
                                            {
                                                statueState[i] = BossStates.Idle;
                                                statueStateCounter = 0;
                                            }
                                            else
                                            {
                                                // Add to the jump counter the current elapsed game time
                                                // to create a steady pathfinding jump
                                                jumpCounterStatue += gameTime.ElapsedGameTime.TotalSeconds;

                                                // Add to the statue state counter the current elapsed
                                                // game time to determine when to change states
                                                statueStateCounter +=
                                                    gameTime.ElapsedGameTime.TotalSeconds;

                                                // Determine whether the Bosses need to find a route to the
                                                // player

                                                switch (statuePathfind[i])
                                                {
                                                    case PathfindingStates.Move:
                                                        // Check if the Bosses have collided with something 
                                                        // from the right or left
                                                        if (hivemind.Statues[i].BossCollisionSide == CollisionSide.Right ||
                                                            hivemind.Statues[i].BossCollisionSide == CollisionSide.Left)
                                                        {
                                                            // Transition the state to check the Boss' vertical
                                                            // position with the player's
                                                            statuePathfind[i] = PathfindingStates.CheckVertical;
                                                        }

                                                        // Move the Bosses at the pathfinding speed
                                                        hivemind.Statues[i].X += hivemind.Statues[i].Speed;
                                                        break;
                                                    case PathfindingStates.CheckVertical:
                                                        // Check the Boss' vertical position against the player's
                                                        // and jump if needed
                                                        if (hivemind.Statues[i].Y + hivemind.Statues[i].Position.Height !=
                                                            player.Y && jumpCounterStatue > 1)
                                                        {
                                                            hivemind.Statues[i].Jump();
                                                            jumpCounterStatue = 0;
                                                        }

                                                        // Transition the state to check the Boss' horizontal
                                                        // position against the player's
                                                        statuePathfind[i] = PathfindingStates.CheckHorizontal;
                                                        break;
                                                    case PathfindingStates.CheckHorizontal:
                                                        // Check the Boss' horizontal position against the player's
                                                        // and change direction if needed
                                                        if (hivemind.Statues[i].X > player.X)
                                                        {
                                                            hivemind.Statues[i].Speed = -3;
                                                        }
                                                        else
                                                        {
                                                            hivemind.Statues[i].Speed = 3;
                                                        }

                                                        // Transition the state back to continue moving
                                                        statuePathfind[i] = PathfindingStates.Move;
                                                        break;
                                                }
                                            }
                                            break;
                                        case BossStates.Idle:
                                            // Change the state to Attack after 1 second
                                            if (statueStateCounter > 1)
                                            {
                                                statueState[i] = BossStates.Attack;
                                                // Increase the Boss' speed for an attack
                                                hivemind.Statues[i].Speed = 6;
                                                statueStateCounter = 0;
                                            }
                                            else
                                            {
                                                // Prevent the Bosses from freezing in mid-air
                                                // and continue to count the time until
                                                // the state needs to be changed
                                                hivemind.Statues[i].ApplyGravity(gravity);
                                                statueStateCounter +=
                                                    gameTime.ElapsedGameTime.TotalSeconds;
                                            }
                                            break;

                                        case BossStates.Attack:
                                            // Store if all distances in the x- and y- directions for all statues
                                            // are within the required range
                                            bool closeEnoughInX = true;
                                            bool closeEnoughInY = true;

                                            // Gravity will control the Boss' vertical movement
                                            hivemind.Statues[i].ApplyGravity(hivemind.Statues[i].Gravity);

                                            double distanceFromBossX = CalcDistanceFromCenterX(player, statue1);
                                            double distanceFromBossY = CalcDistanceFromCenterY(player, statue1);
                                            double requiredDistanceX = statue1.Position.Width / 2.0;
                                            double requiredDistanceY = statue1.Position.Height / 2.0;

                                            // Evaluate if the Bosses are close enough to attack yet in the x- 
                                            // and y- directions

                                            if (distancesX[i] > requiredDistanceX)
                                            {
                                                closeEnoughInX = false;
                                            }

                                            // Evaluate if the Bosses are close enough to attack yet in the x- 
                                            // and y- directions                                
                                            if (distancesY[i] > requiredDistanceY)
                                            {
                                                closeEnoughInY = false;
                                            }

                                            // Allow the Bosses to attack
                                            if (closeEnoughInX && closeEnoughInY)
                                            {
                                                hasAttacked = hivemind.Statues[i].Attack(player, healthBar, gameTime.ElapsedGameTime.TotalSeconds, 
                                                    statueRocks, _game1Variable.CamX);
                                            }

                                            // Change back to the Move state after 2 seconds
                                            if (statueStateCounter > 5)
                                            {
                                                // Reset the Boss state counter and Boss speed
                                                // variables and whether or the Boss has dashed
                                                // a single time this attack phase
                                                statueState[i] = BossStates.Move;
                                                statueStateCounter = 0;
                                                hivemind.Statues[i].Speed = 3;
                                                hivemind.Statues[i].HasDashed = false;
                                            }
                                            else
                                            {
                                                // Continue to move the Boss and count the time
                                                // until the state needs to be changed
                                                statueStateCounter +=
                                                gameTime.ElapsedGameTime.TotalSeconds;
                                                if (hasAttacked)
                                                {
                                                    hivemind.Statues[i].StepBack(player.X - hivemind.Statues[i].X);
                                                    hivemind.Statues[i].HasDashed = true;
                                                    hivemind.Statues[i].Speed /= 3;
                                                }

                                                // Reset the speed and continue to move 
                                                // the Boss
                                                hivemind.Statues[i].Move(player.X, player.Y);
                                            }

                                            // Reset that the Statues have attacked
                                            hasAttacked = false;
                                            break;
                                        case BossStates.SequenceAttack:
                                            statueState[i] = BossStates.Move;
                                            break;
                                        case BossStates.SimultaneousAttack:
                                            break;
                                    }
                                }

                                // Reset the collision side of the Statues
                                for (int i = 0; i < hivemind.Statues.Count; i++)
                                {
                                    hivemind.Statues[i].BossCollisionSide = CollisionSide.None;
                                }

                                // Switch to statue if 2 is pressed
                                if (kb.IsKeyDown(Keys.D1))
                                {
                                    bossLevel = BossLevel.Executioner;
                                }
                                break;
                            }
                            break;
                    }
                    break;
                case GameState.Lose:
                    UpdateLoss();
                    break;

                case GameState.Win:
                    UpdateWin();
                    break;
            }

            // update executioner health bar
            bossHealth.Update(executioner.CurrentHealth);

            for(int i = 0; i < hivemind.Statues.Count; i++)
            {
                statuesHealthValues[i] = hivemind.Statues[i].CurrentHealth;
            }

            // Update the statue health bars
            statuesHealth.Update(statuesHealthValues[0], statuesHealthValues[1], statuesHealthValues[2]);

            // Determine if statues are dead
            if(statuesHealthValues[0] <= 0)
            {
                hivemind.Statues[0].Dead = true;
            }
            if(statuesHealthValues[1] <= 0)
            {
                hivemind.Statues[1].Dead = true;
            }
            if(statuesHealthValues[2] <= 0)
            {
                hivemind.Statues[2].Dead = true;
            }

            // Check for death state
            if (player.CurrentHealth <= 0)
            {
                TransitionGame(GameState.Lose, gameTime);
            }

            // If the executioner is dead, set the GameState to the Win
            // GameState
            if (executioner.Dead)
            {
                TransitionGame(GameState.Win, gameTime);
            }

            // Set previous MouseState to current MouseState
            prevMS = ms;

            // Set previous KeyboardState to current KeyboardState
            prevKB = kb;
        }

        // Methods
        /// <summary>
        /// Check if the left mouse button is being held
        /// </summary>
        /// <param name="ms">The MouseState to check</param>
        /// <returns>True if the left mouse button is being held, false is not</returns>
        public bool MouseHeld(MouseState ms)
        {
            // Check if both the last state and the current state are both pressing the left mouse button
            if (ms.LeftButton == ButtonState.Pressed && prevMS.LeftButton == ButtonState.Pressed)
            {
                // If so, return true
                return true;
            }

            // Otherwise, return false
            else return false;
        }

        /// <summary>
        /// Calculate the distance between two GameObjects
        /// </summary>
        /// <param name="go1">The first GameObject</param>
        /// <param name="go2">The second GameObject</param>
        /// <returns>The distance between two GameObjects</returns>
        double CalcDistance(GameObject go1, GameObject go2)
        {
            return Math.Sqrt(Math.Pow(go1.X - go2.X, 2) + Math.Pow(go1.Y - go2.Y, 2));
        }

        /// <summary>
        /// Returns the difference between the x-positions of two GameObjects
        /// </summary>
        /// <param name="go1">The first GameObject</param>
        /// <param name="go2">The second GameObject</param>
        /// <returns>The x distance between the two GameObjects</returns>
        public double CalcDistanceFromCenterX(GameObject go1, GameObject go2)
        {
            return Math.Abs((go2.X + go2.Position.Width / 2) - (go1.X + go1.Position.Width / 2));
        }

        /// <summary>
        /// Returns the difference between the y-positions of two GameObjects
        /// </summary>
        /// <param name="go1">The first GameObject</param>
        /// <param name="go2">The second GameObject</param>
        /// <returns>The y distance between the two GameObjects</returns>
        public double CalcDistanceFromCenterY(GameObject go1, GameObject go2)
        {
            return Math.Abs((go2.Y + go2.Position.Height / 2) - (go1.Y + go1.Position.Height / 2));
        }

        /// <summary>
        /// Transition the game by locking the player and setting a fade, then transition to a new GameState
        /// </summary>
        /// <param name="newGameState"></param>
        /// <param name="gameTime"></param>
        public void TransitionGame(GameState newGameState, GameTime gameTime)
        {
            // Lock the player
            player.Locked = true;

            // Set a timer before losing
            fadeTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (fadeTimer > 3)
            {
                gameState = newGameState;
            }
        }

        /// <summary>
        /// Draw the fade overlay for transitions
        /// </summary>
        public void DrawFade()
        {
            loseFade = 1.0f - ((3 * brightness) - (float)fadeTimer);

            _spriteBatch.Draw(pixel, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight), Color.Black * loseFade);
        }

        /// <summary>
        /// NOTE: The code in this method is TEMPORARY and subject to change
        /// </summary>
        private void UpdateMenu()
        {
            gameStatePrev = GameState.Menu;

            // If the Start button is pressed, start the game
            if (start.Clicked(ms))
            {
                gameState = GameState.Game;
            }

            // If the Options button is pressed, go to options
            if (options.Clicked(ms))
            {
                gameState = GameState.Options;
            }

            // If the Quit button is pressed, quit the game
            if (quit.Clicked(ms))
            {
                _game1Variable.Exit();
            }
        }

        /// <summary>
        /// NOTE: The code in this method is TEMPORARY and subject to change
        /// (it might even be deleted)
        /// Update the Options screen
        /// </summary>
        private void UpdateOptions()
        {
            // Brightness Slider movement
            brightnessSlider.SliderHeld(ms, prevMS, _graphics);

            // Music Slider movement
            musicSlider.SliderHeld(ms, prevMS, _graphics);

            // If the reset button is pressed, reset the controls to default
            if (reset.Clicked(ms))
            {
                brightnessSlider.Reset();
                musicSlider.Reset();
            }

            // Set brightness
            brightness = brightnessSlider.SliderValue;

            // Set volume
            musicVolume = musicSlider.SliderValue;
            MediaPlayer.Volume = musicVolume;

            // If the back button is pressed, go back to the previous screen
            if (back.Clicked(ms))
            {
                gameState = gameStatePrev;
            }
        }

        /// <summary>
        /// Update the Loss Screen
        /// </summary>
        void UpdateLoss()
        {
            // Reset Game
            Reset();

            // Set button functions
            if (lossToMenu.Clicked(ms))
            {
                gameState = GameState.Menu;
            }

            if (lossToCheckpoint.Clicked(ms))
            {
                gameState = GameState.Game;
            }
        }

        /// <summary>
        /// Update the Win Screen
        /// </summary>
        void UpdateWin()
        {
            // Reset Game
            Reset();

            // Set button functions
            if (winToMenu.Clicked(ms))
            {
                gameState = GameState.Menu;
            }
        }

        /// <summary>
        /// Update the player
        /// </summary>
        /// <param name="player">The player to update</param>
        void UpdatePlayer(Player player)
        {
            // PRESS G TO ENTER GOD MODE
            if (kb.IsKeyDown(Keys.G) && prevKB.IsKeyUp(Keys.G))
            {
                if (!player.GodMode)
                {
                    player.GodMode = true;
                } else player.GodMode = false;
            }


            // Movement updates
            if (!player.Locked)
            {
                player.ApplyGravity(gravity);
                player.Move(kb, prevKB, Keys.Space, Keys.A, Keys.D, elapsedMilliseconds);
                
                player.ResolveCollisions(tiles);
                player.KillOutOfBounds(healthBar, _graphics.PreferredBackBufferHeight);

                // update spell stuff
                player.CastSpell(kb, prevKB, ms, prevMS, elapsedMilliseconds, _game1Variable.CamX);

                // Update player's previous position
                player.PrevPosition = player.Position;
            }

            // regain 1 point of mana every 3 seconds
            currentManaTime += elapsedMilliseconds;

            if (currentManaTime > 3000)
            {
                if (player.CurrentMana < player.MaximumMana)
                {
                    player.CurrentMana += 1;
                }
                currentManaTime = 0;
            }

            // StatusBar updates
            player.UseMana(kb, prevKB, manaBar, 1);
        }

        /// <summary>
        /// Draws the various game elements to the screen
        /// each frame
        /// </summary>
        /// <param name="gameTime">The currently elapsed game
        /// runtime</param>
        public void Draw(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Menu:
                    DrawMenu();
                    break;

                case GameState.Options:
                    DrawOptions();
                    break;

                case GameState.Keybind:
                    // To be implementer later (maybe)
                    break;

                case GameState.Game:
                    DrawGame();
                    break;

                case GameState.Win:
                    DrawWin();
                    break;

                case GameState.Lose:
                    DrawLoss();
                    break;

                case GameState.Pause:
                    break;
            }

            // Apply Brightness
            float brightnessReverse = 1.0f - brightness;

            _spriteBatch.Draw(pixel, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight), Color.Black * brightnessReverse);
        }

        /// <summary>
        /// Draw the Menu screen
        /// </summary>
        void DrawMenu()
        {
            // Draw the buttons
            start.Draw(_spriteBatch, "Start", start.ButtonShape, Alignment.Center, Color.White, Color.Black);
            options.Draw(_spriteBatch, "Options", options.ButtonShape, Alignment.Center, Color.White, Color.Black);
            quit.Draw(_spriteBatch, "Quit", quit.ButtonShape, Alignment.Center, Color.White, Color.Black);

            // If the buttons are being hovered over, highlight them
            if (start.MouseInBounds(ms))
            {
                start.Draw(_spriteBatch, "Start", start.ButtonShape, Alignment.Center, Color.Red, Color.Black);
            }

            if (options.MouseInBounds(ms))
            {
                options.Draw(_spriteBatch, "Options", options.ButtonShape, Alignment.Center, Color.Red, Color.Black);
            }

            if (quit.MouseInBounds(ms))
            {
                quit.Draw(_spriteBatch, "Quit", quit.ButtonShape, Alignment.Center, Color.Red, Color.Black);
            }
        }

        /// <summary>
        /// Draw the Options screen
        /// </summary>
        void DrawOptions()
        {
            // Draw Brightness Box
            _spriteBatch.Draw(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (1100 / 2),
        brightnessSlider.Y - 50, 1100, 130), Color.White);

            _spriteBatch.Draw(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (1090 / 2),
        brightnessSlider.Y - 45, 1090, 120), Color.Black);

            // Draw Brightness Slider and Value
            _spriteBatch.DrawString(arial16, "Brightness", new Vector2((_graphics.PreferredBackBufferWidth / 2) - (600 / 2) - 165,
                brightnessSlider.Y), Color.White);

            brightnessSlider.Draw(_spriteBatch, Color.White);

            _spriteBatch.Draw(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (600 / 2),
        brightnessSlider.Y + (brightnessSlider.Position.Height / 2) - (5 / 2), 600, 5), Color.White);

            _spriteBatch.DrawString(arial16, "Value " + String.Format("{0:0.00}", brightness),
                new Vector2((_graphics.PreferredBackBufferWidth / 2) + (600 / 2) + 80,
                brightnessSlider.Y), Color.White);

            // Change color if the slider is being held
            if (brightnessSlider.BeingHeld == true)
            {
                brightnessSlider.Draw(_spriteBatch, Color.Red);

                _spriteBatch.Draw(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (600 / 2),
        brightnessSlider.Y + (brightnessSlider.Position.Height / 2) - (5 / 2), 600, 5), Color.Red);

                _spriteBatch.DrawString(arial16, "Value " + String.Format("{0:0.00}", brightness),
                new Vector2((_graphics.PreferredBackBufferWidth / 2) + (600 / 2) + 80,
                brightnessSlider.Y), Color.Red);
            }

            // Draw Music Box
            _spriteBatch.Draw(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (1100 / 2),
        musicSlider.Y - 50, 1100, 130), Color.White);

            _spriteBatch.Draw(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (1090 / 2),
        musicSlider.Y - 45, 1090, 120), Color.Black);

            // Draw Music Slider and Value
            _spriteBatch.DrawString(arial16, "Volume", new Vector2((_graphics.PreferredBackBufferWidth / 2) - (600 / 2) - 150,
                musicSlider.Y), Color.White);

            musicSlider.Draw(_spriteBatch, Color.White);

            _spriteBatch.Draw(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (600 / 2),
        musicSlider.Y + (musicSlider.Position.Height / 2) - (5 / 2), 600, 5), Color.White);

            _spriteBatch.DrawString(arial16, "Value " + String.Format("{0:0.00}", musicVolume),
                new Vector2((_graphics.PreferredBackBufferWidth / 2) + (600 / 2) + 80,
                musicSlider.Y), Color.White);

            // Change color if the slider is being held
            if (musicSlider.BeingHeld == true)
            {
                musicSlider.Draw(_spriteBatch, Color.Red);

                _spriteBatch.Draw(pixel, new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (600 / 2),
        musicSlider.Y + (musicSlider.Position.Height / 2) - (5 / 2), 600, 5), Color.Red);

                _spriteBatch.DrawString(arial16, "Value " + String.Format("{0:0.00}", musicVolume),
                new Vector2((_graphics.PreferredBackBufferWidth / 2) + (600 / 2) + 80,
                musicSlider.Y), Color.Red);
            }

            // Draw Buttons
            reset.Draw(_spriteBatch, "Reset", reset.ButtonShape, Alignment.Center, Color.White, Color.Black);
            back.Draw(_spriteBatch, "Back", back.ButtonShape, Alignment.Center, Color.White, Color.Black);

            if (reset.MouseInBounds(ms))
            {
                reset.Draw(_spriteBatch, "Reset", reset.ButtonShape, Alignment.Center, Color.Red, Color.Black);
            }

            if (back.MouseInBounds(ms))
            {
                back.Draw(_spriteBatch, "Back", back.ButtonShape, Alignment.Center, Color.Red, Color.Black);
            }
        }

        /// <summary>
        /// Draw the Game screen
        /// </summary>
        void DrawGame()
        {
            // Draw Status Bars
            _spriteBatch.Draw(pixel, new Rectangle(0, 0, 500, 75), Color.Gray);
            healthBar.Draw(_spriteBatch, "Health", healthBar.Position, Alignment.Center, Color.Black, Color.Red, Color.White);
            manaBar.Draw(_spriteBatch, "Mana", manaBar.Position, Alignment.Center, Color.Black, Color.Blue, Color.White);

            // Draw Player Pos (for Testing)
            _spriteBatch.DrawString(arial16, "Current X Pos: " + player.X, new Vector2(900, 80), Color.Black);
            _spriteBatch.DrawString(arial16, "Current Y Pos: " + player.Y, new Vector2(900, 100), Color.Black);

            // Draw the skeletons
            if (!skeleton1.Dead)
            {
                switch (skeleton1State)
                {
                    case SkeletonStates.Move:
                        skeleton1.DrawState(skeleton1State, _spriteBatch, _game1Variable.CamX);
                        break;
                    case SkeletonStates.Idle:
                        skeleton1.DrawState(skeleton1State, _spriteBatch, _game1Variable.CamX);
                        break;
                    case SkeletonStates.Aggro:
                        skeleton1.DrawState(skeleton1State, _spriteBatch, _game1Variable.CamX);
                        break;
                }
            }
            if (!skeleton2.Dead)
            {
                switch (skeleton2State)
                {
                    case SkeletonStates.Move:
                        skeleton2.DrawState(skeleton2State, _spriteBatch, _game1Variable.CamX);
                        break;
                    case SkeletonStates.Idle:
                        skeleton2.DrawState(skeleton2State, _spriteBatch, _game1Variable.CamX);
                        break;
                    case SkeletonStates.Aggro:
                        skeleton2.DrawState(skeleton2State, _spriteBatch, _game1Variable.CamX);
                        break;
                }
            }
            if (!skeleton3.Dead)
            {
                switch (skeleton3State)
                {
                    case SkeletonStates.Move:
                        skeleton3.DrawState(skeleton3State, _spriteBatch, _game1Variable.CamX);
                        break;
                    case SkeletonStates.Idle:
                        skeleton3.DrawState(skeleton3State, _spriteBatch, _game1Variable.CamX);
                        break;
                    case SkeletonStates.Aggro:
                        skeleton3.DrawState(skeleton3State, _spriteBatch, _game1Variable.CamX);
                        break;
                }
            }
            if (!skeleton4.Dead)
            {
                switch (skeleton4State)
                {
                    case SkeletonStates.Move:
                        skeleton4.DrawState(skeleton4State, _spriteBatch, _game1Variable.CamX);
                        break;
                    case SkeletonStates.Idle:
                        skeleton4.DrawState(skeleton4State, _spriteBatch, _game1Variable.CamX);
                        break;
                    case SkeletonStates.Aggro:
                        skeleton4.DrawState(skeleton4State, _spriteBatch, _game1Variable.CamX);
                        break;
                }
            }



            // Draw Player
            DrawPlayer(player);
            //dialoger.Draw(_spriteBatch, _game1Variable.CamX, Color.Green);

            // Check for Dialog
            if (player.Collides(dialoger) && (!dialog.Show))
            {
                Button pressF = new Button(pixel, arial16, new Rectangle(dialoger.X - _game1Variable.CamX - 75, dialoger.Y - 35, 150 + dialoger.Position.Width, 30));
                pressF.Draw(_spriteBatch, "Press F to Interact", pressF.ButtonShape, Alignment.Center, Color.Transparent, Color.White);
            }

            if (dialog.Show)
            {
                dialog.Draw(_spriteBatch, Color.White);
                dialog.BoxButton.Draw(_spriteBatch, "Ok", dialog.BoxButton.ButtonShape, Alignment.Center, Color.White, Color.Black);
                if (dialog.BoxButton.MouseInBounds(ms))
                {
                    dialog.BoxButton.Draw(_spriteBatch, "Ok", dialog.BoxButton.ButtonShape, Alignment.Center, Color.Red, Color.Black);
                }
            }


            // Draw Boss Health Bar
            switch (bossLevel)
            {
                case BossLevel.Executioner:
                    if (!executioner.Dead && (CalcDistance(player, executioner) < (graphicsEnemy.PreferredBackBufferWidth * 1.2)))
                    {
                        // Draw the Boss
                        executioner.DrawState(executionerState, _spriteBatch, pixel, _game1Variable.CamX);

                        // Draw Boss Health
                        if (bossHealth.CurrentValue >= (bossHealth.MaximumValue / 2))
                        {
                            _spriteBatch.Draw(pixel, new Rectangle(bossHealth.X - 7, bossHealth.Y - 7, bossHealth.Position.Width + 14, bossHealth.Position.Height + 14), Color.Gray);
                            bossHealth.Draw(_spriteBatch, "The Executioner", bossHealth.Position, Alignment.Center, Color.Black, Color.Red, Color.Black);
                        }
                        else
                        {
                            _spriteBatch.Draw(pixel, new Rectangle(bossHealth.X - 7, bossHealth.Y - 7, bossHealth.Position.Width + 14, bossHealth.Position.Height + 14), Color.Gray);
                            bossHealth.Draw(_spriteBatch, "The Executioner", bossHealth.Position, Alignment.Center, Color.Black, Color.Red, Color.White);
                        }

                        // Draw the rocks
                        foreach (Projectile p in rocks)
                        {
                            _spriteBatch.Draw(rockSprite, new Rectangle(p.Position.X - _game1Variable.CamX, p.Position.Y, p.Position.Width, p.Position.Height), Color.White);
                        }
                    }
                    break;

                case BossLevel.Statue:
                    if (!(statuesDead >= 3))
                    {
                        // For each of the statues, draw them in their state
                        // as long as they are not dead
                        foreach (Statue s in hivemind.Statues)
                        {
                            if (!s.Dead)
                            {
                                for(int i = 0; i < statueState.Count; i++)
                                {
                                    s.DrawState(statueState[i], _spriteBatch, pixel, _game1Variable.CamX);
                                }
                            }
                        }

                        _spriteBatch.DrawString(arial16, "Statue 1 Action: " + hivemind.Statues[0].ActionState.ToString(), new Vector2(800, 100), Color.Black);
                        _spriteBatch.DrawString(arial16, "Statue 2 Action: " + hivemind.Statues[1].ActionState.ToString(), new Vector2(800, 120), Color.Black);
                        _spriteBatch.DrawString(arial16, "Statue 3 Action: " + hivemind.Statues[2].ActionState.ToString(), new Vector2(800, 140), Color.Black);

                        // Draw healthbar outline
                        _spriteBatch.Draw(pixel, new Rectangle(bossHealth.X - 7, bossHealth.Y - 7, bossHealth.Position.Width + 14, bossHealth.Position.Height + 14), Color.Gray);
                        if (statuesHealth.CurrentValue >= (statuesHealth.MaximumValue / 2))
                        {
                            statuesHealth.Draw(_spriteBatch, "The Sister Statues", bossHealth.Position, Alignment.Center, Color.Black, Color.Red, Color.Black);
                        }
                        else
                        {
                            statuesHealth.Draw(_spriteBatch, "The Sister Statues", bossHealth.Position, Alignment.Center, Color.Black, Color.Red, Color.White);
                        }

                        // Draw the rock projectiles
                        foreach (Projectile p in statueRocks)
                        {
                            _spriteBatch.Draw(statueRockTexture, p.Position, Color.White);
                        }
                    }
                    break;
            }

            // draw spell stuff
            player.DrawSpell(_spriteBatch, _game1Variable.CamX);

            // Draw fades for possible transitions
            if (player.CurrentHealth <= 0)
            {
                DrawFade();
            }

            if (executioner.Dead)
            {
                DrawFade();
            }

            // Draw if God Mode is activated
            if (player.GodMode)
            {
                _spriteBatch.DrawString(arial16, "GOD MODE", new Vector2(1150, 10), Color.White);
            }


            // Draw Tutorial Messages
            _spriteBatch.DrawString(arial16, "A - Move Left", new Vector2(80 - _game1Variable.CamX, 250), Color.Black);
            _spriteBatch.DrawString(arial16, "D - Move Right", new Vector2(80 - _game1Variable.CamX, 270), Color.Black);
            _spriteBatch.DrawString(arial16, "SPACE - Jump", new Vector2(80 - _game1Variable.CamX, 290), Color.Black);

            _spriteBatch.DrawString(arial16, "G - Toggle God Mode", new Vector2(380 - _game1Variable.CamX, 140), Color.Black);

            _spriteBatch.DrawString(arial16, "LEFT CLICK - Basic Attack", new Vector2(1000 - _game1Variable.CamX, 150), Color.Black);
            _spriteBatch.DrawString(arial16, "Deals damage and generates mana.", new Vector2(1000 - _game1Variable.CamX, 170), Color.Black);

            _spriteBatch.DrawString(arial16, "RIGHT CLICK - Deflect", new Vector2(2150 - _game1Variable.CamX, 250), Color.Black);
            _spriteBatch.DrawString(arial16, "Deflects an incoming projectile", new Vector2(2150 - _game1Variable.CamX, 270), Color.Black);
            _spriteBatch.DrawString(arial16, "towards your cursor.", new Vector2(2150 - _game1Variable.CamX, 290), Color.Black);

            _spriteBatch.DrawString(arial16, "LEFT SHIFT - Dash", new Vector2(2600 - _game1Variable.CamX, 100), Color.Black);
            _spriteBatch.DrawString(arial16, "Quickly move in the direction of your cursor,", new Vector2(2600 - _game1Variable.CamX, 120), Color.Black);
            _spriteBatch.DrawString(arial16, "damaging enemies you pass.", new Vector2(2600 - _game1Variable.CamX, 140), Color.Black);
        }

        /// <summary>
        /// Draw the Win Screen
        /// </summary>
        void DrawWin()
        {
            if (winToMenu.MouseInBounds(ms))
            {
                winToMenu.Draw(_spriteBatch, "Main Menu", winToMenu.Position, Alignment.Center, Color.Red, Color.Black);
            }
            else winToMenu.Draw(_spriteBatch, "Main Menu", winToMenu.Position, Alignment.Center, Color.White, Color.Black);
        }

        /// <summary>
        /// Draw the Loss Screen
        /// </summary>
        void DrawLoss()
        {
            if (lossToMenu.MouseInBounds(ms))
            {
                lossToMenu.Draw(_spriteBatch, "Main Menu", lossToMenu.Position, Alignment.Center, Color.Red, Color.Black);
            } else lossToMenu.Draw(_spriteBatch, "Main Menu", lossToMenu.Position, Alignment.Center, Color.White, Color.Black);

            if (lossToCheckpoint.MouseInBounds(ms))
            {
                lossToCheckpoint.Draw(_spriteBatch, "Last Checkpoint", lossToCheckpoint.Position, Alignment.Center, Color.Red, Color.Black);
            } else lossToCheckpoint.Draw(_spriteBatch, "Last Checkpoint", lossToCheckpoint.Position, Alignment.Center, Color.White, Color.Black);

        }

        /// <summary>
        /// Draws the player states to the screen
        /// </summary>
        /// <param name="player">The player 
        /// object to draw</param>
        void DrawPlayer(Player player)
        {
            switch (player.State)
            {
                case PlayerState.StandRight:
                    player.Draw(_spriteBatch, SpriteEffects.None, _game1Variable.CamX);
                    break;
                case PlayerState.StandLeft:
                    player.Draw(_spriteBatch, SpriteEffects.FlipHorizontally, _game1Variable.CamX);
                    break;
                case PlayerState.MoveRight:
                    player.Draw(_spriteBatch, SpriteEffects.None, _game1Variable.CamX);
                    break;
                case PlayerState.MoveLeft:
                    player.Draw(_spriteBatch, SpriteEffects.FlipHorizontally, _game1Variable.CamX);
                    break;
            }
        }

        public void DrawShapes()
        {
            // Call the player's method to draw shapes with spritebatch
            player.DrawPlayerShapes(_game1Variable.CamX);

            executioner.DrawShapes(_game1Variable.CamX);
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        void Reset()
        {
            // Reset loss timer
            fadeTimer = 0;

            // Reset player
            player.Reset();

            // Reset executioner
            executioner.Reset();

            // Reset statues
            foreach(Statue s in hivemind.Statues)
            {
                s.Reset();
            }
            statuesDead = 0;

            // Reset enemies
            foreach(Enemy e in enemyList)
            {
                e.Reset();
            }

            // Reset statusbars
            healthBar.Update(player.CurrentHealth);
            manaBar.Update(player.CurrentMana);

            // Reset cam
            _game1Variable.CamX = 0;
            _game1Variable.CamY = 0;
        }*/
    }        
}
