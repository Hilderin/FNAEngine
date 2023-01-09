using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms.VisualStyles;

namespace FNAEngine2D.Particules
{

    /// <summary>
    /// Particule emitter
    /// </summary>
    public class ParticuleEmitter: GameObject
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
        public ParticuleEmitter()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ParticuleEmitter(Vector2 location)
        {
            this.Location = location;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ParticuleEmitter(EmissionData data)
        {
            _emissionData = data;
            _intervalLeft = data.IntervalSeconds;

            
        }

        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            if (_emissionData != null)
            {
                //Default texture if needed
                if (_emissionData.Texture == null)
                    _emissionData.Texture = GetContent<Texture2D>(ContentManager.TEXTURE_PARTICULE);
            }
        }


        /// <summary>
        /// Update the particules
        /// </summary>
        protected override void Update()
        {
            if (_emissionData != null && _emissionData.IntervalSeconds > 0f)
            {
                _intervalLeft -= this.ElapsedGameTimeSeconds;
                while (_intervalLeft <= 0f)
                {
                    _intervalLeft += _emissionData.IntervalSeconds;

                    Emit();
                }
            }
        }

        /// <summary>
        /// Do the emission
        /// </summary>
        public void Emit()
        {
            //Default texture if needed
            if (_emissionData.Texture == null)
                _emissionData.Texture = GetContent<Texture2D>(ContentManager.TEXTURE_PARTICULE);

            int nbParticules = GameMath.RandomInt(_emissionData.EmitCountMin, _emissionData.EmitCountMax);
            for (int i = 0; i <= nbParticules; i++)
            {
                EmitParticule();
            }

        }

        /// <summary>
        /// Do the emission
        /// </summary>
        public void EmitParticule()
        {
            ParticuleData particuleData = new ParticuleData();
            particuleData.LifespanSeconds = GameMath.RandomFloat(_emissionData.LifespanMinSeconds, _emissionData.LifespanMaxSeconds);
            particuleData.SpeedPPS = GameMath.RandomFloat(_emissionData.SpeedPPSMin, _emissionData.SpeedPPSMax);
            particuleData.Angle = GameMath.RandomFloat(_emissionData.Angle - _emissionData.AngleVariance, _emissionData.Angle + _emissionData.AngleVariance);


            //Add in the root object so the particule does not move with the emitter and if we disable the emitter
            //the particules continue to update, etc...
            this.RootGameObject.Add(new Particule(this.Location, particuleData, _emissionData));
        }

    }
}