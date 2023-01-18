
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace FNAEngine2D.Renderers
{
    /// <summary>
    /// Render for lines
    /// </summary>
    public class LineRenderer : Component, IDraw
    {
        /// <summary>
        /// Texture to renderer
        /// </summary>
        private static Content<Texture2D> _texture;

        /// <summary>
        /// Scale
        /// </summary>
        private Vector2 _scale;

        /// <summary>
        /// Rotation
        /// </summary>
        private float _rotation;

        public Vector2 _startPosition { get; set; }
        public Vector2 _stopPosition { get; set; }

        /// <summary>
        /// Start location
        /// </summary>
        public Vector2 StartPosition
        {
            get { return _startPosition; }
            set
            {
                if (_startPosition != value)
                {
                    _startPosition = value;
                    RecalculateScale();
                }
            }
        }

        /// <summary>
        /// Stop location
        /// </summary>
        public Vector2 StopPosition
        {
            get { return _stopPosition; }
            set
            {
                if (_stopPosition != value)
                {
                    _stopPosition = value;
                    RecalculateScale();
                }
            }
        }


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
        public LineRenderer()
        {

        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public LineRenderer(Vector2 startPosition, Vector2 stopPosition, Color color, float lineWidth)
        {
            _startPosition = startPosition;
            _startPosition = stopPosition - startPosition;
            this.Color = color;
            this.LineWidth = lineWidth;
        }


        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            //Texture pixel 1x1
            if (_texture == null)
                _texture = GetContent<Texture2D>(ContentManager.TEXTURE_PIXEL);

            RecalculateScale();
        }

        ///// <summary>
        ///// On resize...
        ///// </summary>
        //protected override void OnResized()
        //{
        //    RecalculateScale();
        //}



        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public void Draw()
        {
            DrawingContext.Draw(_texture.Data, _startPosition, null, this.Color, _rotation, Vector2.Zero, _scale, SpriteEffects.None, this.GameObject.Depth);
        }

        /// <summary>
        /// Recalculate the scale of the texture
        /// </summary>
        private void RecalculateScale()
        {
            Vector2 size = _stopPosition - _startPosition;
            float distance = size.Length();

            _scale = new Vector2(this.LineWidth, distance);
            _rotation = size.ToAngle() - GameMath.PiOver2;
        }
    }
}