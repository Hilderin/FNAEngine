using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Particules
{
    /// <summary>
    /// Particule size effect
    /// </summary>
    public class SizeEffect : IParticuleEffect
    {
        /// <summary>
        /// Start width in pixels
        /// </summary>
        public float WidthStart;

        /// <summary>
        /// End width in pixels
        /// </summary>
        public float WidthEnd;

        /// <summary>
        /// Start height in pixels
        /// </summary>
        public float HeightStart;

        /// <summary>
        /// End width in pixels
        /// </summary>
        public float HeightEnd;

        /// <summary>
        /// Constructor
        /// </summary>
        public SizeEffect(float start, float end)
        {
            this.WidthStart = start;
            this.WidthEnd = end;
            this.HeightStart = start;
            this.HeightEnd = end;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SizeEffect(float widthStart, float widthEnd, float heightStart, float heightEnd)
        {
            this.WidthStart = widthStart;
            this.WidthEnd = widthEnd;
            this.HeightStart = heightStart;
            this.HeightEnd = heightEnd;
        }

        /// <summary>
        /// Apply the effect
        /// </summary>
        public void Apply(Particule particule)
        {
            float scaleWidthLerp = MathHelper.Lerp(this.WidthEnd, this.WidthStart, particule.LifespanAmount);
            float scaleHeightLerp = MathHelper.Lerp(this.HeightEnd, this.HeightStart, particule.LifespanAmount);

            particule.Scale = new Vector2(scaleWidthLerp / particule.Texture.Width, scaleHeightLerp / particule.Texture.Height);

        }
    }
}
