using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Information on a Sprite
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// TextureName
        /// </summary>
        private string _textureName;

        /// <summary>
        /// Tile Width
        /// </summary>
        public int TileWidth { get; set; } = 16;

        /// <summary>
        /// Tile Width
        /// </summary>
        public int TileHeight { get; set; } = 16;

        /// <summary>
        /// Tile Width on screen
        /// </summary>
        public int TileScreenWidth { get; set; } = 32;

        /// <summary>
        /// Tile Height on screen
        /// </summary>
        public int TileScreenHeight { get; set; } = 32;

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
        public Sprite()
        {
            ContentHelper.ContentChanged += ContentManager_ContentChanged;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Sprite(string textureName, int tileWidth, int tileHeight, int tileScreenWidth, int tileScreenHeight): this()
        {
            this.TextureName = textureName;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.TileScreenWidth = tileScreenWidth;
            this.TileScreenHeight = tileScreenHeight;
        }



        /// <summary>
        /// Changement du content
        /// </summary>
        private void ContentManager_ContentChanged(string textureName)
        {
            if (!String.IsNullOrEmpty(this.TextureName) && this.TextureName.Equals(textureName, StringComparison.OrdinalIgnoreCase))
                this.Texture = GameHost.GetContent<Texture2D>(this.TextureName);
        }
    }
}
