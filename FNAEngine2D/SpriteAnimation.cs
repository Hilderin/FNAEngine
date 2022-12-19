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
        public Sprite Sprite { get; set; }

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
                    this.Sprite = GameHost.GetContent<Sprite>(value);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteAnimation()
        {
            ContentHelper.ContentChanged += ContentManager_ContentChanged;
        }

        /// <summary>
        /// Changement du content
        /// </summary>
        private void ContentManager_ContentChanged(string assetName)
        {
            if (!String.IsNullOrEmpty(this.SpriteName) && this.SpriteName.Equals(assetName, StringComparison.OrdinalIgnoreCase))
                this.Sprite = GameHost.GetContent<Sprite>(this.SpriteName);
        }

    }
}
