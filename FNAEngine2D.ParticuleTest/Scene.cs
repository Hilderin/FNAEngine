using FNAEngine2D;
using FNAEngine2D.Particules;
using Microsoft.Xna.Framework;

namespace FNAEngine2D.ParticuleTest
{
    public class Scene : GameObject
    {
        /// <summary>
        /// Mouse Emitter
        /// </summary>
        private ParticleEmitter _mouseEmitter;

        /// <summary>
        /// CircleEmitter
        /// </summary>
        private ParticleEmitter _circleEmitter;

        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            Add(new FPSRender());


            //Mouse emitter at the mouse position
            _mouseEmitter = Add(new ParticleEmitter());
            _mouseEmitter.TranslateTo(this.Game.CenterX, this.Game.CenterY);

            _mouseEmitter.Emission.IntervalSeconds = 0.01f;
            _mouseEmitter.Emission.EmitCount = 10;

            //Circle emitter
            _circleEmitter = Add(new ParticleEmitter());
            _circleEmitter.TranslateTo(700, 500);

            _circleEmitter.Emission = new EmissionData()
            {
                IntervalSeconds = 0.2f,
                EmitCount = 150,
                LifespanMinSeconds = 2f,
                LifespanMaxSeconds = 2f,
                SpeedPPSMin = 100f,
                SpeedPPSMax = 100f,
                ColorStart = Color.Green,
                ColorEnd = Color.Red,
                SizeStart = 8,
                SizeEnd = 32
            };
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        protected override void Update()
        {
            _mouseEmitter.Location = this.Input.GetMousePosition();

        }

    }
}
