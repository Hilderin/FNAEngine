using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FNAEngine2D.Particules
{
    /// <summary>
    /// A particule
    /// </summary>
    public class Particle: GameObject
    {
        private readonly ParticleData _particuleData;
        private readonly EmissionData _emissionData;
        private Vector2 _position;
        private float _lifespanLeft;
        private float _lifespanAmount;
        private Color _color;
        private float _opacity;
        public bool isFinished = false;
        private Vector2 _scale;
        private Vector2 _origin;
        private Vector2 _direction;

        /// <summary>
        /// A particule
        /// </summary>
        public Particle(Vector2 pos, ParticleData particuleData, EmissionData emissionData)
        {
            _particuleData = particuleData;
            _emissionData = emissionData;

            _lifespanLeft = particuleData.LifespanSeconds;
            _lifespanAmount = 1f;
            _position = pos;
            _color = emissionData.ColorStart;
            _opacity = emissionData.OpacityStart;
            _origin = new Vector2(emissionData.Texture.Data.Width / 2, emissionData.Texture.Data.Height / 2);

            if (particuleData.SpeedPPS != 0)
            {
                _direction = new Vector2((float)Math.Sin(_particuleData.Angle), -(float)Math.Cos(_particuleData.Angle));
            }
            else
            {
                _direction = Vector2.Zero;
            }
        }

        /// <summary>
        /// Update the particule
        /// </summary>
        protected override void Update()
        {
            _lifespanLeft -= this.ElapsedGameTimeSeconds;
            if (_lifespanLeft <= 0f)
            {
                isFinished = true;
                this.Destroy();
                return;
            }

            _lifespanAmount = MathHelper.Clamp(_lifespanLeft / _particuleData.LifespanSeconds, 0, 1);
            _color = Color.Lerp(_emissionData.ColorEnd, _emissionData.ColorStart, _lifespanAmount);
            _opacity = MathHelper.Clamp(MathHelper.Lerp(_emissionData.OpacityEnd, _emissionData.OpacityStart, _lifespanAmount), 0, 1);
            
            float scaleLerp = MathHelper.Lerp(_emissionData.SizeEnd, _emissionData.SizeStart, _lifespanAmount);
            _scale = new Vector2(scaleLerp / _emissionData.Texture.Data.Width, scaleLerp / _emissionData.Texture.Data.Height);
            _position += _direction * _particuleData.SpeedPPS * this.ElapsedGameTimeSeconds;
        }

        /// <summary>
        /// Draw the particule
        /// </summary>
        protected override void Draw()
        {
            DrawingContext.Draw(_emissionData.Texture.Data, _position, null, _color * _opacity, 0f, _origin, _scale, SpriteEffects.None, 1f);
        }
    }
}