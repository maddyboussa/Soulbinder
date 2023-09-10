using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Soulbinder.GameObjects;

namespace Soulbinder
{
    public class SpriteManager
    {
        // FIELDS =======================================================================
        // Vital Connections
        private Game1 game;

        // Fonts
        private SpriteFont arial16;

        // Player Sprites
        private Texture2D playerSprite;
        private Texture2D basicAttackSprite;
        private Texture2D fireDashSprite;
        private Texture2D deflectSprite;
        private Texture2D projectileSpellSprite;

        // Executioner Sprites
        private Texture2D executionerSprite;
        private Texture2D executionerHorizontalHigh;
        private Texture2D executionerHorizontalMid;
        private Texture2D executionerHorizontalLow;
        private Texture2D executionerVertical;
        private Texture2D executionerSlam;
        private List<Texture2D> executionerAttackAnims;

        // Statue Sprites
        private Texture2D statueSprite;
        private Texture2D statueStab;
        private Texture2D statueDash;
        private Texture2D statueThrow;
        private Texture2D statueDodge;
        private Texture2D statueRockTexture;
        private List<Texture2D> statueAttackAnims;

        // Lich King Sprites
        private Texture2D lichKingSprite;
        private Texture2D lichKingHandRightOpen;
        private Texture2D lichKingHandRightFist;
        private Texture2D lichKingHandLeftOpen;
        private Texture2D lichKingHandLeftFist;
        private Texture2D lichKingProjectile;

        // Tile Sprites
        private Texture2D darkBrick;
        private Texture2D darkBrickFloor;
        private Texture2D darkGray;
        private Texture2D darkGrayFloor;
        private Texture2D lightBrick;
        private Texture2D lightBrickFloor;
        private Texture2D lightGray;
        private Texture2D lightGrayFloor;
        private Texture2D missingTexture;

        // Background Sprites
        private Texture2D titleScreen;
        private Texture2D titleScreenNoText;
        private Texture2D dungeonBackground;
        private Texture2D castleBackground;
        private Texture2D keepBackground;
        private Texture2D litchKingBackground;
        private Texture2D gameOverScreen;

        // Miscellaneous Sprites
        private Texture2D pixel;
        private Texture2D skeletonSprite;
        private Texture2D rockSprite;



        // PROPERTIES ===================================================================
        public SpriteFont Arial16 { get => arial16; set => arial16 = value; }
        public Texture2D PlayerSprite { get => playerSprite; set => playerSprite = value; }
        public Texture2D BasicAttackSprite { get => basicAttackSprite; set => basicAttackSprite = value; }
        public Texture2D FireDashSprite { get => fireDashSprite; set => fireDashSprite = value; }
        public Texture2D DeflectSprite { get => deflectSprite; set => deflectSprite = value; }
        public Texture2D ProjectileSpellSprite { get => projectileSpellSprite; set => projectileSpellSprite = value; }
        public Texture2D ExecutionerSprite { get => executionerSprite; set => executionerSprite = value; }
        public Texture2D ExecutionerHorizontalHigh { get => executionerHorizontalHigh; set => executionerHorizontalHigh = value; }
        public Texture2D ExecutionerHorizontalMid { get => executionerHorizontalMid; set => executionerHorizontalMid = value; }
        public Texture2D ExecutionerHorizontalLow { get => executionerHorizontalLow; set => executionerHorizontalLow = value; }
        public Texture2D ExecutionerVertical { get => executionerVertical; set => executionerVertical = value; }
        public Texture2D ExecutionerSlam { get => executionerSlam; set => executionerSlam = value; }
        public List<Texture2D> ExecutionerAttackAnims { get => executionerAttackAnims; set => executionerAttackAnims = value; }
        public Texture2D StatueSprite { get => statueSprite; set => statueSprite = value; }
        public Texture2D StatueStab { get => statueStab; set => statueStab = value; }
        public Texture2D StatueDash { get => statueDash; set => statueDash = value; }
        public Texture2D StatueThrow { get => statueThrow; set => statueThrow = value; }
        public Texture2D StatueDodge { get => statueDodge; set => statueDodge = value; }
        public Texture2D StatueRockTexture { get => statueRockTexture; set => statueRockTexture = value; }
        public List<Texture2D> StatueAttackAnims { get => statueAttackAnims; set => statueAttackAnims = value; }
        public Texture2D DarkBrick { get => darkBrick; set => darkBrick = value; }
        public Texture2D DarkBrickFloor { get => darkBrickFloor; set => darkBrickFloor = value; }
        public Texture2D DarkGray { get => darkGray; set => darkGray = value; }
        public Texture2D DarkGrayFloor { get => darkGrayFloor; set => darkGrayFloor = value; }
        public Texture2D LightBrick { get => lightBrick; set => lightBrick = value; }
        public Texture2D LightBrickFloor { get => lightBrickFloor; set => lightBrickFloor = value; }
        public Texture2D LightGray { get => lightGray; set => lightGray = value; }
        public Texture2D LightGrayFloor { get => lightGrayFloor; set => lightGrayFloor = value; }
        public Texture2D MissingTexture { get => missingTexture; set => missingTexture = value; }
        public Texture2D TitleScreen { get => titleScreen; set => titleScreen = value; }
        public Texture2D TitleScreenNoText { get => titleScreenNoText; set => titleScreenNoText = value; }
        public Texture2D DungeonBackground { get => dungeonBackground; set => dungeonBackground = value; }
        public Texture2D LichKingSprite { get => lichKingSprite; set => lichKingSprite = value; }
        public Texture2D LichKingHandRightOpen { get => lichKingHandRightOpen; set => lichKingHandRightOpen = value; }
        public Texture2D LichKingHandRightFist { get => lichKingHandRightFist; set => lichKingHandRightFist = value; }
        public Texture2D LichKingHandLeftOpen { get => lichKingHandLeftOpen; set => lichKingHandLeftOpen = value; }
        public Texture2D LichKingHandLeftFist { get => lichKingHandLeftFist; set => lichKingHandLeftFist = value; }
        public Texture2D LichKingProjectile { get => lichKingProjectile; set => lichKingProjectile = value; }
        public Texture2D CastleBackground { get => castleBackground; set => castleBackground = value; }
        public Texture2D KeepBackground { get => keepBackground; set => keepBackground = value; }
        public Texture2D LitchKingBackground { get => litchKingBackground; set => litchKingBackground = value; }
        public Texture2D GameOverScreen { get => gameOverScreen; set => gameOverScreen = value; }
        public Texture2D Pixel { get => pixel; set => pixel = value; }
        public Texture2D SkeletonSprite { get => skeletonSprite; set => skeletonSprite = value; }
        public Texture2D RockSprite { get => rockSprite; set => rockSprite = value; }



        // CONSTRUCTORS =================================================================
        public SpriteManager(Game1 game)
        {
            this.game = game;
        }

        

        // METHODS ======================================================================
        public void LoadContent()
        {
            // Load Fonts
            Arial16 = game.Content.Load<SpriteFont>("Arial16");

            // Load Player Sprites
            PlayerSprite = game.Content.Load<Texture2D>("Mage");
            BasicAttackSprite = game.Content.Load<Texture2D>("BasicAttack");
            FireDashSprite = game.Content.Load<Texture2D>("FireDash");
            DeflectSprite = game.Content.Load<Texture2D>("Deflection");
            ProjectileSpellSprite = game.Content.Load<Texture2D>("BasicAttack");

            // Executioner Sprites
            ExecutionerSprite = game.Content.Load<Texture2D>("ExecutionerIdleV2"); ;
            ExecutionerHorizontalHigh = game.Content.Load<Texture2D>("ExecutionerHorizontalHighV2");
            ExecutionerHorizontalMid = game.Content.Load<Texture2D>("ExecutionerHorizontalMidV2");
            ExecutionerHorizontalLow = game.Content.Load<Texture2D>("ExecutionerHorizontalLowV2");
            ExecutionerVertical = game.Content.Load<Texture2D>("ExecutionerVerticalV2");
            ExecutionerSlam = game.Content.Load<Texture2D>("ExecutionerSlamV2");

            ExecutionerAttackAnims = new List<Texture2D>();
            ExecutionerAttackAnims.Add(ExecutionerHorizontalHigh);
            ExecutionerAttackAnims.Add(ExecutionerHorizontalMid);
            ExecutionerAttackAnims.Add(ExecutionerHorizontalLow);
            ExecutionerAttackAnims.Add(ExecutionerVertical);
            ExecutionerAttackAnims.Add(ExecutionerSlam);

            // Statue Sprites
            StatueSprite = game.Content.Load<Texture2D>("StatueIdle");
            StatueStab = game.Content.Load<Texture2D>("StatueStab");
            StatueDash = game.Content.Load<Texture2D>("StatueDashSlash");
            StatueThrow = game.Content.Load<Texture2D>("StatueThrow");
            StatueDodge = game.Content.Load<Texture2D>("StatueStepBack");
            StatueRockTexture = game.Content.Load<Texture2D>("Rock");

            StatueAttackAnims = new List<Texture2D>();
            StatueAttackAnims.Add(StatueStab);
            StatueAttackAnims.Add(StatueDash);
            StatueAttackAnims.Add(StatueThrow);
            StatueAttackAnims.Add(StatueDodge);

            // Lich King Sprites
            lichKingSprite = game.Content.Load<Texture2D>("LitchHead");
            lichKingHandRightOpen = game.Content.Load<Texture2D>("OpenHandReversed");
            lichKingHandRightFist = game.Content.Load<Texture2D>("FistHandFlipped");
            lichKingHandLeftOpen = game.Content.Load<Texture2D>("OpenHand");
            lichKingHandLeftFist = game.Content.Load<Texture2D>("FistHand");
            lichKingProjectile = game.Content.Load<Texture2D>("LitchShot");

            // Tile Sprites
            DarkBrick = game.Content.Load<Texture2D>("DarkBrick");
            DarkBrickFloor = game.Content.Load<Texture2D>("DarkBrickFloor");
            DarkGray = game.Content.Load<Texture2D>("DarkGray");
            DarkGrayFloor = game.Content.Load<Texture2D>("DarkGrayFloor");
            LightBrick = game.Content.Load<Texture2D>("LightBrick");
            LightBrickFloor = game.Content.Load<Texture2D>("LightBrickFloor");
            LightGray = game.Content.Load<Texture2D>("LightGray");
            LightGrayFloor = game.Content.Load<Texture2D>("LightGrayFloor");
            MissingTexture = game.Content.Load<Texture2D>("MissingTexture");

            // Background Sprites
            TitleScreen = game.Content.Load<Texture2D>("TitleScreen");
            TitleScreenNoText = game.Content.Load<Texture2D>("TitleNoText");
            DungeonBackground = game.Content.Load<Texture2D>("DungeonBG");
            CastleBackground = game.Content.Load<Texture2D>("CastleBG");
            KeepBackground = game.Content.Load<Texture2D>("CastleTopBG");
            LitchKingBackground = game.Content.Load<Texture2D>("LitchBackground");
            GameOverScreen = game.Content.Load<Texture2D>("GameOver");


            // Miscellaneous Sprites
            Pixel = game.Content.Load<Texture2D>("WhitePixel");
            SkeletonSprite = game.Content.Load<Texture2D>("SkeletonV2");
            RockSprite = game.Content.Load<Texture2D>("Rock");
        }
    }
}
