using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public class SpriteAnimationRender : GameObject
    {
        /// <summary>
        /// CurrentFrame
        /// </summary>
        private int _currentFrame = -1;

        /// <summary>
        /// ElapsedTime
        /// </summary>
        private float _elapsedTime = 0f;

        /// <summary>
        /// Sprite animation
        /// </summary>
        private Content<SpriteAnimation> _spriteAnimation;

        /// <summary>
        /// Information on the sprite animation
        /// </summary>
        public string SpriteAnimationName { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; } = Color.White;
               

        /// <summary>
        /// Empty constructor
        /// </summary>
        public SpriteAnimationRender()
        {
            
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public SpriteAnimationRender(string spriteAnimationName): this()
        {
            this.SpriteAnimationName = spriteAnimationName;
        }

        /// <summary>
        /// Restart the animation
        /// </summary>
        public void Restart()
        {
            _currentFrame = -1;
        }

        /// <summary>
        /// Loading...
        /// </summary>
        public override void Load()
        {
            if (String.IsNullOrEmpty(this.SpriteAnimationName))
            {
                _spriteAnimation = null;
            }
            else
            {
                _spriteAnimation = GameHost.GetContent<SpriteAnimation>(this.SpriteAnimationName);

                if (this.Width == 0)
                    this.Width = _spriteAnimation.Data.Sprite.ColumnScreenWidth;
                if (this.Height == 0)
                    this.Height = _spriteAnimation.Data.Sprite.RowScreenHeight;
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            //Littre validations...
            SpriteAnimation spriteAnimation = _spriteAnimation.Data;

            if (spriteAnimation == null || spriteAnimation.Frames.Length == 0 || spriteAnimation.Sprite == null || spriteAnimation.Sprite.Texture == null)
            {
                _currentFrame = -1;
                return;
            }

            //Only one frame?
            if (spriteAnimation.Frames.Length == 0)
            {
                _currentFrame = 0;
                return;
            }

            float newTime = _elapsedTime + GameHost.ElapsedGameTimeMilliseconds;

            //First frame?
            if (_currentFrame < 0)
            {
                _currentFrame = 0;
                newTime = 0;
            }

            //We could skip frames...
            while (newTime >= spriteAnimation.Frames[_currentFrame].Duration)
            {
                //Next frame...
                _currentFrame++;
                if (_currentFrame >= spriteAnimation.Frames.Length)
                    _currentFrame = 0;

                //We remove the duration used on this frame...
                newTime -= spriteAnimation.Frames[_currentFrame].Duration;
            }

            _elapsedTime = newTime;


        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            if (_currentFrame < 0)
                return;

            SpriteAnimation spriteAnimation = _spriteAnimation.Data;

            DrawingContext.Draw(spriteAnimation.Sprite.Texture, this.Bounds, new Rectangle(spriteAnimation.Frames[_currentFrame].ColumnIndex * spriteAnimation.Sprite.ColumnWidth, spriteAnimation.Frames[_currentFrame].RowIndex * spriteAnimation.Sprite.RowHeight, spriteAnimation.Sprite.ColumnWidth, spriteAnimation.Sprite.RowHeight), this.Color, 0f, Vector2.Zero, SpriteEffects.None, this.Depth);
            
        }

    }
}
