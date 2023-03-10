using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAEngine2D.Animations
{
    
    public class SpriteAnimator : Component, IUpdate, IDraw
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
        /// Bounds of the sprites
        /// </summary>
        private Rectangle[] _frameBounds;

        /// <summary>
        /// Play the animation on start
        /// </summary>
        private bool _playOnStart = true;

        /// <summary>
        /// Scale
        /// </summary>
        private Vector2 _scale = Vector2.One;

        /// <summary>
        /// StartPosition
        /// </summary>
        private StartPosition _startPosition = StartPosition.TopLeft;


        /// <summary>
        /// Width
        /// </summary>
        private float _width;

        /// <summary>
        /// Height
        /// </summary>
        private float _height;

        /// <summary>
        /// Location
        /// </summary>
        private Vector2 _location;


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
        public SpriteAnimator()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteAnimator(string spriteAnimationName): this()
        {
            this.SpriteAnimationName = spriteAnimationName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteAnimator(string spriteAnimationName, StartPosition startPosition) : this(spriteAnimationName)
        {
            _startPosition = startPosition;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteAnimator(string spriteAnimationName, bool loop, bool playOnStart, bool hideOnStop) : this(spriteAnimationName)
        {
            this.Loop = loop;
            this.PlayOnStart = playOnStart;
            this.HideOnStop = hideOnStop;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteAnimator(string spriteAnimationName, bool loop, bool playOnStart, bool hideOnStop, StartPosition startPosition) : this(spriteAnimationName, loop, playOnStart, hideOnStop)
        {
            _startPosition = startPosition;
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
        protected override void Load()
        {
            if (String.IsNullOrEmpty(this.SpriteAnimationName))
            {
                _spriteAnimation = null;
                _currentFrame = -1;
            }
            else
            {
                _spriteAnimation = GetContent<SpriteAnimation>(this.SpriteAnimationName);

                _width = _spriteAnimation.Data.Sprite.ColumnScreenWidth;
                _height = _spriteAnimation.Data.Sprite.RowScreenHeight;

                ////Adjust the starting position...
                //if (_startPosition == StartPosition.CenterBottom)
                //        this.Bounds = this.Parent.Bounds.CenterBottom(_width, _height);
                //    else if (_startPosition == StartPosition.TopLeft)
                //        this.Location = this.Parent.Location;
                //}

                RecalculateLocation();
                RecalculateScale();

                if (_spriteAnimation.Data.Frames.Length == 0 || _spriteAnimation.Data.Sprite == null || _spriteAnimation.Data.Sprite.Texture == null)
                {
                    _currentFrame = -1;
                    return;
                }

                //Initisalisation of the rectangles for each frames...
                _frameBounds = new Rectangle[_spriteAnimation.Data.Frames.Length];
                for (int index = 0; index < _spriteAnimation.Data.Frames.Length; index++)
                {
                    _frameBounds[index] = new Rectangle(_spriteAnimation.Data.Frames[index].ColumnIndex * _spriteAnimation.Data.Sprite.ColumnWidth, _spriteAnimation.Data.Frames[index].RowIndex * _spriteAnimation.Data.Sprite.RowHeight, _spriteAnimation.Data.Sprite.ColumnWidth, _spriteAnimation.Data.Sprite.RowHeight);
                }

                if (!PlayOnStart)
                {
                    _stopped = true;
                }
            }

        }

        /// <summary>
        /// On resized...
        /// </summary>
        protected override void OnResized()
        {
            RecalculateScale();
        }

        /// <summary>
        /// On removed...
        /// </summary>
        protected override void OnMoved()
        {
            RecalculateLocation();
        }


        /// <summary>
        /// Recalculate the scale
        /// </summary>
        private void RecalculateScale()
        {
            if (_spriteAnimation == null || _spriteAnimation.Data.Sprite == null || _spriteAnimation.Data.Sprite.ColumnScreenWidth == 0 || _spriteAnimation.Data.Sprite.RowScreenHeight == 0)
            {
                _scale = Vector2.Zero;
            }
            else
            {
                _scale = new Vector2(_width / _spriteAnimation.Data.Sprite.ColumnScreenWidth, _height / _spriteAnimation.Data.Sprite.RowScreenHeight);
            }
        }

        /// <summary>
        /// Recalculate the scale
        /// </summary>
        private void RecalculateLocation()
        {
            switch (_startPosition)
            {
                case StartPosition.CenterBottom:
                    _location = VectorHelper.CenterBottom(this.GameObject.Location, this.GameObject.Size, _width, _height);
                    break;
                case StartPosition.CenterMiddle:
                    _location = VectorHelper.CenterMiddle(this.GameObject.Location, this.GameObject.Size, _width, _height);
                    break;
                default:
                    //Top left...
                    _location = this.GameObject.Location;
                    break;

            }
        }


        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            UpdateInternal(this.ElapsedGameTimeMilliseconds);

        }

        /// <summary>
        /// Update
        /// </summary>
        private void UpdateInternal(float elapsedGameTimeMilliseconds)
        {
            if (_stopped || _spriteAnimation == null)
                return;

            //Little validations...
            SpriteAnimation spriteAnimation = _spriteAnimation.Data;
           
            if (spriteAnimation == null || spriteAnimation.Frames.Length == 0 || spriteAnimation.Sprite == null || spriteAnimation.Sprite.Texture == null)
            {
                _currentFrame = -1;
                return;
            }

            //Only one frame?
            if (spriteAnimation.Frames.Length == 1)
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
        public void Draw()
        {
            if (_currentFrame < 0 || (_stopped && HideOnStop))
                return;

            SpriteAnimation spriteAnimation = _spriteAnimation.Data;

            SpriteEffects spriteEffects = SpriteEffects.None;

            if (InvertedX)
                spriteEffects = SpriteEffects.FlipHorizontally;

            DrawingContext.Draw(spriteAnimation.Sprite.Texture, _location, _frameBounds[_currentFrame], this.Color, 0f, Vector2.Zero, _scale, spriteEffects, this.GameObject.Depth);

        }

    }
}
