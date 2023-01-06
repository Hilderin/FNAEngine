using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Particules
{
    /// <summary>
    /// Particule Opacity effect
    /// </summary>
    public class OpacityEffect : IParticuleEffect
    {
        //// <summary>
        /// Start opacity
        /// </summary>
        public float OpacityStart;

        /// <summary>
        /// End opacity
        /// </summary>
        public float OpacityEnd;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpacityEffect(float opacityStart, float opacityEnd)
        {
            this.OpacityStart = opacityStart;
            this.OpacityEnd = opacityEnd;
        }

        /// <summary>
        /// Apply the effect
        /// </summary>
        public void Apply(Particule particule)
        {
            particule.Opacity = MathHelper.Clamp(MathHelper.Lerp(this.OpacityEnd, this.OpacityStart, particule.LifespanAmount), 0, 1);

        }
    }
}
