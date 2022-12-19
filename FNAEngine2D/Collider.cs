using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        /// Location
        /// </summary>
        public Vector2 Location { get { return this.GameObject.Location; } }

        /// <summary>
        /// Location
        /// </summary>
        public Vector2 Size { get { return this.GameObject.Size; } }

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
            this.GameObject = new GameObject();
            this.GameObject.Bounds = bounds;
        }

        /// <summary>
        /// Collider sous forme de rectangle
        /// </summary>
        public Collider(int x, int y, int width, int height): this(new Rectangle(x, y, width, height))
        {
            
        }

        
        /// <summary>
        /// Permet de savoir si le rectangle collider avec ce collider
        /// </summary>
        public bool Intersects(Vector2 location, Vector2 size)
        {
            return VectorHelper.Intersects(this.Location, this.Size, location, size);
        }

    }
}
