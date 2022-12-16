using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Collider
    /// </summary>
    public class Collider
    {

        /// <summary>
        /// Bounds redéfini
        /// </summary>
        private Rectangle? _bounds = null;

        /// <summary>
        /// Bounds
        /// </summary>
        public Rectangle Bounds
        { 
            get 
            {
                if (_bounds != null)
                    return _bounds.Value;

                return this.GameObject.Bounds;
            } 
        }

        /// <summary>
        /// GameObject associé au collider
        /// </summary>
        public GameObject GameObject;

        /// <summary>
        /// Collider sous forme de rectangle
        /// </summary>
        public Collider(GameObject gameObject)
        {
            this.GameObject = gameObject;
        }

        /// <summary>
        /// Collider sous forme de rectangle
        /// </summary>
        public Collider(Rectangle bounds)
        {
            _bounds = bounds;
        }

        /// <summary>
        /// Collider sous forme de rectangle
        /// </summary>
        public Collider(int x, int y, int width, int height)
        {
            _bounds = new Rectangle(x, y, width, height);
        }

        
        /// <summary>
        /// Permet de savoir si le rectangle collider avec ce collider
        /// </summary>
        public bool Intersects(Rectangle rectangle)
        {
            return rectangle.Intersects(this.Bounds);
        }

    }
}
