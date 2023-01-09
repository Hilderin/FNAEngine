
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Render for Triangle
    /// </summary>
    public class TriangleRender : GameObject
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
        protected override void Update()
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
