
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

        public Vector2 _offsetStartPosition { get; set; }
        public Vector2 _offsetStopPosition { get; set; }

        /// <summary>
        /// Offset start location
        /// Offset from the location of the parent GameObject
        /// </summary>
        public Vector2 OffsetStartPosition
        {
            get { return _offsetStartPosition; }
            set
            {
                if (_offsetStartPosition != value)
                {
                    _offsetStartPosition = value;
                    RecalculateScale();
                }
            }
        }

        /// <summary>
        /// Offset Stop location
        /// Offset from the location of the parent GameObject
        /// </summary>
        public Vector2 OffsetStopPosition
        {
            get { return _offsetStopPosition; }
            set
            {
                if (_offsetStopPosition != value)
                {
                    _offsetStopPosition = value;
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
        public LineRenderer(Vector2 offsetStartPosition, Vector2 offsetStopPosition, Color color, float lineWidth)
        {
            _offsetStartPosition = offsetStartPosition;
            _offsetStopPosition = offsetStopPosition;
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



        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public void Draw()
        {
            DrawingContext.Draw(_texture.Data, this.GameObject.Location + _offsetStartPosition, null, this.Color, _rotation, Vector2.Zero, _scale, SpriteEffects.None, this.GameObject.Depth);
        }

        /// <summary>
        /// Recalculate the scale of the texture
        /// </summary>
        private void RecalculateScale()
        {
            Vector2 size = _offsetStopPosition - _offsetStartPosition;
            float distance = size.Length();

            _scale = new Vector2(this.LineWidth, distance);
            _rotation = size.ToAngle() - GameMath.PiOver2;
        }
    }
}