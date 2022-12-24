using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public class TextureRender: GameObject
    {
        /// <summary>
        /// Texture à renderer
        /// </summary>
        private Content<Texture2D> _texture;

        /// <summary>
        /// TextureName
        /// </summary>
        [Category("Layout")]
        [DefaultValue("")]
        public string TextureName { get; set; } = String.Empty;

        /// <summary>
        /// Color
        /// </summary>
        [Category("Layout")]
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TextureRender()
        {
            
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextureRender(string textureName): this()
        {
            this.TextureName = textureName;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextureRender(string textureName, Rectangle bounds) : this(textureName)
        {
            this.Bounds = bounds;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextureRender(string TextureName, Rectangle bounds, Color color) : this(TextureName, bounds)
        {
            this.Color = color;
        }

        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void Load()
        {
            if (String.IsNullOrEmpty(this.TextureName))
            {
                _texture = null;
            }
            else
            {
                _texture = GameHost.GetContent<Texture2D>(this.TextureName);

                if (this.Width == 0)
                    this.Width = _texture.Data.Width;
                if (this.Height == 0)
                    this.Height = _texture.Data.Height;
            }
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            if (_texture == null)
                return;

            DrawingContext.Draw(_texture.Data, this.Bounds, null, this.Color, 0f, Vector2.Zero, SpriteEffects.None, this.Depth);
        }

    }
}
