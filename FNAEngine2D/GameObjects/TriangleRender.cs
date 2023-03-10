
using FNAEngine2D.Geometry;
using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace FNAEngine2D.GameObjects
{
    /// <summary>
    /// Render for Triangle
    /// </summary>
    public class TriangleRender : GameObject, IUpdate
    {
        /// <summary>
        /// Last triangle
        /// </summary>
        private Triangle _lastTriangle;

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
        /// Triangle
        /// </summary>
        public Triangle Triangle { get; set; }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TriangleRender()
        {

        }

        /// <summary>
        /// Constructor with a triangle
        /// </summary>
        public TriangleRender(Triangle triangle, Color color, float lineWidth)
        {
            this.Triangle = triangle;
            this.Color = color;
            this.LineWidth = lineWidth;
        }


        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            UpdateLines();


        }

        /// <summary>
        /// Loading
        /// </summary>
        public void Update()
        {
            if (_lastTriangle != this.Triangle)
                UpdateLines();
        }

        /// <summary>
        /// Update lines
        /// </summary>
        private void UpdateLines()
        {
            this.RemoveAll();

            if (Triangle != null)
            {
                Add(new LineRender(Triangle.v1.position, Triangle.v2.position, this.Color, this.LineWidth));
                Add(new LineRender(Triangle.v2.position, Triangle.v3.position, this.Color, this.LineWidth));
                Add(new LineRender(Triangle.v3.position, Triangle.v1.position, this.Color, this.LineWidth));
            }

            _lastTriangle = this.Triangle;
        }


    }
}
