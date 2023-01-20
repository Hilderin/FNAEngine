using FNAEngine2D.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace FNAEngine2D.GameObjects
{
    /// <summary>
    /// Render for lines
    /// </summary>
    public class LineRender : GameObject
    {
        /// <summary>
        /// Line renderer
        /// </summary>
        private LineRenderer _lineRenderer;


        /// <summary>
        /// Color
        /// </summary>
        [Category("Layout")]
        public Color Color { get { return _lineRenderer.Color; } set { _lineRenderer.Color = value; } }

        /// <summary>
        /// LineWidth
        /// </summary>
        [Category("Layout")]
        [DefaultValue(1f)]
        public float LineWidth { get { return _lineRenderer.LineWidth; } set { _lineRenderer.LineWidth = value; } }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public LineRender()
        {
            _lineRenderer = new LineRenderer();
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public LineRender(Vector2 startPosition, Vector2 stopPosition, Color color, float lineWidth)
        {
            this.Location = startPosition;
            this.Size = stopPosition - startPosition;

            _lineRenderer = new LineRenderer(Vector2.Zero, this.Size, color, lineWidth);
        }

        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            AddComponent(_lineRenderer);
        }

        /// <summary>
        /// On resize...
        /// </summary>
        protected override void OnResized()
        {
            _lineRenderer.OffsetStopPosition = this.Size;
        }

    }
}
