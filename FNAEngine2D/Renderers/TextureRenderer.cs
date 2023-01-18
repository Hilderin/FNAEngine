using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Renderers
{
    public class TextureRenderer: Component, IDraw
    {
        /// <summary>
        /// Texture to renderer
        /// </summary>
        private Content<Texture2D> _texture;

        /// <summary>
        /// Scale
        /// </summary>
        private Vector2 _scale = Vector2.One;


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
        public TextureRenderer()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TextureRenderer(string textureName): this()
        {
            this.TextureName = textureName;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextureRenderer(string textureName, Color color) : this(textureName)
        {
            this.Color = color;
        }


        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            if (String.IsNullOrEmpty(this.TextureName))
            {
                _texture = null;
            }
            else
            {
                _texture = GetContent<Texture2D>(this.TextureName);

                RecalculateScale();
            }
        }

        /// <summary>
        /// On resize...
        /// </summary>
        protected override void OnResized()
        {
            RecalculateScale();
        }

        /// <summary>
        /// Recalculate the scale
        /// </summary>
        private void RecalculateScale()
        {
            if (_texture == null || _texture.Data.Width == 0 || _texture.Data.Height == 0)
            {
                _scale = Vector2.Zero;
            }
            else
            {
                _scale = new Vector2(this.GameObject.Width / _texture.Data.Width, this.GameObject.Height / _texture.Data.Height);
            }
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public void Draw()
        {
            if (_texture == null)
                return;

            DrawingContext.Draw(_texture.Data, this.GameObject.Location, null, this.Color, 0f, Vector2.Zero, _scale, SpriteEffects.None, this.GameObject.Depth);
        }

    }
}
