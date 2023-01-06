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
    /// Particule color effect
    /// </summary>
    public class ColorEffect: IParticuleEffect
    {
        /// <summary>
        /// Start color for the particules
        /// </summary>
        public Color ColorStart = Color.Yellow;

        /// <summary>
        /// End color for the particules
        /// </summary>
        public Color ColorEnd = Color.Red;

        /// <summary>
        /// Constructor
        /// </summary>
        public ColorEffect(Color colorStart, Color colorEnd)
        {
            this.ColorStart = colorStart;
            this.ColorEnd = colorEnd;
        }

        /// <summary>
        /// Apply the effect
        /// </summary>
        public void Apply(Particule particule)
        {
            particule.Color = Color.Lerp(this.ColorEnd, this.ColorStart, particule.LifespanAmount);

        }
    }
}
