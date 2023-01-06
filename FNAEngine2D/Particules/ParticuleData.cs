using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAEngine2D.Particules
{

    /// <summary>
    /// Particule data
    /// </summary>
    public class ParticuleData
    {
        /// <summary>
        /// Life span in seconds
        /// </summary>
        public float LifespanSeconds = 2f;     

        /// <summary>
        /// Speed in pixel per seconds
        /// </summary>
        public float SpeedPPS = 100f;

        /// <summary>
        /// Angle in radians
        /// </summary>
        public float Angle = 0f;

    }
}