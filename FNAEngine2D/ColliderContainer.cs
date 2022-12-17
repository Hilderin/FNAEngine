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
        /// Collider empty?
        /// </summary>
        public bool IsEmpty { get { return _colliders.Count == 0; } }

        /// <summary>
        /// Colliders
        /// </summary>
        private List<Collider> _colliders = new List<Collider>();


        /// <summary>
        /// Ajoute un collider
        /// </summary>
        public void Add(Collider collider)
        {
            _colliders.Add(collider);
        }

        /// <summary>
        /// Retire un collider
        /// </summary>
        public void Remove(Collider collider)
        {
            _colliders.Remove(collider);
        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollision(Rectangle movingColliderNextBounds, Collider movingCollider, Type[] types)
        {
            if (_colliders.Count == 0)
                return null;

            for (int index = 0; index < _colliders.Count; index++)
            {
                if (_colliders[index] != movingCollider)
                {
                    if (types == null || types.Contains(_colliders[index].GameObject.GetType()))
                    {
                        Collision collision = CollisionHelper.GetCollision(movingColliderNextBounds, _colliders[index]);
                        if (collision != null)
                            return collision;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public List<Collision> GetCollisions(Rectangle movingColliderNextBounds, Collider movingCollider, Type[] types)
        {
            if (_colliders.Count == 0)
                return CollisionHelper.EMPTY_COLLISIONS;

            List<Collision> collisions = new List<Collision>();
            for (int index = 0; index < _colliders.Count; index++)
            {
                if (_colliders[index] != movingCollider)
                {
                    if (types == null || types.Contains(_colliders[index].GameObject.GetType()))
                    {
                        Collision collision = CollisionHelper.GetCollision(movingColliderNextBounds, _colliders[index]);
                        if (collision != null)
                            collisions.Add(collision);
                    }
                }
            }

            return collisions;
        }




    }
}
