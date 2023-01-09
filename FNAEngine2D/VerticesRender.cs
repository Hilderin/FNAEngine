
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
    /// Render for vertices
    /// </summary>
    public class VerticesRender : GameObject
    {
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
        /// Vectices
        /// </summary>
        public List<Vector2> Vectices { get; set; } = new List<Vector2>();

        /// <summary>
        /// Empty constructor
        /// </summary>
        public VerticesRender()
        {

        }


        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            for (int index = 1; index < this.Vectices.Count; index++)
            {
                var line = Add(new LineRender());
                line.TranslateTo(this.Vectices[index - 1]);
                line.Size = this.Vectices[index] - this.Vectices[index - 1];
                line.Color = this.Color;
                line.LineWidth = this.LineWidth;
            }
        }

    }
}
