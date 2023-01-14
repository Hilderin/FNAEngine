using FNAEngine2D;
using FNAEngine2D.GameObjects;
using FNAEngine2D.Particules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FNAEngine2D.ParticuleTest
{
    public class Scene : GameObject
    {
        /// <summary>
        /// Cursor
        /// </summary>
        private GameObject _mouseCursor;

        /// <summary>
        /// Mouse Emitter
        /// </summary>
        private ParticuleEmitter _mouseEmitter;

        /// <summary>
        /// CircleEmitter
        /// </summary>
        private ParticuleEmitter _circleEmitter;

        /// <summary>
        /// Shot Emitter
        /// </summary>
        private ParticuleEmitter _shotEmitter;

        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            Add(new FPSRender());

            _mouseCursor = Add(new PrimitiveRender(GameObjects.PrimitiveType.RectangleFill, Vector2.Zero, Vector2.One, Color.White));


            //Mouse emitter at the mouse position
            _mouseEmitter = Add(new ParticuleEmitter());
            _mouseEmitter.TranslateTo(this.Game.CenterX, this.Game.CenterY);

            _mouseEmitter.Emission = new EmissionData()
            {
                IntervalSeconds = 0.01f,
                EmitCountMin = 10,
                EmitCountMax = 10,
                Effects = new List<IParticuleEffect>
                {
                    new ColorEffect(Color.Red, Color.Blue),
                    new SizeEffect(8, 32),
                    new OpacityEffect(1f, 0f),
                }

            };
            //_mouseEmitter.Paused = true;

            //Effects.Add(new ColorEffect(Color.Yellow, Color.Red));


            //Circle emitter
            _circleEmitter = Add(new ParticuleEmitter());
            _circleEmitter.TranslateTo(700, 500);

            _circleEmitter.Emission = new EmissionData()
            {
                IntervalSeconds = 0.2f,
                EmitCountMin = 150,
                EmitCountMax = 150,
                LifespanMinSeconds = 2f,
                LifespanMaxSeconds = 2f,
                SpeedPPSMin = 100f,
                SpeedPPSMax = 100f,
                Effects = new List<IParticuleEffect>
                {
                    new ColorEffect(Color.Green, Color.Red),
                    new SizeEffect(8, 32),
                    new OpacityEffect(1f, 0f),
                }
            };
            //_circleEmitter.Paused = true;



            //Circle emitter
            _shotEmitter = Add(new ParticuleEmitter());
            _shotEmitter.TranslateTo(200, 200);


        }

        /// <summary>
        /// Update each frame
        /// </summary>
        protected override void Update()
        {
            _mouseCursor.Location = this.Input.GetMousePosition();

            _mouseEmitter.Location = this.Input.GetMousePosition();

            if (this.Input.IsMouseLeftClicked())
            {
                _shotEmitter.Emission = new EmissionData()
                {
                    Texture = GetContent<Texture2D>(ContentManager.TEXTURE_PIXEL),
                    IntervalSeconds = 0f,
                    EmitCountMin = 4,
                    EmitCountMax = 8,
                    LifespanMinSeconds = 0.2f,
                    LifespanMaxSeconds = 0.4f,
                    SpeedPPSMin = 150f,
                    SpeedPPSMax = 250f,
                    Effects = new List<IParticuleEffect>
                    {
                        new SizeEffect(2, 2, 8, 8),
                        new OpacityEffect(1f, 0f),
                    }
                };
                //_circleEmitter.Emission.Effects.Add(new ColorEffect(Color.Green, Color.Red));


                _shotEmitter.Location = this.Input.GetMousePosition();
                _shotEmitter.Emit();
            }
        }

    }
}
