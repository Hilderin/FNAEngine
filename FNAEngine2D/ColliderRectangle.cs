using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Collider
    /// </summary>
    public class ColliderRectangle : Collider
    {

        /// <summary>
        /// Size
        /// </summary>
        public virtual Vector2 Size { get { return this.GameObject.Size; } }


        /// <summary>
        /// Collider rectangle form
        /// </summary>
        public ColliderRectangle()
        {
        }

        /// <summary>
        /// Collider rectangle form
        /// </summary>
        public ColliderRectangle(GameObject gameObject)
        {
            this.GameObject = gameObject;
        }

        /// <summary>
        /// Collider rectangle form
        /// </summary>
        public ColliderRectangle(Rectangle bounds)
        {
            this.GameObject = new GameObject();
            this.GameObject.Bounds = bounds;
        }

        /// <summary>
        /// Collider rectangle form
        /// </summary>
        public ColliderRectangle(int x, int y, int width, int height) : this(new Rectangle(x, y, width, height))
        {

        }


        /// <summary>
        /// Check if the collider intersects with a rectangle
        /// </summary>
        public override bool Intersects(Collider movingCollider, out CollisionDirection direction, out Vector2 hitLocation)
        {
            if (movingCollider is ColliderRectangle)
            {
                //With another rectangle...
                return CollisionHelper.Intersects(movingCollider.Location, ((ColliderRectangle)movingCollider).Size, this.Location, this.Size, out direction, out hitLocation);
            }
            else if (movingCollider is ColliderCircle)
            {
                //With a circle..
                if (CollisionHelper.Intersects(((ColliderCircle)movingCollider).CenterMovingLocation, ((ColliderCircle)movingCollider).Radius, this.Location, this.Size, out direction, out hitLocation))
                {
                    hitLocation -= ((ColliderCircle)movingCollider).CenterOffset;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                throw new NotSupportedException("Collider type not supported: " + movingCollider.GetType().FullName);
            }
        }


    }
}
