using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Multi texture render from a enum
    /// </summary>
    public class MultiTextureRender<T> : GameObject where T : System.Enum
    {
        /// <summary>
        /// Textures
        /// </summary>
        private Dictionary<T, Content<Texture2D>> _textures = new Dictionary<T, Content<Texture2D>>();

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
        public Texture2D CurrentTexture { get { return _textures[this.CurrentValue].Data; } }

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
        public override void Load()
        {
            if(this.Parent != null)
                this.Bounds = this.Parent.Bounds;

            LoadTextures();
            
        }

        /// <summary>
        /// Draw the texture
        /// </summary>
        public override void Draw()
        {
            DrawingContext.Draw(_textures[this.CurrentValue].Data, this.Bounds, null, this.Color, 0f, Vector2.Zero, SpriteEffects.None, this.Depth);
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

                _textures[textureEnum] = texture;
            }
        }


    }
}
