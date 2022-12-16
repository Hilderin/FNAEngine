using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Container de collider
    /// </summary>
    public class ColliderContainer
    {
        /// <summary>
        /// Colliders
        /// </summary>
        public List<Collider> Colliders { get; private set; } = new List<Collider>();

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollision(Rectangle movingColliderNextBounds, Collider movingCollider)
        {
            for (int index = 0; index < this.Colliders.Count; index++)
            {
                if (this.Colliders[index] != movingCollider)
                {
                    Collision collision = CollisionHelper.GetCollision(movingColliderNextBounds, this.Colliders[index]);
                    if (collision != null)
                        return collision;
                }
            }

            return null;
        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public List<Collision> GetCollisions(Rectangle movingColliderNextBounds, Collider movingCollider)
        {
            List<Collision> collisions = new List<Collision>();
            for (int index = 0; index < this.Colliders.Count; index++)
            {
                if (this.Colliders[index] != movingCollider)
                {
                    Collision collision = CollisionHelper.GetCollision(movingColliderNextBounds, this.Colliders[index]);
                    if (collision != null)
                        collisions.Add(collision);
                }
            }

            return collisions;
        }




    }
}
