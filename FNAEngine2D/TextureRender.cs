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
    public class TextureRender: GameObject
    {
        /// <summary>
        /// Texture à renderer
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// TextureName
        /// </summary>
        public string TextureName { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TextureRender()
        {
            ContentHelper.ContentChanged += ContentManager_ContentChanged;
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
                    this.Width = _texture.Width;
                if (this.Height == 0)
                    this.Height = _texture.Height;
            }
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            if (_texture == null)
                return;

            GameHost.SpriteBatch.Draw(_texture, this.Bounds, this.Color);
        }

        /// <summary>
        /// Changement du content
        /// </summary>
        private void ContentManager_ContentChanged(string TextureName)
        {
            if(!String.IsNullOrEmpty(this.TextureName) && this.TextureName.Equals(TextureName, StringComparison.OrdinalIgnoreCase))
                _texture = GameHost.GetContent<Texture2D>(this.TextureName);
        }

    }
}
