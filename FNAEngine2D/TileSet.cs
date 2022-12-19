using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Information on a tileset
    /// </summary>
    public class TileSet
    {
        /// <summary>
        /// TextureName
        /// </summary>
        private string _textureName;

        /// <summary>
        /// Tileset
        /// </summary>
        public string TileSetName { get; set; }

        /// <summary>
        /// Tile data
        /// </summary>
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
        public Texture2D Texture { get; set; }

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
                        this.Texture = null;
                    else
                        this.Texture = GameHost.GetContent<Texture2D>(value);
                }
            }
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

            ContentHelper.ContentChanged += ContentManager_ContentChanged;
        }


        /// <summary>
        /// Changement du content
        /// </summary>
        private void ContentManager_ContentChanged(string textureName)
        {
            if (!String.IsNullOrEmpty(this.TileSetName) && this.TileSetName.Equals(textureName, StringComparison.OrdinalIgnoreCase))
                this.Texture = GameHost.GetContent<Texture2D>(this.TileSetName);
        }
    }
}
