using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                        _texture = GameHost.GetContent<Texture2D>(value);
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
