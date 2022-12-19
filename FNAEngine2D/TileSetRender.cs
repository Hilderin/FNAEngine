using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Tileset
    /// </summary>
    public class TileSetRender: GameObject
    {
        /// <summary>
        /// Information on tileset
        /// </summary>
        public TileSet TileSet { get; set; }


        /// <summary>
        /// Empty constructor
        /// </summary>
        public TileSetRender()
        {
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TileSetRender(TileSet tileSet)
        {
            this.TileSet = tileSet;
        }

        /// <summary>
        /// Overwride du drawing...
        /// </summary>
        public override void Draw()
        {
            if (this.TileSet == null)
                return;

            TileSet tileset = this.TileSet;
            Tile[][] tiles = tileset.Tiles;
            Texture2D texture = tileset.Texture;

            if(texture == null || tiles == null)
                return;
                        

            int posX = 0;
            for (int x = 0; x < tiles.Length; x++)
            {
                Tile[] column = tiles[x];

                int posY = 0;
                for (int y = 0; y < column.Length; y++)
                {
                    if (column[y] != null)
                    {
                        GameHost.SpriteBatch.Draw(texture, new Rectangle(posX, posY, tileset.TileScreenSize, tileset.TileScreenSize), new Rectangle(column[y].X * tileset.TileSize, column[y].Y * tileset.TileSize, tileset.TileSize, tileset.TileSize), Color.White);
                        //GameHost.SpriteBatch.Draw(_texture, new Vector2(posX, posY), new Rectangle(column[y].X * TileSize, column[y].Y * TileSize, TileSize, TileSize), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    }

                    posY += tileset.TileScreenSize;
                }

                posX += tileset.TileScreenSize;
            }

        }


    }
}
