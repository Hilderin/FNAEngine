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
        public Collision GetCollision(Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider movingCollider, Type[] types)
        {
            if (_colliders.Count == 0)
                return null;

            Collision ret = null;

            for (int index = 0; index < _colliders.Count; index++)
            {
                if (_colliders[index] != movingCollider)
                {
                    if (types == null || types.Contains(_colliders[index].GameObject.GetType()))
                    {
                        CollisionHelper.GetCollision(movingColliderLocation, movingColliderSize, _colliders[index], ref ret);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollisionTravel(Vector2 movingColliderOriginLocation, Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider movingCollider, Type[] types)
        {
            if (_colliders.Count == 0)
                return null;

            Collision ret = null;

            for (int index = 0; index < _colliders.Count; index++)
            {
                if (_colliders[index] != movingCollider)
                {
                    if (types == null || types.Contains(_colliders[index].GameObject.GetType()))
                    {
                        CollisionHelper.GetCollisionTravel(movingColliderOriginLocation, movingColliderLocation, movingColliderSize, _colliders[index], ref ret);
                    }
                }
            }

            return ret;
        }

        ///// <summary>
        ///// Permet d'obtenir la liste des collisions
        ///// </summary>
        //public List<Collision> GetCollisions(Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider movingCollider, Type[] types)
        //{
        //    if (_colliders.Count == 0)
        //        return CollisionHelper.EMPTY_COLLISIONS;

        //    List<Collision> collisions = new List<Collision>();
        //    for (int index = 0; index < _colliders.Count; index++)
        //    {
        //        if (_colliders[index] != movingCollider)
        //        {
        //            if (types == null || types.Contains(_colliders[index].GameObject.GetType()))
        //            {
        //                Collision collision = CollisionHelper.GetCollision(movingColliderLocation, movingColliderSize, _colliders[index]);
        //                if (collision != null)
        //                    collisions.Add(collision);
        //            }
        //        }
        //    }

        //    return collisions;
        //}




    }
}
