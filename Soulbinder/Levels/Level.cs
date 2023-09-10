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
    public abstract class Level
    {
        // FIELDS =======================================================================
        // Environnment
        private Tile[,] tiles;
        private List<Rectangle> collisions;
        private Texture2D background;

        // Entities & Interactables
        private List<Door> doors; // Need to talk to Vincent about how dialogers work.
        private List<Unlockable> unlockables; // Vincent has been talked to -Vincent
        private List<RestSite> restSites;
        private List<Skeleton> enemies;
        private List<Projectile> projectiles;
        bool dontApplyProjectileGravity;
        private List<Boss> boss;

        // Miscellaneous Data
        private Vector2 checkpointPosition;
        private string name;


        // PROPERTIES ===================================================================
        public Tile[,] Tiles { get => tiles; set => tiles = value; }
        public List<Rectangle> Collisions { get => collisions; set => collisions = value; }
        public List<Skeleton> Enemies { get => enemies; set => enemies = value; }
        public List<Projectile> Projectiles { get => projectiles; set => projectiles = value; }
        public Vector2 CheckpointPosition { get => checkpointPosition; set => checkpointPosition = value; }
        public Texture2D Background { get => background; set => background = value; }
        public List<Door> Doors { get => doors; set => doors = value; }
        public List<Unlockable> Unlockables { get => unlockables; set => unlockables = value; }
        public List<RestSite> RestSites { get => restSites; set => restSites = value; }
        public string Name { get => name; set => name = value; }
        public bool DontApplyProjectileGravity { get => dontApplyProjectileGravity; set => dontApplyProjectileGravity = value; }
        public List<Boss> Boss { get => boss; set => boss = value; }



        // CONSTRUCTORS =================================================================
        public Level(Game1 game, string titFile)
        {
            game.TileManager.LoadContent(titFile);

            this.tiles = game.TileManager.ToSpriteArray();
            this.collisions = game.TileManager.ToCollisionList();

            this.background = null;

            this.doors = new List<Door>();
            this.unlockables = new List<Unlockable>();
            this.restSites = new List<RestSite>();
            this.enemies = new List<Skeleton>();
            this.projectiles = new List<Projectile>();
            this.Boss = new List<Boss>();
            this.checkpointPosition = Vector2.Zero;

            name = "Basic Level";
        }



        // METHODS ======================================================================
        public virtual void DrawText(Game1 game)
        {
            // Level-specific Text
            // Draw level name - FOR DEBUGGING
            // game.SpriteBatch.DrawString(game.SpriteManager.Arial16, "Current Level: " + this.Name, new Vector2(1050, 50), Color.White);
        }

        public virtual void ConnectDoors(Game1 game)
        {
            // Hook up doors here
        }

        public virtual void CreateUnlockable(Game1 game)
        {
            // Create unlockables here
        }

        public virtual void CreateRestSite(Game1 game)
        {
            // Create Rest Sites here
        }

        public virtual void Update(Game1 game)
        {
            // Any level can do its update here
        }

        public void UpdateProjectiles(Game1 game)
        {
            if (!DontApplyProjectileGravity)
            {
                // If there are projectiles in the list, move them appropriately
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].ApplyGravity(game.Gravity / 2);
                }
            }



            // If there are projectiles in the list, move them appropriately
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].Collides(game.Player) && game.Player.SpellState != SpellState.Deflect)
                {
                    if (!game.Player.IsInvincible)
                    {
                        game.Player.DealDamage(projectiles[i].Damage);
                        game.Player.StartInvincibility();
                        projectiles.RemoveAt(i--);
                        continue;
                    }
                }

                // Despawn any projectiles that are too low
                if (projectiles[i].Y > game.GraphicsManager.PreferredBackBufferHeight)
                {
                    projectiles.RemoveAt(i--);
                    continue;
                }
                // Or too far left or right
                if (DontApplyProjectileGravity && (projectiles[i].X < -game.GraphicsManager.PreferredBackBufferWidth 
                    || projectiles[i].X > game.GraphicsManager.PreferredBackBufferWidth * 1.5))
                {
                    projectiles.RemoveAt(i--);
                    continue;
                }
            }
        }

        public virtual void Draw(Game1 game)
        {
            if(background != null)
            {
                game.SpriteBatch.Draw(
                    Background, 
                    new Rectangle(
                        0, 0, 
                        game.GraphicsManager.PreferredBackBufferWidth, 
                        game.GraphicsManager.PreferredBackBufferHeight),
                    Color.White);
            }

            if(tiles == null)
            {
                return;
            }

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null)
                    {
                        tiles[x, y].Draw(game.SpriteBatch, game.Camera);
                    }
                }
            }

            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(game.SpriteBatch, game.Camera);
            }
        }

    }
}
