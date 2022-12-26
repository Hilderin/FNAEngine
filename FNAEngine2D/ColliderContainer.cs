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
        /// Colliders
        /// </summary>
        private Dictionary<Type, List<Collider>> _collidersPerType = new Dictionary<Type, List<Collider>>();


        /// <summary>
        /// Ajoute un collider
        /// </summary>
        public void Add(Collider collider)
        {
            _colliders.Add(collider);

            //Add in dictionary per type...
            List<Collider> colliders;
            if (!_collidersPerType.TryGetValue(collider.GameObject.GetType(), out colliders))
            {
                colliders = new List<Collider>();
                _collidersPerType[collider.GameObject.GetType()] = colliders;
            }
            colliders.Add(collider);
        }

        /// <summary>
        /// Retire un collider
        /// </summary>
        public void Remove(Collider collider)
        {
            _colliders.Remove(collider);

            //Remove in dictionary per type...
            List<Collider> colliders;
            if (_collidersPerType.TryGetValue(collider.GameObject.GetType(), out colliders))
            {
                colliders.Remove(collider);
            }
        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollision(Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider movingCollider, Type[] types)
        {
            if (_colliders.Count == 0)
                return null;


            Collision collision = null;
            if (types == null)
            {
                //All types...
                GetCollision(movingColliderLocation, movingColliderSize, movingCollider, _colliders, ref collision);
            }
            else
            {
                //For some types...
                List<Collider> colliders;
                for (int index = 0; index < types.Length; index++)
                {
                    if (_collidersPerType.TryGetValue(types[index], out colliders))
                    {
                        GetCollision(movingColliderLocation, movingColliderSize, movingCollider, colliders, ref collision);
                    }
                }
            }


            return collision;
        }

        /// <summary>
        /// Get collision from a list of colliders
        /// </summary>
        private void GetCollision(Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider movingCollider, List<Collider> colliders, ref Collision collision)
        {

            for (int index = 0; index < colliders.Count; index++)
            {
                if (colliders[index] != movingCollider)
                {
                    CollisionHelper.GetCollision(movingColliderLocation, movingColliderSize, colliders[index], ref collision);
                }
            }

        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollisionTravel(Vector2 movingColliderOriginLocation, Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider movingCollider, Type[] types)
        {
            if (_colliders.Count == 0)
                return null;

            Collision collision = null;
            if (types == null)
            {
                //All types...
                GetCollisionTravel(movingColliderOriginLocation, movingColliderLocation, movingColliderSize, movingCollider, _colliders, ref collision);
            }
            else
            {
                //For some types...
                List<Collider> colliders;
                for (int index = 0; index < types.Length; index++)
                {
                    if (_collidersPerType.TryGetValue(types[index], out colliders))
                    {
                        GetCollisionTravel(movingColliderOriginLocation, movingColliderLocation, movingColliderSize, movingCollider, colliders, ref collision);
                    }
                }
            }


            return collision;
        }

        /// <summary>
        /// Get collision from a list of colliders
        /// </summary>
        private void GetCollisionTravel(Vector2 movingColliderOriginLocation, Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider movingCollider, List<Collider> colliders, ref Collision collision)
        {

            for (int index = 0; index < colliders.Count; index++)
            {
                if (colliders[index] != movingCollider)
                {
                    CollisionHelper.GetCollisionTravel(movingColliderOriginLocation, movingColliderLocation, movingColliderSize, colliders[index], ref collision);
                }
            }

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
