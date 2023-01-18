using FNAEngine2D.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SharpFont.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.GameObjects
{
    /// <summary>
    /// Multi texture render from a enum
    /// </summary>
    public class MultiTextureBox<T> : GameObject where T : System.Enum
    {
        /// <summary>
        /// Texture renderer
        /// </summary>
        private MultiTextureRenderer<T> _textureRenderer;

        /// <summary>
        /// Texture folder
        /// </summary>
        public string TextureFolder { get { return _textureRenderer.TextureFolder; } set { _textureRenderer.TextureFolder = value; } }

        /// <summary>
        /// Color
        /// </summary>
        [Category("Layout")]
        public Color Color { get { return _textureRenderer.Color; } set { _textureRenderer.Color = value; } }

        /// <summary>
        /// Current value
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public T CurrentValue { get; set; }

        /// <summary>
        /// Current texture
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public Texture2D CurrentTexture { get { return _textureRenderer.CurrentTexture; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiTextureBox(string textureFolder)
        {
            _textureRenderer = new MultiTextureRenderer<T>(textureFolder);

            //Default texture...
            CurrentValue = (T)Enum.GetValues(typeof(T)).GetValue(0);

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
