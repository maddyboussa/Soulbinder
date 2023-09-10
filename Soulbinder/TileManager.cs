using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Soulbinder.GameObjects;
using System.IO;

namespace Soulbinder
{
    public class TileManager
    {
        // FIELDS =======================================================================
        private Game1 game;

        // Global Tile-Manager Variables
        private Tile[,] tiles;  // 2D array of current level
        private int mapWidth;   // Width of current levels in tiles
        private int mapHeight;  // Height of current levels in tiles
        private int tileSize;   // Pixel width/height of tiles (tiles are squares)

        // PROPERTIES ===================================================================
        public Tile[,] Tiles
        {
            get { return tiles; }
        }
        
        public int MapWidth
        {
            get { return mapWidth; }
        }
        public int MapHeight
        {
            get { return mapHeight; }
        }
        

        // CONSTRUCTORS =================================================================
        public TileManager(Game1 game)
        {
            this.game = game;

            tiles = null;
        }

        // METHODS ======================================================================
        
        /// <summary>
        /// Initializes a 2d array of tiles using data from a given .tit file.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="titFile"></param>
        public void LoadContent(string titFile)
        {
            
            Stream saveDataStream = null;

            try
            {
                saveDataStream = File.OpenRead(titFile);
                BinaryReader saveData = new BinaryReader(saveDataStream);

                mapWidth = saveData.ReadInt32();
                mapHeight = saveData.ReadInt32();

                tileSize = game.GraphicsManager.PreferredBackBufferHeight / mapHeight;

                tiles = new Tile[mapWidth, mapHeight];

                for (int x = 0; x < mapWidth; x++)
                {
                    for (int y = 0; y < mapHeight; y++)
                    {
                        switch (saveData.ReadString())
                        {
                            case "darkBrick":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.DarkBrick, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            case "darkBrickFloor":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.DarkBrickFloor, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            case "darkGray":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.DarkGray, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            case "darkGrayFloor":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.DarkGrayFloor, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            case "lightBrick":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.LightBrick, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            case "lightBrickFloor":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.LightBrickFloor, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            case "lightGray":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.LightGray, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            case "lightGrayFloor":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.LightGrayFloor, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            case "missingTexture":
                                tiles[x, y] = new Tile(
                                    game.SpriteManager.MissingTexture, 
                                    new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize));
                                break;
                            default:
                                tiles[x, y] = null;
                                break;
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {
                if (saveDataStream != null)
                {
                    saveDataStream.Close();
                }
            }
        }

        /// <summary>
        /// Returns a 2D Array of Tiles repreenting the tiles of a level.
        /// </summary>
        /// <returns></returns>
        public Tile[,] ToSpriteArray()
        {
            Tile[,] spriteArray = new Tile[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    spriteArray[x, y] = tiles[x, y];
                }
            }

            return spriteArray;
        }

        /// <summary>
        /// Returns a List of Rectangles for use with Entity Collision.
        /// </summary>
        /// <returns></returns>
        public List<Rectangle> ToCollisionList()
        {
            List<Rectangle> rectList = new List<Rectangle>();

            Tile anchor;
            int count;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    if (tiles[x, y] != null)
                    {
                        anchor = tiles[x, y];
                        count = 0;

                        while(x < mapWidth && tiles[x,y] != null)
                        {
                            x++;
                            count++;
                        }

                        rectList.Add(new Rectangle(anchor.X, anchor.Y, tileSize * count, tileSize));
                    }
                }
            }

            for(int i = rectList.Count - 1; i >= 0; i--)
            {
                for(int v = rectList.Count - 1; v >= 0; v--)
                {
                    if (rectList[i].X == rectList[v].X
                        && rectList[i].X + rectList[i].Width == rectList[v].X + rectList[v].Width
                        && rectList[i].Y + tileSize == rectList[v].Y)
                    {
                        rectList[i] = new Rectangle(rectList[i].X, rectList[i].Y,
                            rectList[i].Width, rectList[i].Height + rectList[v].Height);

                        rectList.RemoveAt(v);
                    }
                }
            }

            return rectList;
        }

    }
}
