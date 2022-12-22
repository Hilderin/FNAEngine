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
        /// Texture
        /// </summary>
        private Content<Texture2D> _texture; 

        /// <summary>
        /// TextureName
        /// </summary>
        private string _textureName;

        /// <summary>
        /// Width of a column
        /// </summary>
        public int ColumnWidth { get; set; } = 16;

        /// <summary>
        /// Height of a row
        /// </summary>
        public int RowHeight { get; set; } = 16;

        /// <summary>
        /// Width of a column on screen
        /// </summary>
        public int ColumnScreenWidth { get; set; } = 32;

        /// <summary>
        /// Height of a row on screen
        /// </summary>
        public int RowScreenHeight { get; set; } = 32;

        /// <summary>
        /// Texture for the tileset
        /// </summary>
        public Texture2D Texture { get { return _texture.Data; } }


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
        public Sprite()
        {
        }

        /// <summary>
        /// Set manually the texture (without hot reload)
        /// </summary>
        public void SetTexture(Texture2D texture)
        {
            _texture = new Content<Texture2D>(texture);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Sprite(string textureName, int tileWidth, int tileHeight, int tileScreenWidth, int tileScreenHeight): this()
        {
            this.TextureName = textureName;
            this.ColumnWidth = tileWidth;
            this.RowHeight = tileHeight;
            this.ColumnScreenWidth = tileScreenWidth;
            this.RowScreenHeight = tileScreenHeight;
        }

    }
}
