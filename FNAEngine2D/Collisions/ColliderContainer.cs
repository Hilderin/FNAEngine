using FNAEngine2D.SpaceTrees;
using System;
using System.Collections.Generic;

namespace FNAEngine2D.Collisions
{
    /// <summary>
    /// Container de collider
    /// </summary>
    public class ColliderContainer
    {

        /// <summary>
        /// Collider empty?
        /// </summary>
        public bool IsEmpty { get { return _spaceTree.Count == 0; } }

        ///// <summary>
        ///// Colliders
        ///// </summary>
        //private List<Collider> _colliders = new List<Collider>();

        /// <summary>
        /// Space tree for the colliders
        /// </summary>
        private Space2DTree<Collider> _spaceTree = new Space2DTree<Collider>();

        ///// <summary>
        ///// Colliders
        ///// </summary>
        //private Dictionary<Type, List<Collider>> _collidersPerType = new Dictionary<Type, List<Collider>>();

        /// <summary>
        /// Cache of types
        /// </summary>
        private static Dictionary<Type, List<Type>> _cacheTypes = new Dictionary<Type, List<Type>>();


        /// <summary>
        /// Add a collider
        /// </summary>
        public void Add(Collider collider)
        {
            //_colliders.Add(collider);
            if (collider.Size.X == 0 || collider.Size.Y == 0)
                throw new InvalidOperationException("Cannot add a collider of size zero, size received: " + collider.Size);

            collider.SpaceTreeDataNode = _spaceTree.Add(collider.Location.X, collider.Location.Y, collider.Size.X, collider.Size.Y, collider);

            ////Add in dictionary per type...
            //List<Collider> colliders;
            //foreach (Type type in GetAllTypesForGameObject(collider.GameObject))
            //{
            //    if (!_collidersPerType.TryGetValue(type, out colliders))
            //    {
            //        colliders = new List<Collider>();
            //        _collidersPerType[type] = colliders;
            //    }
            //    colliders.Add(collider);
            //}
        }

        /// <summary>
        /// Update the location and size of a collider
        /// </summary>
        public void Update(Collider collider)
        {
            if (collider.Size.X == 0 || collider.Size.Y == 0)
                throw new InvalidOperationException("Cannot update a collider to a size of zero, size received: " + collider.Size);


            _spaceTree.Move(collider.Location.X, collider.Location.Y, collider.Size.X, collider.Size.Y, collider.SpaceTreeDataNode);
        }

        /// <summary>
        /// Remove a
        /// </summary>
        public void Remove(Collider collider)
        {
            //_colliders.Remove(collider);
            _spaceTree.Remove(collider);

            ////Remove in dictionary per type...
            //List<Collider> colliders;
            //foreach (Type type in GetAllTypesForGameObject(collider.GameObject))
            //{
            //    if (_collidersPerType.TryGetValue(type, out colliders))
            //    {
            //        colliders.Remove(collider);
            //    }
            //}
        }


        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollision(Collider movingCollider, Type[] types)
        {
            if (_spaceTree.Count == 0)
                return null;


            Collision collision = null;

            foreach (Collider collider in _spaceTree.Search(movingCollider.MovingLocation.X, movingCollider.MovingLocation.Y, movingCollider.Size.X, movingCollider.Size.Y))
            {
                //Not ourself.
                if (collider != movingCollider)
                {
                    if (types == null || IsGameObjectInTypes(collider.GameObject, types))
                        CollisionHelper.GetCollision(movingCollider, collider, ref collision);
                }
            }


            return collision;
        }



        /// <summary>
        /// Check if there is something un a rect
        /// </summary>
        public bool Any(float x, float y, float width, float height)
        {
            return _spaceTree.Any(x, y, width, height);
        }


        /// <summary>
        /// Check if a game object is the right type
        /// </summary>
        private bool IsGameObjectInTypes(GameObject gameObject, Type[] types)
        {
            List<Type> gameObjectTypes = GetAllTypesForGameObject(gameObject);
            for (int index = 0; index < types.Length; index++)
            {
                if(gameObjectTypes.Contains(types[index]))
                {
                    return true;
                }

            }

            return false;
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


        ///// <summary>
        ///// Get collision from a list of collisions
        ///// </summary>
        //private void GetCollision(Collider movingCollider, List<Collider> colliders, ref Collision collision)
        //{
        //    //foreach (Collider collider in _spaceTree.Search(movingCollider.MovingLocation.X, movingCollider.MovingLocation.Y, movingCollider.Size.X, movingCollider.Size.Y))
        //    //{
        //    //    //Not ourself.
        //    //    if (collider != movingCollider)
        //    //    {
        //    //        CollisionHelper.GetCollision(movingCollider, collider, ref collision);
        //    //    }
        //    //}
        //    for (int index = 0; index < colliders.Count; index++)
        //    {
        //        //Not ourself.
        //        if (colliders[index] != movingCollider)
        //        {
        //            //Little check to skip the call to the collider may be colliding... if not, we will skip immediately
        //            if (VectorHelper.Intersects(movingCollider.MovingLocation, movingCollider.Size, colliders[index].Location, colliders[index].Size))
        //            {
        //                CollisionHelper.GetCollision(movingCollider, colliders[index], ref collision);
        //            }
        //        }
        //    }

        //}

        ///// <summary>
        ///// Get collision from a list of collisions
        ///// </summary>
        //private void GetCollision(Collider movingCollider, ref Collision collision)
        //{
        //    foreach (Collider collider in _spaceTree.Search(movingCollider.MovingLocation.X, movingCollider.MovingLocation.Y, movingCollider.Size.X, movingCollider.Size.Y))
        //    {
        //        //Not ourself.
        //        if (collider != movingCollider)
        //        {
        //            CollisionHelper.GetCollision(movingCollider, collider, ref collision);
        //        }
        //    }


        //}

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public Collision GetCollisionTravel(Collider movingCollider, Type[] types)
        {
            if (_spaceTree.Count == 0)
                return null;

            Collision collision = null;

            foreach (Collider collider in _spaceTree.Search(movingCollider.MovingLocation.X, movingCollider.MovingLocation.Y, movingCollider.Size.X, movingCollider.Size.Y))
            {
                //Not ourself.
                if (collider != movingCollider)
                {
                    if (types == null || IsGameObjectInTypes(collider.GameObject, types))
                        CollisionHelper.GetCollisionTravel(movingCollider, collider, ref collision);
                }
            }

            //if (types == null)
            //{
            //    //All types...
            //    GetCollisionTravel(movingCollider, _colliders, ref collision);
            //}
            //else
            //{
            //    //For some types...
            //    List<Collider> colliders;
            //    for (int index = 0; index < types.Length; index++)
            //    {
            //        if (_collidersPerType.TryGetValue(types[index], out colliders))
            //        {
            //            GetCollisionTravel(movingCollider, colliders, ref collision);
            //        }
            //    }
            //}


            return collision;
        }

        ///// <summary>
        ///// Get collision from a list of colliders
        ///// </summary>
        //private void GetCollisionTravel(Collider movingCollider, List<Collider> colliders, ref Collision collision)
        //{

        //    for (int index = 0; index < colliders.Count; index++)
        //    {
        //        if (colliders[index] != movingCollider)
        //        {
        //            CollisionHelper.GetCollisionTravel(movingCollider, colliders[index], ref collision);
        //        }
        //    }

        //}

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
