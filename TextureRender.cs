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
        /// Position à l'écran
        /// </summary>
        public Rectangle Destination;

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
        public TextureRender(string assetName, Rectangle destination)
        {
            _assetName = assetName;

            this.Destination = destination;
        }

        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void Load()
        {
            _texture = GameHost.GetContent<Texture2D>(_assetName);

            if (this.Destination == Rectangle.Empty)
                this.Destination = new Rectangle(0, 0, _texture.Width, _texture.Height);
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            GameHost.SpriteBatch.Draw(_texture, this.Destination,  Color.Wheat);
        }

    }
}
