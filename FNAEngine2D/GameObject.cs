using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FNAEngine2D
{
    /// <summary>
    /// RenderObject
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// Gère le loading des childrens
        /// </summary>
        private bool _loaded = false;

        /// <summary>
        /// Add children autorized
        /// </summary>
        internal bool _addAuthorized = false;

        /// <summary>
        /// Visible
        /// </summary>
        private bool _visible = true;

        /// <summary>
        /// Collider container
        /// </summary>
        private ColliderContainer _colliderContainer = null;

        /// <summary>
        /// Avons-nous un collider?
        /// </summary>
        private Collider _collider = null;


        /// <summary>
        /// Childrens
        /// </summary>
        private List<GameObject> _childrens = new List<GameObject>();

        /// <summary>
        /// Location
        /// </summary>
        private Vector2 _location;

        /// <summary>
        /// Size
        /// </summary>
        private Vector2 _size;

        /// <summary>
        /// RootGameObject
        /// </summary>
        public GameObject RootGameObject;

        /// <summary>
        /// Parent GameObject
        /// </summary>
        public GameObject Parent;

        /// <summary>
        /// Name of the object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicate if GameObject is enabled (is disable, GameObject is not updated or drow)
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Indique si l'objet est paused
        /// </summary>
        public bool Paused { get; set; }

        /// <summary>
        /// Visibity of the object
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (_visible != value)
                {
                    if (value)
                        Show();
                    else
                        Hide();
                }
            }
        }


        /// <summary>
        /// Bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return new Rectangle((int)_location.X, (int)_location.Y, (int)_size.X, (int)_size.Y); }
            set
            {
                _location = value.GetLocation();
                _size = value.GetSize();
            }
        }

        /// <summary>
        /// Position X
        /// </summary>
        public float X
        {
            get { return _location.X; }
            set { _location.X = value; }
        }

        /// <summary>
        /// Position Y
        /// </summary>
        public float Y
        {
            get { return _location.Y; }
            set { _location.Y = value; }
        }

        /// <summary>
        /// Width
        /// </summary>
        public float Width
        {
            get { return _size.X; }
            set { _size.X = value; }
        }

        /// <summary>
        /// Height
        /// </summary>
        public float Height
        {
            get { return (int)_size.Y; }
            set { _size.Y = value; }
        }

        /// <summary>
        /// Right
        /// </summary>
        public float Right
        {
            get { return _location.X + _size.X; }
            set { _location.X = value - _size.X; }
        }

        /// <summary>
        /// Bottom
        /// </summary>
        public float Bottom
        {
            get { return _location.Y + _size.Y; }
            set { _location.Y = value - _size.Y; }
        }

        /// <summary>
        /// Location
        /// </summary>
        public Vector2 Location
        {
            get { return _location; }
            set { _location = value; }
        }

        ///// <summary>
        ///// Position
        ///// </summary>
        //public Vector2 Position
        //{
        //    get { return new Vector2(this.X, this.Y); }
        //    set
        //    {
        //        this.Bounds.X = (int)value.X;
        //        this.Bounds.Y = (int)value.Y;
        //    }
        //}

        ///// <summary>
        ///// Rectangle
        ///// </summary>
        //public Rectangle Rectangle
        //{
        //    get { return this.Bounds; }
        //    set { this.Bounds = value; }
        //}

        /// <summary>
        /// Size
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }

        }

        /// <summary>
        /// Center en X
        /// </summary>
        public float CenterX
        {
            get { return _location.X + (_size.X / 2); }
        }

        /// <summary>
        /// Center en Y
        /// </summary>
        public float CenterY
        {
            get { return _location.Y + (_size.Y / 2); }
        }


        /// <summary>
        /// Scale
        /// </summary>
        public float Scale = 1f;

        /// <summary>
        /// Rotation
        /// </summary>
        public float Rotation = 0f;

        /// <summary>
        /// Rotation
        /// </summary>
        public Vector2 RotationOrigin;


        /// <summary>
        /// Count des childrens
        /// </summary>
        public int NbChildren
        {
            get { return _childrens.Count; }
        }


        /// <summary>
        /// Get a child
        /// </summary>
        public GameObject Get(int indexChildren)
        {
            return _childrens[indexChildren];
        }

        /// <summary>
        /// Ajout d'un render object enfant
        /// </summary>
        public T Add<T>(T gameObject) where T : GameObject
        {
            return Insert(_childrens.Count, gameObject);
        }

        /// <summary>
        /// Ajout d'un render object enfant
        /// </summary>
        public T Insert<T>(int index, T gameObject) where T : GameObject
        {
            
            if (!_addAuthorized)
                throw new InvalidOperationException("Add or Insert not authorized before Load method.");

            gameObject.Parent = this;
            gameObject.RootGameObject = this.RootGameObject;


            this._childrens.Insert(index, gameObject);

            //Already a collider? can happen if remove and readded...
            AddColliders(gameObject);

            //If not loaded already... can happen if remove and readded...
            if (!gameObject._loaded)
            {
                gameObject._addAuthorized = true;
                gameObject.Load();
            }

            return gameObject;
        }

        /// <summary>
        /// Remove the gameobject
        /// </summary>
        public void Remove(GameObject gameObject)
        {
            gameObject.Parent = null;
            gameObject.RootGameObject = null;

            //Remove de colliders for the gameobject and all children...
            RemoveColliders(gameObject);

            this._childrens.Remove(gameObject);

            MouseManager.RemoveGameObject(gameObject);
        }

        /// <summary>
        /// Remove the gameobject
        /// </summary>
        public void RemoveAt(int index)
        {
            GameObject gameObject = _childrens[index];

            gameObject.Parent = null;
            gameObject.RootGameObject = null;

            //Remove de colliders for the gameobject and all children...
            RemoveColliders(gameObject);

            this._childrens.RemoveAt(index);

            MouseManager.RemoveGameObject(gameObject);
        }

        /// <summary>
        /// Remove all the gameobject of a certain type
        /// </summary>
        public void Remove(Type gameObjectType)
        {
            for (int index = this._childrens.Count - 1; index >= 0; index--)
            {
                if (this._childrens[index].GetType() == gameObjectType)
                {
                    Remove(this._childrens[index]);
                }
            }
        }

        /// <summary>
        /// Remove all the gameobject
        /// </summary>
        public void RemoveAll()
        {
            for (int index = this._childrens.Count - 1; index >= 0; index--)
            {
                Remove(this._childrens[index]);
            }
        }

        /// <summary>
        /// Find a GameObjet by name
        /// </summary>
        public GameObject Find(string name)
        {
            return Find(o => o.Name == name);
        }

        /// <summary>
        /// Find a GameObjet by name
        /// </summary>
        public T Find<T>(string name) where T : GameObject
        {
            return (T)Find(o => typeof(T).IsAssignableFrom(o.GetType()) && o.Name == name);
        }

        /// <summary>
        /// Find a GameObject
        /// </summary>
        public GameObject Find(Func<GameObject, bool> findFunc)
        {
            GameObject ret = _childrens.Find(o => findFunc(o));
            if (ret == null)
            {
                if (_childrens.Count > 0)
                {
                    for (int index = 0; index < _childrens.Count; index++)
                    {
                        ret = _childrens[index].Find(findFunc);
                        if (ret != null)
                            break;
                    }
                }

            }
            return ret;
        }

        /// <summary>
        /// Find a GameObjet
        /// </summary>
        public T Find<T>(Func<T, bool> findFunc) where T: GameObject
        {
            return (T)Find(o => typeof(T).IsAssignableFrom(o.GetType()) && findFunc((T)o));

        }


        /// <summary>
        /// Execute an action for each GameObjet in childrens
        /// </summary>
        public void ForEach(Action<GameObject> action)
        {
            if (_childrens.Count == 0)
                return;

            for (int index = 0; index < _childrens.Count; index++)
                action(_childrens[index]);
        }

        /// <summary>
        /// Chargement du content
        /// </summary>
        public virtual void Load()
        {

        }

        ///// <summary>
        ///// Chargement du content
        ///// </summary>
        //internal void LoadWithChildren()
        //{
        //    //_isLoading = true;

        //    this.Load();

        //    if (this.Childrens.Count > 0)
        //    {
        //        for (int index = 0; index < this.Childrens.Count; index++)
        //        {
        //            if (!this.Childrens[index]._loaded)
        //                this.Childrens[index].LoadWithChildren();
        //        }
        //    }

        //    _loaded = true;

        //}

        /// <summary>
        /// Logique d'update
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Logique d'update
        /// </summary>
        internal void UpdateWithChildren()
        {
            //Paused or disabled?
            if (this.Paused || !this.Enabled)
                return;

            //Update GameContent if needed...
#if DEBUG
            GameContentManager.ReloadModifiedContent(this);
#endif

            this.Update();

            if (this._childrens.Count == 0)
                return;

            for (int index = 0; index < this._childrens.Count; index++)
                this._childrens[index].UpdateWithChildren();
        }

        /// <summary>
        /// Draw à l'écran
        /// </summary>
        public virtual void Draw()
        {

        }

        /// <summary>
        /// Draw à l'écran
        /// </summary>
        internal void DrawWithChildren()
        {
            //Disabled?
            if (!this.Enabled)
                return;

            this.Draw();

            if (this._childrens.Count == 0)
                return;

            for (int index = 0; index < this._childrens.Count; index++)
            {
                if (this._childrens[index].Visible)
                    this._childrens[index].DrawWithChildren();
            }
        }


        /// <summary>
        /// Destruction du game object
        /// </summary>
        public void Destroy()
        {
            if (this.Parent == null)
                throw new InvalidOperationException("Impossible du destroy de root game object.");

            //Simply removing this...
            this.Parent._childrens.Remove(this);

            //Retrait du collider...
            if (_collider != null)
                GetColliderContainer().Remove(_collider);
        }


        /// <summary>
        /// Permet de déplacer en X l'objet et tous ses enfants
        /// </summary>
        public void TranslateX(float offsetX)
        {
            _location.X += offsetX;

            if (this._childrens.Count == 0)
                return;

            for (int index = 0; index < this._childrens.Count; index++)
                this._childrens[index].TranslateX(offsetX);

        }

        /// <summary>
        /// Permet de déplacer en Y l'objet et tous ses enfants
        /// </summary>
        public void TranslateY(float offsetY)
        {
            _location.Y += offsetY;

            if (this._childrens.Count == 0)
                return;

            for (int index = 0; index < this._childrens.Count; index++)
                this._childrens[index].TranslateY(offsetY);

        }


        /// <summary>
        /// Permet de déplacer en X et Y l'objet et tous ses enfants
        /// </summary>
        public void Translate(float offsetX, float offsetY)
        {
            _location.X += offsetX;
            _location.Y += offsetY;


            if (this._childrens.Count == 0)
                return;

            for (int index = 0; index < this._childrens.Count; index++)
                this._childrens[index].Translate(offsetX, offsetY);

        }

        /// <summary>
        /// Permet de déplacer en X et Y l'objet et tous ses enfants
        /// </summary>
        public void Translate(Vector2 translation)
        {
            this.Translate(translation.X, translation.Y);
        }

        /// <summary>
        /// Permet de déplacer vers une coordonnées
        /// </summary>
        public void TranslateTo(Vector2 destination)
        {
            this.Translate(destination.X - _location.X, destination.Y - _location.Y);
        }

        /// <summary>
        /// Permet de déplacer vers une coordonnées
        /// </summary>
        public void TranslateTo(Point destination)
        {
            this.Translate(destination.X - _location.X, destination.Y - _location.Y);
        }


        /// <summary>
        /// Resize width of an offset
        /// </summary>
        public void ResizeWidth(float offsetX)
        {
            _size.X += offsetX;


            if (this._childrens.Count == 0)
                return;

            for (int index = 0; index < this._childrens.Count; index++)
                this._childrens[index].ResizeWidth(offsetX);

        }

        /// <summary>
        /// Resize height of an offset
        /// </summary>
        public void ResizeHeight(float offsetY)
        {
            _size.Y += offsetY;


            if (this._childrens.Count == 0)
                return;

            for (int index = 0; index < this._childrens.Count; index++)
                this._childrens[index].ResizeHeight(offsetY);

        }


        /// <summary>
        /// Resize width and height of an offset
        /// </summary>
        public void Resize(float offsetX, float offsetY)
        {
            _size.X += offsetX;
            _size.Y += offsetY;


            if (this._childrens.Count == 0)
                return;

            for (int index = 0; index < this._childrens.Count; index++)
                this._childrens[index].Resize(offsetX, offsetY);

        }

        /// <summary>
        /// Resize to a width and height
        /// </summary>
        public void ResizeTo(float offsetX, float offsetY)
        {
            this.Resize(offsetX - _size.X, offsetY - _size.Y);
        }

        /// <summary>
        /// Active le collider
        /// </summary>
        public GameObject EnableCollider()
        {
            //Already enabled?
            if (_collider != null)
                return this;

            //RootGameObject is never really added anywhere...
            if (this == GameHost.RootGameObject)
                throw new InvalidOperationException("Impossible to enable collider on root game object.");


            _collider = new Collider(this);

            //If not already added, we will add it in the ColliderContainer on Add
            if (this.Parent == null)
                return this;

            //Add to container...
            GetColliderContainer().Add(_collider);

            return this;
        }

        /// <summary>
        /// Permet d'obtenir la première collision
        /// </summary>
        public Collision GetCollision(Type[] types)
        {
            ColliderContainer container = GetColliderContainer();

            if (container.IsEmpty)
                return null;

            return container.GetCollision(this._location, this._size, _collider, types);
        }

        /// <summary>
        /// Permet d'obtenir la première collision
        /// </summary>
        public Collision GetCollision(Vector2 position, Type[] types)
        {
            ColliderContainer container = GetColliderContainer();

            if (container.IsEmpty)
                return null;

            return container.GetCollision(position, _size, _collider, types);
        }

        /// <summary>
        /// Permet d'obtenir la première collision
        /// </summary>
        public Collision GetCollision(float nextX, float nextY, Type[] types)
        {
            return GetCollision(new Vector2(nextX, nextY), types);
        }

        ///// <summary>
        ///// Permet d'obtenir la liste des collisions
        ///// </summary>
        //public List<Collision> GetCollisions(Type[] types)
        //{
        //    return GetCollisions(_location, types);

        //}

        ///// <summary>
        ///// Permet d'obtenir la première collision
        ///// </summary>
        //public List<Collision> GetCollisions(Vector2 position, Type[] types)
        //{
        //    ColliderContainer container = GetColliderContainer();

        //    if (container.IsEmpty)
        //        return CollisionHelper.EMPTY_COLLISIONS;

        //    return container.GetCollisions(position, _size, _collider, types);

        //}


        ///// <summary>
        ///// Permet d'obtenir la liste des collisions
        ///// </summary>
        //public List<Collision> GetCollisions(int nextX, int nextY, Type[] types)
        //{
        //    return GetCollisions(new Vector2(nextX, nextY), types);
        //}

        /// <summary>
        /// Hide a object
        /// </summary>
        public virtual void Hide()
        {
            _visible = false;
        }

        /// <summary>
        /// Show a object
        /// </summary>
        public virtual void Show()
        {
            _visible = true;
        }



        /// <summary>
        /// Count the gameobject in children
        /// </summary>
        public int Count(Type gameObjectType)
        {
            int count = 0;

            for (int index = 0; index < this._childrens.Count; index++)
            {
                if (this._childrens[index].GetType() == gameObjectType)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Permet de trouver le collider container où tout mettre les collider
        /// </summary>
        private ColliderContainer GetColliderContainer()
        {
            if (this.RootGameObject._colliderContainer == null)
                this.RootGameObject._colliderContainer = new ColliderContainer();

            return this.RootGameObject._colliderContainer;
        }

        /// <summary>
        /// Process the removal of a game object for colliders (including children)
        /// </summary>
        private void AddColliders(GameObject gameObject)
        {
            if (gameObject._collider != null)
            {
                GetColliderContainer().Add(gameObject._collider);
            }

            if (gameObject._childrens.Count > 0)
            {
                for (int index = 0; index < gameObject._childrens.Count; index++)
                {
                    AddColliders(gameObject._childrens[index]);
                }
            }

        }

        /// <summary>
        /// Process the removal of a game object for colliders (including children)
        /// </summary>
        private void RemoveColliders(GameObject gameObject)
        {
            if (gameObject._collider != null)
            {
                GetColliderContainer().Remove(gameObject._collider);
            }

            if (gameObject._childrens.Count > 0)
            {
                for (int index = 0; index < gameObject._childrens.Count; index++)
                {
                    RemoveColliders(gameObject._childrens[index]);
                }
            }

        }


    }
}
