using FNAEngine2D.Renderers;
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

namespace FNAEngine2D.GameObjects
{
    public class TextureBox: GameObject
    {
        /// <summary>
        /// Texture renderer
        /// </summary>
        private TextureRenderer _textureRenderer;

        /// <summary>
        /// TextureName
        /// </summary>
        [Category("Layout")]
        [DefaultValue("")]
        public string TextureName { get { return _textureRenderer.TextureName; } set { _textureRenderer.TextureName = value; } }

        /// <summary>
        /// Color
        /// </summary>
        [Category("Layout")]
        public Color Color { get { return _textureRenderer.Color; } set { _textureRenderer.Color = value; } }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TextureBox()
        {
            _textureRenderer = new TextureRenderer();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TextureBox(string textureName): this()
        {
            _textureRenderer.TextureName = textureName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TextureBox(string textureName, Rectangle bounds) : this(textureName)
        {
            this.Bounds = bounds;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextureBox(string textureName, Rectangle bounds, Color color) : this(textureName, bounds)
        {
            _textureRenderer.Color = color;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TextureBox(string textureName, Vector2 location, Vector2 size, Color color) : this(textureName)
        {
            _textureRenderer.Color = color;
            this.Location = location;
            this.Size = size;
        }

        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            AddComponent(_textureRenderer);

        }

    }
}
