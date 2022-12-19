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
        private SpriteAnimation _spriteAnimation;

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
            ContentHelper.ContentChanged += ContentManager_ContentChanged;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public SpriteAnimationRender(string spriteAnimationName): this()
        {
            this.SpriteAnimationName = spriteAnimationName;
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
                    this.Width = _spriteAnimation.Sprite.TileScreenWidth;
                if (this.Height == 0)
                    this.Height = _spriteAnimation.Sprite.TileScreenHeight;
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            //Littre validations...
            if (_spriteAnimation == null || _spriteAnimation.Frames.Length == 0 || _spriteAnimation.Sprite == null || _spriteAnimation.Sprite.Texture == null)
            {
                _currentFrame = -1;
                return;
            }

            //Only one frame?
            if (_spriteAnimation.Frames.Length == 0)
            {
                _currentFrame = 0;
                return;
            }

            float newTime = _elapsedTime + GameHost.ElapsedGameTimeMilliseconds;

            //First frame?
            if (_currentFrame < 0)
                _currentFrame = 0;

            //We could skip frames...
            while (newTime >= _spriteAnimation.Frames[_currentFrame].Duration)
            {
                //Next frame...
                _currentFrame++;
                if (_currentFrame >= _spriteAnimation.Frames.Length)
                    _currentFrame = 0;

                //We remove the duration used on this frame...
                newTime -= _spriteAnimation.Frames[_currentFrame].Duration;
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

            GameHost.SpriteBatch.Draw(_spriteAnimation.Sprite.Texture, this.Bounds, new Rectangle(_spriteAnimation.Frames[_currentFrame].SpriteX * _spriteAnimation.Sprite.TileWidth, _spriteAnimation.Frames[_currentFrame].SpriteY * _spriteAnimation.Sprite.TileHeight, _spriteAnimation.Sprite.TileWidth, _spriteAnimation.Sprite.TileHeight), this.Color);

        }


        /// <summary>
        /// Changement du content
        /// </summary>
        private void ContentManager_ContentChanged(string assetName)
        {
            if (!String.IsNullOrEmpty(this.SpriteAnimationName) && this.SpriteAnimationName.Equals(assetName, StringComparison.OrdinalIgnoreCase))
                _spriteAnimation = GameHost.GetContent<SpriteAnimation>(this.SpriteAnimationName);
        }
    }
}
