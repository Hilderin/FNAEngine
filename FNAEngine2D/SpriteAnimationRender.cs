using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private int _currentFrame = 0;

        /// <summary>
        /// ElapsedTime
        /// </summary>
        private float _elapsedTime = 0f;

        /// <summary>
        /// Ended
        /// </summary>
        private bool _stopped = false;

        /// <summary>
        /// Sprite animation
        /// </summary>
        private Content<SpriteAnimation> _spriteAnimation;

        /// <summary>
        /// Play the animation on start
        /// </summary>
        private bool _playOnStart = true;


        /// <summary>
        /// Information on the sprite animation
        /// </summary>
        public string SpriteAnimationName { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loop the animation
        /// </summary>
        [DefaultValue(true)]
        public bool Loop { get; set; } = true;
                /// <summary>
        /// Hide if animation is stopped?
        /// </summary>
        [DefaultValue(false)]
        public bool HideOnStop { get; set; } = false;

        /// <summary>
        /// Inverted on the X axis
        /// </summary>
        [DefaultValue(false)]
        public bool InvertedX { get; set; } = false;

        /// <summary>
        /// Play the animation on start
        /// </summary>
        [DefaultValue(true)]
        public bool PlayOnStart
        {
            get { return _playOnStart; }
            set
            {
                if (_playOnStart != value)
                {
                    _playOnStart = value;

                    //If not started.... we stop it
                    if (!value)
                    {
                        if (_elapsedTime == 0)
                            Stop();
                    }
                }
            }
        }


        /// <summary>
        /// Animation ended?
        /// </summary>
        [JsonIgnore]
        public bool Ended { get { return _stopped; } }


        /// <summary>
        /// Empty constructor
        /// </summary>
        public SpriteAnimationRender()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteAnimationRender(string spriteAnimationName): this()
        {
            this.SpriteAnimationName = spriteAnimationName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteAnimationRender(string spriteAnimationName, bool loop, bool playOnStart, bool hideOnStop) : this(spriteAnimationName)
        {
            this.Loop = loop;
            this.PlayOnStart = playOnStart;
            this.HideOnStop = hideOnStop;
        }

        /// <summary>
        /// Start the animation
        /// </summary>
        public void Play()
        {
            if (_stopped)
                Restart();
        }

        /// <summary>
        /// Restart the animation
        /// </summary>
        public void Restart()
        {
            _currentFrame = 0;
            _elapsedTime = 0;
            _stopped = false;

            //Force drawing the first frame...
            UpdateInternal(0);
        }

        /// <summary>
        /// Stop the current animation
        /// </summary>
        public void Stop()
        {
            _stopped = true;

        }

        /// <summary>
        /// Loading...
        /// </summary>
        public override void Load()
        {
            if (String.IsNullOrEmpty(this.SpriteAnimationName))
            {
                _spriteAnimation = null;
                _currentFrame = -1;
            }
            else
            {
                _spriteAnimation = GameHost.GetContent<SpriteAnimation>(this.SpriteAnimationName);

                if (this.Width == 0)
                    this.Width = _spriteAnimation.Data.Sprite.ColumnScreenWidth;
                if (this.Height == 0)
                    this.Height = _spriteAnimation.Data.Sprite.RowScreenHeight;

                if (_spriteAnimation.Data.Frames.Length == 0 || _spriteAnimation.Data.Sprite == null || _spriteAnimation.Data.Sprite.Texture == null)
                {
                    _currentFrame = -1;
                    return;
                }

                if (!PlayOnStart)
                {
                    _stopped = true;
                }
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            UpdateInternal(GameHost.ElapsedGameTimeMilliseconds);

        }


        /// <summary>
        /// Update
        /// </summary>
        private void UpdateInternal(float elapsedGameTimeMilliseconds)
        {
            if (_stopped)
                return;

            //Little validations...
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

            float newTime = _elapsedTime + elapsedGameTimeMilliseconds;

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
                {
                    if (this.Loop)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        _currentFrame = spriteAnimation.Frames.Length - 1;
                        _stopped = true;
                        break;
                    }
                }

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
            if (_currentFrame < 0 || (_stopped && HideOnStop))
                return;

            SpriteAnimation spriteAnimation = _spriteAnimation.Data;

            SpriteEffects spriteEffects = SpriteEffects.None;

            if (InvertedX)
                spriteEffects = SpriteEffects.FlipHorizontally;

            DrawingContext.Draw(spriteAnimation.Sprite.Texture, this.Bounds, new Rectangle(spriteAnimation.Frames[_currentFrame].ColumnIndex * spriteAnimation.Sprite.ColumnWidth, spriteAnimation.Frames[_currentFrame].RowIndex * spriteAnimation.Sprite.RowHeight, spriteAnimation.Sprite.ColumnWidth, spriteAnimation.Sprite.RowHeight), this.Color, 0f, Vector2.Zero, spriteEffects, this.Depth);
            
        }

    }
}
