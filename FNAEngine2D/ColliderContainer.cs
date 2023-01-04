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
        /// Cache of types
        /// </summary>
        private static Dictionary<Type, List<Type>> _cacheTypes = new Dictionary<Type, List<Type>>();


        /// <summary>
        /// Ajoute un collider
        /// </summary>
        public void Add(Collider collider)
        {
            _colliders.Add(collider);

            //Add in dictionary per type...
            List<Collider> colliders;
            foreach (Type type in GetAllTypesForGameObject(collider.GameObject))
            {
                if (!_collidersPerType.TryGetValue(type, out colliders))
                {
                    colliders = new List<Collider>();
                    _collidersPerType[type] = colliders;
                }
                colliders.Add(collider);
            }
        }

        /// <summary>
        /// Retire un collider
        /// </summary>
        public void Remove(Collider collider)
        {
            _colliders.Remove(collider);

            //Remove in dictionary per type...
            List<Collider> colliders;
            foreach (Type type in GetAllTypesForGameObject(collider.GameObject))
            {
                if (_collidersPerType.TryGetValue(type, out colliders))
                {
                    colliders.Remove(collider);
                }
            }
        }


        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollision(Collider movingCollider, Type[] types)
        {
            if (_colliders.Count == 0)
                return null;


            Collision collision = null;
            if (types == null)
            {
                //All types...
                GetCollision(movingCollider, _colliders, ref collision);
            }
            else
            {
                //For some types...
                List<Collider> colliders;
                for (int index = 0; index < types.Length; index++)
                {
                    if (_collidersPerType.TryGetValue(types[index], out colliders))
                    {
                        GetCollision(movingCollider, colliders, ref collision);
                    }
                }
            }


            return collision;
        }

        /// <summary>
        /// Get all types for a type
        /// </summary>
        private List<Type> GetAllTypesForGameObject(GameObject gameObject)
        {
            List<Type> types;

            Type type = gameObject.GetType();

            if (_cacheTypes.TryGetValue(type, out types))
                return types;


            types = new List<Type>();

            LoadAllTypesForType(type, types);

            _cacheTypes[type] = types;

            return types;

        }

        /// <summary>
        /// Load all types for a type
        /// </summary>
        private void LoadAllTypesForType(Type type, List<Type> types)
        {
            if (type == typeof(GameObject))
                return;

            if (types.Contains(type))
                return;


            types.Add(type);

            if (type.BaseType != null)
                LoadAllTypesForType(type.BaseType, types);

            foreach (Type interfaceType in type.GetInterfaces())
                LoadAllTypesForType(interfaceType, types);
        }


        /// <summary>
        /// Get collision from a list of colliders
        /// </summary>
        private void GetCollision(Collider movingCollider, List<Collider> colliders, ref Collision collision)
        {

            for (int index = 0; index < colliders.Count; index++)
            {
                //Not ourself.
                if (colliders[index] != movingCollider)
                {
                    //Little check to skip the call to the collider may be colliding... if not, we will skip immediately
                    if (VectorHelper.Intersects(movingCollider.MovingLocation, movingCollider.Size, colliders[index].Location, colliders[index].Size))
                    {
                        CollisionHelper.GetCollision(movingCollider, colliders[index], ref collision);
                    }
                }
            }

        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollisionTravel(Collider movingCollider, Type[] types)
        {
            if (_colliders.Count == 0)
                return null;

            Collision collision = null;
            if (types == null)
            {
                //All types...
                GetCollisionTravel(movingCollider, _colliders, ref collision);
            }
            else
            {
                //For some types...
                List<Collider> colliders;
                for (int index = 0; index < types.Length; index++)
                {
                    if (_collidersPerType.TryGetValue(types[index], out colliders))
                    {
                        GetCollisionTravel(movingCollider, colliders, ref collision);
                    }
                }
            }


            return collision;
        }

        /// <summary>
        /// Get collision from a list of colliders
        /// </summary>
        private void GetCollisionTravel(Collider movingCollider, List<Collider> colliders, ref Collision collision)
        {

            for (int index = 0; index < colliders.Count; index++)
            {
                if (colliders[index] != movingCollider)
                {
                    CollisionHelper.GetCollisionTravel(movingCollider, colliders[index], ref collision);
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
