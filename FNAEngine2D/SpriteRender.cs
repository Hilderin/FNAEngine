using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;
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
        private Content<Sprite> _sprite;
        
        /// <summary>
        /// Information on the sprite
        /// </summary>
        public string SpriteName { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Column in the sprite sheet
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// Row in the sprite sheet
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public SpriteRender()
        {
            
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public SpriteRender(string spriteName, int spriteX, int spriteY): this()
        {
            this.SpriteName = spriteName;
            this.ColumnIndex = spriteX;
            this.RowIndex = spriteY;
            
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
                    this.Width = _sprite.Data.ColumnScreenWidth;
                if (this.Height == 0)
                    this.Height = _sprite.Data.RowScreenHeight;
            }

        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            if (_sprite == null || _sprite.Data.Texture == null)
                return;

            Sprite sprite = _sprite.Data;

            GameHost.SpriteBatch.Draw(sprite.Texture, this.Bounds, new Rectangle(this.ColumnIndex * sprite.ColumnWidth, this.RowIndex * sprite.RowHeight, sprite.ColumnWidth, sprite.RowHeight), this.Color);

        }



    }
}
