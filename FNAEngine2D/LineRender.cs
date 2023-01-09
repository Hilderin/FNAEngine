
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

namespace FNAEngine2D
{
    /// <summary>
    /// Render for lines
    /// </summary>
    public class LineRender : GameObject
    {
        /// <summary>
        /// Texture to renderer
        /// </summary>
        private Content<Texture2D> _texture;

        /// <summary>
        /// Scale
        /// </summary>
        private Vector2 _scale;

        /// <summary>
        /// Rotation
        /// </summary>
        private float _rotation;


        /// <summary>
        /// Color
        /// </summary>
        [Category("Layout")]
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// LineWidth
        /// </summary>
        [Category("Layout")]
        [DefaultValue(1f)]
        public float LineWidth { get; set; } = 1f;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public LineRender()
        {

        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public LineRender(Vector2 startPosition, Vector2 stopPosition, Color color, float lineWidth)
        {
            this.Location = startPosition;
            this.Size = stopPosition - startPosition;
            this.Color = color;
            this.LineWidth = lineWidth;
        }


        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            //Texture pixel 1x1
            _texture = GetContent<Texture2D>(ContentManager.TEXTURE_PIXEL);

            RecalculateScale();
        }

        /// <summary>
        /// On resize...
        /// </summary>
        protected override void OnResized()
        {
            RecalculateScale();
        }
                


        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        protected override void Draw()
        {
            DrawingContext.Draw(_texture.Data, this.Location, null, this.Color, _rotation, Vector2.Zero, _scale, SpriteEffects.None, this.Depth);
        }

        /// <summary>
        /// Recalculate the scale of the texture
        /// </summary>
        private void RecalculateScale()
        {
            float distance = this.Size.Length();

            _scale = new Vector2(this.LineWidth, distance);
            _rotation = this.Size.ToAngle() - GameMath.PiOver2;
        }
    }
}
