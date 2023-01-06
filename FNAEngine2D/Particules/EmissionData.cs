using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAEngine2D.Particules
{
    /// <summary>
    /// Information a un emission
    /// </summary>
    public class EmissionData
    {
        
        /// <summary>
        /// Angle of emission. Zero = Up
        /// This angle is in radians
        /// </summary>
        public float Angle = 0f;

        /// <summary>
        /// Variable in the angle each side of the angle.
        /// Pi = all around. (2 Pi = all the circle in radians)
        /// </summary>
        public float AngleVariance = MathHelper.Pi;

        /// <summary>
        /// Lifespan min in seconds for a particule
        /// </summary>
        public float LifespanMinSeconds = 0.1f;

        /// <summary>
        /// Lifespan max in seconds for a particule
        /// </summary>
        public float LifespanMaxSeconds = 2f;

        /// <summary>
        /// Speed min in pixel per seconds
        /// </summary>
        public float SpeedPPSMin = 10f;

        /// <summary>
        /// Speed max in pixel per seconds
        /// </summary>
        public float SpeedPPSMax = 100f;
        
        /// <summary>
        /// Start color for the particules
        /// </summary>
        public Color ColorStart = Color.Yellow;

        /// <summary>
        /// End color for the particules
        /// </summary>
        public Color ColorEnd = Color.Red;

        /// <summary>
        /// Start size in pixels
        /// </summary>
        public float SizeStart = 32f;

        /// <summary>
        /// End size in pixels
        /// </summary>
        public float SizeEnd = 4f;

        /// <summary>
        /// Start opacity
        /// </summary>
        public float OpacityStart = 1f;

        /// <summary>
        /// End opacity
        /// </summary>
        public float OpacityEnd = 0f;

        /// <summary>
        /// Number of particule to emit at each internal
        /// </summary>
        public int EmitCount = 1;

        /// <summary>
        /// Interval between each emission (in seconds)
        /// </summary>
        public float IntervalSeconds = 1f;

        /// <summary>
        /// Texture to use. If null, default texture will ne used
        /// </summary>
        public Content<Texture2D> Texture;

    }
}