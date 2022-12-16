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
        public List<ColliderRectangle> Colliders { get; private set; } = new List<ColliderRectangle>();

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public IEnumerable<Collision> GetCollisions(Rectangle movingColliderNextBounds, ColliderRectangle movingCollider)
        {
            for (int index = 0; index < this.Colliders.Count; index++)
            {
                if (this.Colliders[index] != movingCollider)
                {
                    CollisionDirection direction = CollisionHelper.GetCollision(movingColliderNextBounds, this.Colliders[index].Bounds);
                    if(direction != CollisionDirection.None)
                        yield return new Collision(this.Colliders[index], direction);
                }
            }
        }

        


    }
}
