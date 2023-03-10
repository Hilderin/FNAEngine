using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace FNAEngine2D.TileSets
{
    /// <summary>
    /// Information on a tileset
    /// </summary>
    public class TileSet
    {
        /// <summary>
        /// Texture
        /// </summary>
        private Content<Texture2D> _texture;

        /// <summary>
        /// TextureName
        /// </summary>
        private string _textureName;

        /// <summary>
        /// Tile data
        /// </summary>
        [JsonConverter(typeof(TilesJsonConverter))]
        public Tile[][] Tiles { get; set; }

        /// <summary>
        /// Tile Size
        /// </summary>
        public int TileSize { get; set; } = 16;

        /// <summary>
        /// Tile Size on screen
        /// </summary>
        public int TileScreenSize { get; set; } = 32;

        /// <summary>
        /// Texture for the tileset
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public Texture2D Texture
        { 
            get
            {
                if (_texture != null)
                    return _texture.Data;
                else
                    return null;
            }
            
        }

        /// <summary>
        /// TextureName
        /// </summary>
        public string TextureName
        {
            get { return _textureName; }
            set
            {
                if (_textureName != value)
                {
                    _textureName = value;

                    //Loading Texture...
                    if (String.IsNullOrEmpty(value))
                        _texture = null;
                    else
                        _texture = ContentManager.Current.GetContent<Texture2D>(value);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TileSet()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TileSet(string textureName, int tileSize, int tileScreenSize, Tile[][] tiles)
        {
            this.TextureName = textureName;
            this.TileSize = tileSize;
            this.TileScreenSize = tileScreenSize;
            this.Tiles = tiles;
        }

        /// <summary>
        /// Get the tile at a position
        /// </summary>
        public Tile GetTile(int x, int y)
        {
            if (this.Tiles == null)
                return null;

            if (x < 0 || y < 0)
                return null;

            if (x >= this.Tiles.Length)
                return null;

            Tile[] column = this.Tiles[x];

            if (column == null)
                return null;

            if (y >= column.Length)
                return null;

            return column[y];
        }

        /// <summary>
        /// Set the tile
        /// </summary>
        public bool SetTile(int tileX, int tileY, Tile tile)
        {
            if (tileX < 0 || tileY < 0)
                return false;

            if (this.Tiles == null)
            {
                this.Tiles = new Tile[tileX + 1][];
            }
            else if (this.Tiles.Length <= tileX)
            {
                //We need to grow the tiles array...
                Tile[][] newColTiles = new Tile[tileX + 1][];

                //Copy the existing data...
                Array.Copy(this.Tiles, newColTiles, this.Tiles.Length);

                this.Tiles = newColTiles;
            }

            if (this.Tiles[tileX] == null)
            {
                this.Tiles[tileX] = new Tile[tileY + 1];
            }
            else if (this.Tiles[tileX].Length <= tileY)
            {
                //We need to grow the tiles array...
                Tile[] newRowTiles = new Tile[tileY + 1];

                //Copy the existing data...
                Array.Copy(this.Tiles[tileX], newRowTiles, this.Tiles[tileX].Length);

                this.Tiles[tileX] = newRowTiles;

            }

            if (!AreTileEguals(this.Tiles[tileX][tileY], tile))
            {
                //Needs to update...
                this.Tiles[tileX][tileY] = tile;
                return true;
            }
            else
            {
                return false;
            }


        }

        /// <summary>
        /// Check if 2 tiles are eguals
        /// </summary>
        private bool AreTileEguals(Tile a, Tile b)
        {
            if (a == null && b == null)
                return true;

            if (a == null || b == null)
                return false;

            return a.Col == b.Col && a.Row == b.Row;
        }

        /// <summary>
        /// Override of the ToString to display the TileSet name
        /// </summary>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(_textureName))
                return "TileSet";

            return _textureName;
        }


    }
}
