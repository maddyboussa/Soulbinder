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
    public class Game1 : Game
    {
        // FIELDS =======================================================================
        // Managers
        private GameManager gameManager;
        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch spriteBatch;
        private SpriteManager spriteManager;
        private TileManager tileManager;
        private UIManager uiManager;

        // Important Variables
        private Level currentLevel;
        private Menu currentMenu;
        private Player player;
        private int camera;

        // Miscellaneous Variables
        private Vector2 gravity;
        private double elapsedMilliseconds;
        private double elapsedSeconds;

        // Input States
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;
        private MouseState mouseState;
        private MouseState previousMouseState;

        // Menus
        private MainMenu mainMenu;
        private DeathMenu deathMenu;
        private WinMenu winMenu;
        private PauseMenu pauseMenu;

        // Levels
        // Dungeon Levels
        private DungeonLevel1 l_Dungeon1;
        private DungeonLevel2 l_Dungeon2;
        private DungeonLevel3 l_Dungeon3;
        private DungeonLevel4 l_Dungeon4;
        private DungeonLevel5 l_Dungeon5;
        private DungeonLevel6Top l_Dungeon6Top;
        private DungeonLevel6Bottom l_Dungeon6Bottom;
        private DungeonLevel7Top l_Dungeon7Top;
        private DungeonLevel7Bottom l_Dungeon7Bottom;
        private DungeonLevel8 l_Dungeon8;
        private DungeonLevel9 l_Dungeon9;
        private DungeonLevelBoss l_DungeonBoss;
        // Castle Levels
        private CastleLevel1 l_Castle1;
        private CastleLevel2 l_Castle2;
        private CastleLevel3 l_Castle3;
        private CastleLevel4Top l_Castle4Top;
        private CastleLevel4Bottom l_Castle4Bottom;
        private CastleLevel5Top l_Castle5Top;
        private CastleLevel5Bottom l_Castle5Bottom;
        private CastleLevel6Top l_Castle6Top;
        private CastleLevel6Bottom l_Castle6Bottom;
        private CastleLevel7 l_Castle7;
        private CastleLevel8 l_Castle8;
        private CastleLevelBoss l_CastleBoss;
        // Keep Levels
        private KeepLevel1 l_Keep1;
        private KeepLevel2 l_Keep2;
        private KeepLevel3 l_Keep3;
        private KeepLevel4Top l_Keep4Top;
        private KeepLevel4Bottom l_Keep4Bottom;
        private KeepLevel5Top l_Keep5Top;
        private KeepLevel5Bottom l_Keep5Bottom;
        private KeepLevel6 l_Keep6;
        private KeepLevel7 l_Keep7;
        private KeepLevel8 l_Keep8;
        private KeepLevel9 l_Keep9;
        private KeepLevelBoss l_KeepBoss;

        public GameManager GameManager { get => gameManager; set => gameManager = value; }
        public GraphicsDeviceManager GraphicsManager { get => graphicsManager; set => graphicsManager = value; }
        public SpriteBatch SpriteBatch { get => spriteBatch; set => spriteBatch = value; }
        public SpriteManager SpriteManager { get => spriteManager; set => spriteManager = value; }
        public TileManager TileManager { get => tileManager; set => tileManager = value; }
        public UIManager UIManager { get => uiManager; set => uiManager = value; }
        public Level CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public Menu CurrentMenu { get => currentMenu; set => currentMenu = value; }
        public Player Player { get => player; set => player = value; }
        public int Camera { get => camera; set => camera = value; }
        public Vector2 Gravity { get => gravity; set => gravity = value; }
        public double ElapsedMilliseconds { get => elapsedMilliseconds; set => elapsedMilliseconds = value; }
        public double ElapsedSeconds { get => elapsedSeconds; set => elapsedSeconds = value; }
        public KeyboardState KeyboardState { get => keyboardState; set => keyboardState = value; }
        public KeyboardState PreviousKeyboardState { get => previousKeyboardState; set => previousKeyboardState = value; }
        public MouseState MouseState { get => mouseState; set => mouseState = value; }
        public MouseState PreviousMouseState { get => previousMouseState; set => previousMouseState = value; }
        internal MainMenu MainMenu { get => mainMenu; set => mainMenu = value; }
        internal DungeonLevel1 L_Dungeon1 { get => l_Dungeon1; set => l_Dungeon1 = value; }
        internal DungeonLevel2 L_Dungeon2 { get => l_Dungeon2; set => l_Dungeon2 = value; }
        internal DungeonLevel3 L_Dungeon3 { get => l_Dungeon3; set => l_Dungeon3 = value; }
        internal DungeonLevel4 L_Dungeon4 { get => l_Dungeon4; set => l_Dungeon4 = value; }
        internal DungeonLevel5 L_Dungeon5 { get => l_Dungeon5; set => l_Dungeon5 = value; }
        internal DungeonLevel6Top L_Dungeon6Top { get => l_Dungeon6Top; set => l_Dungeon6Top = value; }
        internal DungeonLevel6Bottom L_Dungeon6Bottom { get => l_Dungeon6Bottom; set => l_Dungeon6Bottom = value; }
        internal DungeonLevel7Top L_Dungeon7Top { get => l_Dungeon7Top; set => l_Dungeon7Top = value; }
        internal DungeonLevel7Bottom L_Dungeon7Bottom { get => l_Dungeon7Bottom; set => l_Dungeon7Bottom = value; }
        internal DungeonLevel8 L_Dungeon8 { get => l_Dungeon8; set => l_Dungeon8 = value; }
        internal DungeonLevel9 L_Dungeon9 { get => l_Dungeon9; set => l_Dungeon9 = value; }
        internal DungeonLevelBoss L_DungeonBoss { get => l_DungeonBoss; set => l_DungeonBoss = value; }
        internal CastleLevel1 L_Castle1 { get => l_Castle1; set => l_Castle1 = value; }
        internal CastleLevel2 L_Castle2 { get => l_Castle2; set => l_Castle2 = value; }
        internal CastleLevel3 L_Castle3 { get => l_Castle3; set => l_Castle3 = value; }
        internal CastleLevel4Top L_Castle4Top { get => l_Castle4Top; set => l_Castle4Top = value; }
        internal CastleLevel4Bottom L_Castle4Bottom { get => l_Castle4Bottom; set => l_Castle4Bottom = value; }
        internal CastleLevel5Top L_Castle5Top { get => l_Castle5Top; set => l_Castle5Top = value; }
        internal CastleLevel5Bottom L_Castle5Bottom { get => l_Castle5Bottom; set => l_Castle5Bottom = value; }
        internal CastleLevel6Top L_Castle6Top { get => l_Castle6Top; set => l_Castle6Top = value; }
        internal CastleLevel6Bottom L_Castle6Bottom { get => l_Castle6Bottom; set => l_Castle6Bottom = value; }
        internal CastleLevel7 L_Castle7 { get => l_Castle7; set => l_Castle7 = value; }
        internal CastleLevel8 L_Castle8 { get => l_Castle8; set => l_Castle8 = value; }
        internal CastleLevelBoss L_CastleBoss { get => l_CastleBoss; set => l_CastleBoss = value; }
        internal KeepLevel1 L_Keep1 { get => l_Keep1; set => l_Keep1 = value; }
        internal KeepLevel2 L_Keep2 { get => l_Keep2; set => l_Keep2 = value; }
        internal KeepLevel3 L_Keep3 { get => l_Keep3; set => l_Keep3 = value; }
        internal KeepLevel4Bottom L_Keep4Bottom { get => l_Keep4Bottom; set => l_Keep4Bottom = value; }
        internal KeepLevel5Bottom L_Keep5Bottom { get => l_Keep5Bottom; set => l_Keep5Bottom = value; }
        internal KeepLevel4Top L_Keep4Top { get => l_Keep4Top; set => l_Keep4Top = value; }
        internal KeepLevel5Top L_Keep5Top { get => l_Keep5Top; set => l_Keep5Top = value; }
        internal KeepLevel6 L_Keep6 { get => l_Keep6; set => l_Keep6 = value; }
        internal KeepLevel7 L_Keep7 { get => l_Keep7; set => l_Keep7 = value; }
        internal KeepLevel8 L_Keep8 { get => l_Keep8; set => l_Keep8 = value; }
        internal KeepLevel9 L_Keep9 { get => l_Keep9; set => l_Keep9 = value; }
        internal KeepLevelBoss L_KeepBoss { get => l_KeepBoss; set => l_KeepBoss = value; }
        internal DeathMenu DeathMenu { get => deathMenu; set => deathMenu = value; }
        internal WinMenu WinMenu { get => winMenu; set => winMenu = value; }
        internal PauseMenu PauseMenu { get => pauseMenu; set => pauseMenu = value; }



        // PROPERTIES ===================================================================




        // CONSTRUCTOR ==================================================================
        public Game1()
        {
            GraphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }



        // METHODS ======================================================================
        protected override void Initialize()
        {
            // Set Window
            GraphicsManager.PreferredBackBufferWidth = 1280;
            GraphicsManager.PreferredBackBufferHeight = 720;
            GraphicsManager.ApplyChanges();

            // Set Camera
            Camera = 0;

            // Set Miscellaneous Variables
            Gravity = new Vector2(0, 0.5f);
            ElapsedMilliseconds = 0;

            // Initialize Managers
            GameManager = new GameManager(this);
            SpriteManager = new SpriteManager(this);
            TileManager = new TileManager(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Sprites
            SpriteManager.LoadContent();

            // Initialize Player
            Player = new Player(
                SpriteManager.PlayerSprite,
                new Rectangle(145, 350, 32, 64), 5, 20, 20, 10, 10,
                SpriteManager.BasicAttackSprite,
                SpriteManager.FireDashSprite,
                SpriteManager.DeflectSprite,
                SpriteManager.ProjectileSpellSprite,
                SpriteManager);

            // Initizalize Menus & UI
            UIManager = new UIManager(this);

            MainMenu = new MainMenu(this);
            DeathMenu = new DeathMenu(this);
            WinMenu = new WinMenu(this);
            PauseMenu = new PauseMenu(this);
            CurrentMenu = MainMenu;

            // Load Levels
            L_Dungeon1 = new DungeonLevel1(
                this, "Content/DL1.tit");
            L_Dungeon2 = new DungeonLevel2(
                this, "Content/DL2.tit");
            L_Dungeon3 = new DungeonLevel3(
                this, "Content/DL3.tit");
            L_Dungeon4 = new DungeonLevel4(
                this, "Content/DL4.tit");
            L_Dungeon5 = new DungeonLevel5(
                this, "Content/DL5.tit");
            L_Dungeon6Top = new DungeonLevel6Top(
                this, "Content/DL6Top.tit");
            L_Dungeon6Bottom = new DungeonLevel6Bottom(
                this, "Content/DL6Bottom.tit");
            L_Dungeon7Top = new DungeonLevel7Top(
                this, "Content/DL7Top.tit");
            L_Dungeon7Bottom = new DungeonLevel7Bottom(
                this, "Content/DL7Bottom.tit");
            L_Dungeon8 = new DungeonLevel8(
                this, "Content/DL8.tit");
            L_Dungeon9 = new DungeonLevel9(
                this, "Content/DL9.tit");
            L_DungeonBoss = new DungeonLevelBoss(
                this, "Content/DLBoss.tit");
            L_Castle1 = new CastleLevel1(
                this, "Content/CL1.tit");
            L_Castle2 = new CastleLevel2(
                this, "Content/CL2.tit");
            L_Castle3 = new CastleLevel3(
                this, "Content/CL3.tit");
            L_Castle4Top = new CastleLevel4Top(
                this, "Content/CL4Top.tit");
            L_Castle4Bottom = new CastleLevel4Bottom(
                this, "Content/CL4Bottom.tit");
            L_Castle5Top = new CastleLevel5Top(
                this, "Content/CL5Top.tit");
            L_Castle5Bottom = new CastleLevel5Bottom(
                this, "Content/CL5Bottom.tit");
            L_Castle6Top = new CastleLevel6Top(
                this, "Content/CL6Top.tit");
            L_Castle6Bottom = new CastleLevel6Bottom(
                this, "Content/CL6Bottom.tit");
            L_Castle7 = new CastleLevel7(
                this, "Content/CL7.tit");
            L_Castle8 = new CastleLevel8(
                this, "Content/CL8.tit");
            L_CastleBoss = new CastleLevelBoss(
                this, "Content/CLBoss.tit");
            L_Keep1 = new KeepLevel1(
                this, "Content/KL1.tit");
            L_Keep2 = new KeepLevel2(
                this, "Content/KL2.tit");
            L_Keep3 = new KeepLevel3(
                this, "Content/KL3.tit");
            L_Keep4Top = new KeepLevel4Top(
                this, "Content/KL4Top.tit");
            L_Keep4Bottom = new KeepLevel4Bottom(
                this, "Content/KL4Bottom.tit");
            L_Keep5Top = new KeepLevel5Top(
                this, "Content/KL5Top.tit");
            L_Keep5Bottom = new KeepLevel5Bottom(
                this, "Content/KL5Bottom.tit");
            L_Keep6 = new KeepLevel6(
                this, "Content/KL6.tit");
            L_Keep7 = new KeepLevel7(
                this, "Content/KL7.tit");
            L_Keep8 = new KeepLevel8(
                this, "Content/KL8.tit");
            L_Keep9 = new KeepLevel9(
                this, "Content/KL9.tit");
            L_KeepBoss = new KeepLevelBoss(
                this, "Content/KLBoss.tit");



            // Connect the doors
            L_Dungeon1.ConnectDoors(this);
            L_Dungeon2.ConnectDoors(this);
            L_Dungeon3.ConnectDoors(this);
            L_Dungeon4.ConnectDoors(this);
            L_Dungeon5.ConnectDoors(this);
            L_Dungeon6Top.ConnectDoors(this);
            L_Dungeon6Bottom.ConnectDoors(this);
            L_Dungeon7Top.ConnectDoors(this);
            L_Dungeon7Bottom.ConnectDoors(this);
            L_Dungeon8.ConnectDoors(this);
            L_Dungeon9.ConnectDoors(this);
            L_DungeonBoss.ConnectDoors(this);
            L_Castle1.ConnectDoors(this);
            L_Castle2.ConnectDoors(this);
            L_Castle3.ConnectDoors(this);
            L_Castle4Top.ConnectDoors(this);
            L_Castle4Bottom.ConnectDoors(this);
            L_Castle5Top.ConnectDoors(this);
            L_Castle5Bottom.ConnectDoors(this);
            L_Castle6Top.ConnectDoors(this);
            L_Castle6Bottom.ConnectDoors(this);
            L_Castle7.ConnectDoors(this);
            L_Castle8.ConnectDoors(this);
            L_CastleBoss.ConnectDoors(this);
            L_Keep1.ConnectDoors(this);
            L_Keep2.ConnectDoors(this);
            L_Keep3.ConnectDoors(this);
            L_Keep4Top.ConnectDoors(this);
            L_Keep4Bottom.ConnectDoors(this);
            L_Keep5Top.ConnectDoors(this);
            L_Keep5Bottom.ConnectDoors(this);
            L_Keep6.ConnectDoors(this);
            L_Keep7.ConnectDoors(this);
            L_Keep8.ConnectDoors(this);
            L_Keep9.ConnectDoors(this);
            L_KeepBoss.ConnectDoors(this);


            // Create Unlockables
            L_Dungeon7Bottom.CreateUnlockable(this);
            L_Castle6Bottom.CreateUnlockable(this);
            L_Keep5Bottom.CreateUnlockable(this);

            // Create Rest Sites
            L_Dungeon9.CreateRestSite(this);
            l_Castle1.CreateRestSite(this);
            L_Castle4Bottom.CreateRestSite(this);
            L_Castle8.CreateRestSite(this);
            L_Keep1.CreateRestSite(this);
            L_Keep4Top.CreateRestSite(this);
            L_Keep6.CreateRestSite(this);
            L_Keep9.CreateRestSite(this);

            // Set Starting Level & Menu
            currentMenu = mainMenu;
            currentLevel = L_Dungeon1;

            // Set player to checkpoint position of current level
            Player.Position = new Rectangle((int)CurrentLevel.CheckpointPosition.X, (int)CurrentLevel.CheckpointPosition.Y,
                Player.Position.Width, Player.Position.Height);
        }


        protected override void Update(GameTime gameTime)
        {
            // Get KB+M States
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();

            // Update Timing Variables
            ElapsedMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;
            ElapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;

            // FSM Based on GameState
            switch (GameManager.GameState)
            {
                case GameState.Menu:
                    if (gameManager.SingleKeyPress(Keys.Escape) && CurrentMenu != PauseMenu)
                    {
                        Exit();
                    }
                    else if (gameManager.SingleKeyPress(Keys.Escape) && CurrentMenu == PauseMenu)
                    {
                        GameManager.GameState = GameState.Game;
                    }
                    GameManager.Update(CurrentMenu);
                    break;

                case GameState.Game:
                    if (gameManager.SingleKeyPress(Keys.Escape) || !this.IsActive)
                    {
                        CurrentMenu = PauseMenu;
                        GameManager.GameState = GameState.Menu;
                    }
                    GameManager.Update(CurrentLevel);
                    break;
            }

            // Save KB+M States
            PreviousKeyboardState = KeyboardState;
            PreviousMouseState = MouseState;
        }

        protected override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            ShapeBatch.Begin(GraphicsDevice);

            GraphicsDevice.Clear(Color.DarkGray);

            switch (GameManager.GameState)
            {
                case GameState.Menu:
                    IsMouseVisible = true;
                    GameManager.Draw(CurrentMenu);
                    break;

                case GameState.Game:
                    IsMouseVisible = false;
                    GameManager.Draw(CurrentLevel);
                    break;
            }

            SpriteBatch.End();
            ShapeBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Resets the Player and Levels to initial states.
        /// </summary>
        public void ResetGame()
        {
            // Create new Player
            Player = new Player(
                SpriteManager.PlayerSprite,
                new Rectangle(145, 350, 32, 64), 5, 20, 20, 10, 10,
                SpriteManager.BasicAttackSprite,
                SpriteManager.FireDashSprite,
                SpriteManager.DeflectSprite,
                SpriteManager.ProjectileSpellSprite,
                SpriteManager);

            // Load Levels
            L_Dungeon1 = new DungeonLevel1(
                this, "Content/DL1.tit");
            L_Dungeon2 = new DungeonLevel2(
                this, "Content/DL2.tit");
            L_Dungeon3 = new DungeonLevel3(
                this, "Content/DL3.tit");
            L_Dungeon4 = new DungeonLevel4(
                this, "Content/DL4.tit");
            L_Dungeon5 = new DungeonLevel5(
                this, "Content/DL5.tit");
            L_Dungeon6Top = new DungeonLevel6Top(
                this, "Content/DL6Top.tit");
            L_Dungeon6Bottom = new DungeonLevel6Bottom(
                this, "Content/DL6Bottom.tit");
            L_Dungeon7Top = new DungeonLevel7Top(
                this, "Content/DL7Top.tit");
            L_Dungeon7Bottom = new DungeonLevel7Bottom(
                this, "Content/DL7Bottom.tit");
            L_Dungeon8 = new DungeonLevel8(
                this, "Content/DL8.tit");
            L_Dungeon9 = new DungeonLevel9(
                this, "Content/DL9.tit");
            L_DungeonBoss = new DungeonLevelBoss(
                this, "Content/DLBoss.tit");
            L_Castle1 = new CastleLevel1(
                this, "Content/CL1.tit");
            L_Castle2 = new CastleLevel2(
                this, "Content/CL2.tit");
            L_Castle3 = new CastleLevel3(
                this, "Content/CL3.tit");
            L_Castle4Top = new CastleLevel4Top(
                this, "Content/CL4Top.tit");
            L_Castle4Bottom = new CastleLevel4Bottom(
                this, "Content/CL4Bottom.tit");
            L_Castle5Top = new CastleLevel5Top(
                this, "Content/CL5Top.tit");
            L_Castle5Bottom = new CastleLevel5Bottom(
                this, "Content/CL5Bottom.tit");
            L_Castle6Top = new CastleLevel6Top(
                this, "Content/CL6Top.tit");
            L_Castle6Bottom = new CastleLevel6Bottom(
                this, "Content/CL6Bottom.tit");
            L_Castle7 = new CastleLevel7(
                this, "Content/CL7.tit");
            L_Castle8 = new CastleLevel8(
                this, "Content/CL8.tit");
            L_CastleBoss = new CastleLevelBoss(
                this, "Content/CLBoss.tit");
            L_Keep1 = new KeepLevel1(
                this, "Content/KL1.tit");
            L_Keep2 = new KeepLevel2(
                this, "Content/KL2.tit");
            L_Keep3 = new KeepLevel3(
                this, "Content/KL3.tit");
            L_Keep4Top = new KeepLevel4Top(
                this, "Content/KL4Top.tit");
            L_Keep4Bottom = new KeepLevel4Bottom(
                this, "Content/KL4Bottom.tit");
            L_Keep5Top = new KeepLevel5Top(
                this, "Content/KL5Top.tit");
            L_Keep5Bottom = new KeepLevel5Bottom(
                this, "Content/KL5Bottom.tit");
            L_Keep6 = new KeepLevel6(
                this, "Content/KL6.tit");
            L_Keep7 = new KeepLevel7(
                this, "Content/KL7.tit");
            L_Keep8 = new KeepLevel8(
                this, "Content/KL8.tit");
            L_Keep9 = new KeepLevel9(
                this, "Content/KL9.tit");
            L_KeepBoss = new KeepLevelBoss(
                this, "Content/KLBoss.tit");



            // Connect the doors
            L_Dungeon1.ConnectDoors(this);
            L_Dungeon2.ConnectDoors(this);
            L_Dungeon3.ConnectDoors(this);
            L_Dungeon4.ConnectDoors(this);
            L_Dungeon5.ConnectDoors(this);
            L_Dungeon6Top.ConnectDoors(this);
            L_Dungeon6Bottom.ConnectDoors(this);
            L_Dungeon7Top.ConnectDoors(this);
            L_Dungeon7Bottom.ConnectDoors(this);
            L_Dungeon8.ConnectDoors(this);
            L_Dungeon9.ConnectDoors(this);
            L_DungeonBoss.ConnectDoors(this);
            L_Castle1.ConnectDoors(this);
            L_Castle2.ConnectDoors(this);
            L_Castle3.ConnectDoors(this);
            L_Castle4Top.ConnectDoors(this);
            L_Castle4Bottom.ConnectDoors(this);
            L_Castle5Top.ConnectDoors(this);
            L_Castle5Bottom.ConnectDoors(this);
            L_Castle6Top.ConnectDoors(this);
            L_Castle6Bottom.ConnectDoors(this);
            L_Castle7.ConnectDoors(this);
            L_Castle8.ConnectDoors(this);
            L_CastleBoss.ConnectDoors(this);
            L_Keep1.ConnectDoors(this);
            L_Keep2.ConnectDoors(this);
            L_Keep3.ConnectDoors(this);
            L_Keep4Top.ConnectDoors(this);
            L_Keep4Bottom.ConnectDoors(this);
            L_Keep5Top.ConnectDoors(this);
            L_Keep5Bottom.ConnectDoors(this);
            L_Keep6.ConnectDoors(this);
            L_Keep7.ConnectDoors(this);
            L_Keep8.ConnectDoors(this);
            L_Keep9.ConnectDoors(this);
            L_KeepBoss.ConnectDoors(this);


            // Create Unlockables
            L_Dungeon7Bottom.CreateUnlockable(this);
            L_Castle6Bottom.CreateUnlockable(this);
            L_Keep5Bottom.CreateUnlockable(this);

            // Create Rest Sites
            L_Dungeon9.CreateRestSite(this);
            l_Castle1.CreateRestSite(this);
            L_Castle4Bottom.CreateRestSite(this);
            L_Castle8.CreateRestSite(this);
            L_Keep1.CreateRestSite(this);
            L_Keep4Top.CreateRestSite(this);
            L_Keep6.CreateRestSite(this);
            L_Keep9.CreateRestSite(this);

            // Reset to level 1
            CurrentLevel = L_Dungeon1;

            // Reset Speedrun Timer
            UIManager.TimerValue = 0;
            UIManager.TimerActive = true;

            // Set player to checkpoint position of current level
            Player.Position = new Rectangle((int)L_Dungeon1.CheckpointPosition.X, (int)L_Dungeon1.CheckpointPosition.Y,
                Player.Position.Width, Player.Position.Height);
        }
    }
}