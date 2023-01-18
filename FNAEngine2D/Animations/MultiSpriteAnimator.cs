using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Animations
{
    /// <summary>
    /// Sprite animator
    /// </summary>
    public class MultiSpriteAnimator<T> : GameObject where T : System.Enum
    {
        /// <summary>
        /// Animations of the character
        /// </summary>
        private Dictionary<T, SpriteAnimator> _animations = new Dictionary<T, SpriteAnimator>();

        /// <summary>
        /// Current animation
        /// </summary>
        private SpriteAnimator _currentAnimation;

        /// <summary>
        /// Loop the animation
        /// </summary>
        private bool _loop = true;

        /// <summary>
        /// Play the animation on start
        /// </summary>
        private bool _playOnStart = true;

        /// <summary>
        /// Hide if animation is stopped?
        /// </summary>
        private bool _hideOnStop = true;

        /// <summary>
        /// Inverted on the X axis
        /// </summary>
        private bool _invertedX = false;

        /// <summary>
        /// Animation folder
        /// </summary>
        public string AnimationFolder { get; set; }

        /// <summary>
        /// Return the current animation
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public T CurrentAnimation { get; set; }

        /// <summary>
        /// Loop the animation
        /// </summary>
        [DefaultValue(true)]
        public bool Loop
        {
            get { return _loop; }
            set
            {
                if (_loop != value)
                {
                    _loop = value;

                    //End we change the animations...
                    foreach (SpriteAnimator SpriteAnimator in _animations.Values)
                        SpriteAnimator.Loop = value;
                }
            }
        }

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

                    //End we change the animations...
                    foreach (SpriteAnimator SpriteAnimator in _animations.Values)
                        SpriteAnimator.PlayOnStart = value;
                }
            }
        }

        /// <summary>
        /// Hide if animation is stopped?
        /// </summary>
        [DefaultValue(false)]
        public bool HideOnStop
        {
            get { return _hideOnStop; }
            set
            {
                if (_hideOnStop != value)
                {
                    _hideOnStop = value;

                    //End we change the animations...
                    foreach (SpriteAnimator SpriteAnimator in _animations.Values)
                        SpriteAnimator.HideOnStop = value;
                }
            }
        }

        /// <summary>
        /// Inverted on the X axis
        /// </summary>
        [DefaultValue(false)]
        public bool InvertedX
        {
            get { return _invertedX; }
            set
            {
                if (_invertedX != value)
                {
                    _invertedX = value;

                    //End we change the animations...
                    foreach (SpriteAnimator SpriteAnimator in _animations.Values)
                        SpriteAnimator.InvertedX = value;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiSpriteAnimator(string animationFolder)
        {
            this.AnimationFolder = animationFolder;

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiSpriteAnimator(string animationFolder, bool loop, bool playOnStart, bool hideOnStop): this(animationFolder)
        {
            _loop = loop;
            _playOnStart = playOnStart;
            _hideOnStop = hideOnStop;
        }


        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            if(this.Parent != null)
                this.Bounds = this.Parent.Bounds;

            LoadAnimations();

            //Play the first animation...
            if(_playOnStart)
                Play((T)Enum.GetValues(typeof(T)).GetValue(0));
        }


        /// <summary>
        /// Play an animation
        /// </summary>
        public void Play(T animation)
        {
            SpriteAnimator SpriteAnimator = _animations[animation];

            if (SpriteAnimator != _currentAnimation)
            {
                if (_currentAnimation != null)
                    RemoveComponent(_currentAnimation);

                AddComponent(SpriteAnimator);
                //SpriteAnimator.Bounds = this.Bounds.CenterBottom(SpriteAnimator.Width, SpriteAnimator.Height);
                _currentAnimation = SpriteAnimator;
            }
            
            SpriteAnimator.Restart();
            

            this.CurrentAnimation = animation;
        }

        /// <summary>
        /// Stop the current animation
        /// </summary>
        public void Stop()
        {
            if (_currentAnimation != null)
                _currentAnimation.Stop();
        }


        /// <summary>
        /// Load animations
        /// </summary>
        private void LoadAnimations()
        {
            foreach (var anim in Enum.GetValues(typeof(T)))
            {
                T characterAnimation = (T)anim;

                string assetName = Path.Combine(this.AnimationFolder, characterAnimation.ToString());

                
                SpriteAnimator SpriteAnimator = new SpriteAnimator(assetName, _loop, _playOnStart, _hideOnStop);

                //Adding and removing to load the animation...
                AddComponent(SpriteAnimator);
                RemoveComponent(SpriteAnimator);

                _animations[characterAnimation] = SpriteAnimator;
            }
        }


    }
}
