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
    public class ColliderRectangle
    {

        /// <summary>
        /// Bounds
        /// </summary>
        public Rectangle Bounds { get { return this.GameObject.Bounds; } }

        /// <summary>
        /// GameObject associé au collider
        /// </summary>
        public GameObject GameObject;

        /// <summary>
        /// Collider sous forme de rectangle
        /// </summary>
        public ColliderRectangle(GameObject gameObject)
        {
            this.GameObject = gameObject;
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
