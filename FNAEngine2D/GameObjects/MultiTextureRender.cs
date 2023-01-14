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
    public class MultiTextureRender<T> : GameObject where T : System.Enum
    {
        /// <summary>
        /// Textures
        /// </summary>
        private Dictionary<T, TextureInfo> _textures = new Dictionary<T, TextureInfo>();


        /// <summary>
        /// Texture folder
        /// </summary>
        public string TextureFolder { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        [Category("Layout")]
        public Color Color { get; set; } = Color.White;

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
        public Texture2D CurrentTexture { get { return _textures[this.CurrentValue].Texture.Data; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiTextureRender(string textureFolder)
        {
            this.TextureFolder = textureFolder;

            //Default texture...
            CurrentValue = (T)Enum.GetValues(typeof(T)).GetValue(0);

        }


        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            if(this.Parent != null)
                this.Bounds = this.Parent.Bounds;

            LoadTextures();
            
        }

        /// <summary>
        /// Draw the texture
        /// </summary>
        protected override void Draw()
        {
            TextureInfo textureInfo = _textures[this.CurrentValue];
            DrawingContext.Draw(textureInfo.Texture.Data, this.Location, null, this.Color, 0f, Vector2.Zero, textureInfo.Scale, SpriteEffects.None, this.Depth);
        }


        /// <summary>
        /// On resized...
        /// </summary>
        protected override void OnResized()
        {
            RecalculateScale();
        }


        /// <summary>
        /// Load textures
        /// </summary>
        private void LoadTextures()
        {
            foreach (var textureValue in Enum.GetValues(typeof(T)))
            {
                T textureEnum = (T)textureValue;

                string assetName = Path.Combine(this.TextureFolder, textureEnum.ToString());

                Content<Texture2D> texture = GetContent<Texture2D>(assetName);

                if (this.Width == 0)
                    this.Width = texture.Data.Width;
                if (this.Height == 0)
                    this.Height = texture.Data.Height;

                TextureInfo textureInfo = new TextureInfo()
                {
                    Texture = texture
                };
                _textures[textureEnum] = textureInfo;
            }

            RecalculateScale();
        }

        /// <summary>
        /// Recalculate the scale
        /// </summary>
        private void RecalculateScale()
        {
            foreach (TextureInfo textureInfo in _textures.Values)
            {
                if (textureInfo.Texture.Data.Width == 0 || textureInfo.Texture.Data.Height == 0)
                    textureInfo.Scale = Vector2.Zero;
                else
                    textureInfo.Scale = new Vector2(this.Width / textureInfo.Texture.Data.Width, this.Height / textureInfo.Texture.Data.Height);
            }
        }

        /// <summary>
        /// Info on the texture in memory
        /// </summary>
        private class TextureInfo
        {
            public Content<Texture2D> Texture;
            public Vector2 Scale;
        }

    }
}
