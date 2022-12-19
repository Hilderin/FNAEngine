using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public class SpriteRender : GameObject
    {
        /// <summary>
        /// Sprite
        /// </summary>
        private Sprite _sprite;
        
        /// <summary>
        /// Information on the sprite
        /// </summary>
        public string SpriteName { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// X in the sprite sheet
        /// </summary>
        public int SpriteX { get; set; }

        /// <summary>
        /// Y in the sprite sheet
        /// </summary>
        public int SpriteY { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public SpriteRender()
        {
            ContentHelper.ContentChanged += ContentManager_ContentChanged;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public SpriteRender(string spriteName, int spriteX, int spriteY): this()
        {
            this.SpriteName = spriteName;
            this.SpriteX = spriteX;
            this.SpriteY = spriteY;
            
        }

        /// <summary>
        /// Loading...
        /// </summary>
        public override void Load()
        {
            if (String.IsNullOrEmpty(this.SpriteName))
            {
                _sprite = null;
            }
            else
            {
                _sprite = GameHost.GetContent<Sprite>(this.SpriteName);

                if (this.Width == 0)
                    this.Width = _sprite.TileScreenWidth;
                if (this.Height == 0)
                    this.Height = _sprite.TileScreenHeight;
            }

        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            if (_sprite == null || _sprite.Texture == null)
                return;

            GameHost.SpriteBatch.Draw(_sprite.Texture, this.Bounds, new Rectangle(this.SpriteX * _sprite.TileWidth, this.SpriteY * _sprite.TileHeight, _sprite.TileWidth, _sprite.TileHeight), this.Color);

        }


        /// <summary>
        /// Changement du content
        /// </summary>
        private void ContentManager_ContentChanged(string assetName)
        {
            if (!String.IsNullOrEmpty(this.SpriteName) && this.SpriteName.Equals(assetName, StringComparison.OrdinalIgnoreCase))
                _sprite = GameHost.GetContent<Sprite>(this.SpriteName);
        }


    }
}
