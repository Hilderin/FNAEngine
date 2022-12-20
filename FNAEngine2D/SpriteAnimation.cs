using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Sprite animation
    /// </summary>
    public class SpriteAnimation
    {
        /// <summary>
        /// Sprite
        /// </summary>
        private Content<Sprite> _sprite;

        /// <summary>
        /// SpriteName
        /// </summary>
        private string _spriteName;

        /// <summary>
        /// Frames
        /// </summary>
        public SpriteAnimationFrame[] Frames { get; set; }

        /// <summary>
        /// Sprite
        /// </summary>
        public Sprite Sprite { get { return _sprite.Data; } }

        /// <summary>
        /// SpriteName
        /// </summary>
        public string SpriteName
        {
            get { return _spriteName; }
            set
            {
                if (_spriteName != value)
                {
                    _spriteName = value;

                    //Loading Sprite...
                    _sprite = GameHost.GetContent<Sprite>(value);
                }
            }
        }

        /// <summary>
        /// Set manually the sprite (without hot reload)
        /// </summary>
        public void SetSprite(Sprite sprite)
        {
            _sprite = new Content<Sprite>(sprite);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteAnimation()
        {
            
        }

    }
}
