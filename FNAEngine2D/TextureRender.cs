using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public class TextureRender: GameObject
    {
        /// <summary>
        /// Nom de l'asset
        /// </summary>
        private string _assetName;

        /// <summary>
        /// Texture à renderer
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Color
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextureRender(string assetName)
        {
            _assetName = assetName;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextureRender(string assetName, Rectangle bounds)
        {
            _assetName = assetName;

            this.Bounds = bounds;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextureRender(string assetName, Rectangle bounds, Color color)
        {
            _assetName = assetName;

            this.Bounds = bounds;
            this.Color = color;
        }

        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void Load()
        {
            _texture = GameHost.GetContent<Texture2D>(_assetName);

            if (this.Bounds == Rectangle.Empty)
                this.Bounds = new Rectangle(0, 0, _texture.Width, _texture.Height);
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            GameHost.SpriteBatch.Draw(_texture, this.Bounds, this.Color);
        }

    }
}
