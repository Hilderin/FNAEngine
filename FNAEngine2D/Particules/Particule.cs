using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FNAEngine2D.Particules
{
    /// <summary>
    /// A particule
    /// </summary>
    public class Particule: GameObject, IUpdate, IDraw
    {
        private readonly ParticuleData _particuleData;
        private readonly EmissionData _emissionData;

        public float LifespanLeft;
        public float LifespanAmount;
        public Color Color;
        public float Opacity;

        public Vector2 Scale;
        public Vector2 Origin;
        public Vector2 Direction;
        public float Rotation;

        public Texture2D Texture;
        //public Vector2 Position;


        //public bool isFinished = false;

        /// <summary>
        /// A particule
        /// </summary>
        public Particule(Vector2 location, ParticuleData particuleData, EmissionData emissionData)
        {
            _particuleData = particuleData;
            _emissionData = emissionData;

            this.Location = location;

            Texture = emissionData.Texture.Data;
            LifespanLeft = particuleData.LifespanSeconds;
            LifespanAmount = 1f;
            
            Scale = Vector2.One;
            Color = Color.White;
            Opacity = 1f;

            Origin = new Vector2(emissionData.Texture.Data.Width / 2, emissionData.Texture.Data.Height / 2);

            if (particuleData.SpeedPPS != 0)
            {
                Direction = new Vector2((float)Math.Sin(_particuleData.Angle), -(float)Math.Cos(_particuleData.Angle));
                Rotation = _particuleData.Angle;
            }
            else
            {
                Direction = Vector2.Zero;
            }


        }

        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {

        }

        /// <summary>
        /// Update the particule
        /// </summary>
        public void Update()
        {
            
            LifespanLeft -= this.ElapsedGameTimeSeconds;
            if (LifespanLeft <= 0f)
            {
                //isFinished = true;
                this.Destroy();
                return;
            }

            foreach (IParticuleEffect effect in _emissionData.Effects)
                effect.Apply(this);


            LifespanAmount = MathHelper.Clamp(LifespanLeft / _particuleData.LifespanSeconds, 0, 1);



            //float widthStart = _emissionData.WidthStart;
            //float widthEnd = _emissionData.WidthEnd;
            //float heightStart = _emissionData.HeightStart;
            //float heightEnd = _emissionData.HeightEnd;

            //if (widthStart == float.MinValue)
            //    widthStart = _emissionData.Texture.Data.Width;
            //if (widthEnd == float.MinValue)
            //    widthEnd = _emissionData.Texture.Data.Width;
            //if (heightStart == float.MinValue)
            //    heightStart = _emissionData.Texture.Data.Height;
            //if (heightEnd == float.MinValue)
            //    heightEnd = _emissionData.Texture.Data.Height;

            //float scaleWidthLerp = MathHelper.Lerp(widthEnd, widthStart, LifespanAmount);
            //float scaleHeightLerp = MathHelper.Lerp(heightEnd, heightStart, LifespanAmount);
            //Scale = new Vector2(scaleWidthLerp / _emissionData.Texture.Data.Width, scaleHeightLerp / _emissionData.Texture.Data.Height);

            this.Location += Direction * _particuleData.SpeedPPS * this.ElapsedGameTimeSeconds;

            //I update the size to be sure to draw it on screen
            this.Size = new Vector2(_emissionData.Texture.Data.Width, _emissionData.Texture.Data.Height) * this.Scale;
        }

        /// <summary>
        /// Draw the particule
        /// </summary>
        public void Draw()
        {
            DrawingContext.Draw(_emissionData.Texture.Data, this.Location, null, Color * Opacity, this.Rotation, Origin, Scale, SpriteEffects.None, this.Depth);
        }
    }
}