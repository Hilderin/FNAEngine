using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAEngine2D.Particules
{

    /// <summary>
    /// Particule emitter
    /// </summary>
    public class ParticleEmitter: GameObject
    {
        /// <summary>
        /// Information on the particules to emit
        /// </summary>
        private EmissionData _emissionData;

        /// <summary>
        /// Interval left before next emission
        /// </summary>
        private float _intervalLeft;


        /// <summary>
        /// Emission data
        /// </summary>
        public EmissionData Emission
        { 
            get { return _emissionData; }
            set { _emissionData = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ParticleEmitter() : this(new EmissionData())
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ParticleEmitter(EmissionData data)
        {
            _emissionData = data;
            _intervalLeft = data.IntervalSeconds;

            
        }

        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            //Default texture if needed
            if (_emissionData.Texture == null)
                _emissionData.Texture = GetContent<Texture2D>(ContentManager.TEXTURE_PARTICULE);
        }


        /// <summary>
        /// Update the particules
        /// </summary>
        protected override void Update()
        {
            //Default texture if needed
            if (_emissionData.Texture == null)
                _emissionData.Texture = GetContent<Texture2D>(ContentManager.TEXTURE_PARTICULE);

            _intervalLeft -= this.ElapsedGameTimeSeconds;
            while (_intervalLeft <= 0f)
            {
                _intervalLeft += _emissionData.IntervalSeconds;

                for (int i = 0; i < _emissionData.EmitCount; i++)
                {
                    Emit();
                }
            }
        }


        /// <summary>
        /// Do the emission
        /// </summary>
        private void Emit()
        {
            ParticleData particuleData = new ParticleData();
            particuleData.LifespanSeconds = GameMath.RandomFloat(_emissionData.LifespanMinSeconds, _emissionData.LifespanMaxSeconds);
            particuleData.SpeedPPS = GameMath.RandomFloat(_emissionData.SpeedPPSMin, _emissionData.SpeedPPSMax);
            particuleData.Angle = GameMath.RandomFloat(_emissionData.Angle - _emissionData.AngleVariance, _emissionData.Angle + _emissionData.AngleVariance);

            Add(new Particle(this.Location, particuleData, _emissionData));
        }

    }
}