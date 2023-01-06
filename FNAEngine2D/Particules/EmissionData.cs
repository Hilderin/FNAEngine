using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
        public float LifespanMinSeconds = 1f;

        /// <summary>
        /// Lifespan max in seconds for a particule
        /// </summary>
        public float LifespanMaxSeconds = 1f;

        /// <summary>
        /// Speed min in pixel per seconds
        /// </summary>
        public float SpeedPPSMin = 100f;

        /// <summary>
        /// Speed max in pixel per seconds
        /// </summary>
        public float SpeedPPSMax = 100f;
        
        /// <summary>
        /// Number of particules min to emit at each internal
        /// </summary>
        public int EmitCountMin = 1;

        /// <summary>
        /// Number of particules max to emit at each internal
        /// </summary>
        public int EmitCountMax = 1;

        /// <summary>
        /// Interval between each emission (in seconds)
        /// </summary>
        public float IntervalSeconds = 1f;

        /// <summary>
        /// Texture to use. If null, default texture will ne used
        /// </summary>
        public Content<Texture2D> Texture;

        /// <summary>
        /// Effects to apply on the start
        /// </summary>
        public List<IParticuleEffect> StartEffects { get; set; } = new List<IParticuleEffect>();

        /// <summary>
        /// Effects
        /// </summary>
        public List<IParticuleEffect> Effects { get; set; } = new List<IParticuleEffect>();

    }
}