using FNAEngine2D.Desginer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Collidable object
        /// </summary>
        private bool _collidable = false;


        /// <summary>
        /// RootGameObject
        /// </summary>
        [Browsable(false)]
        public GameObject RootGameObject;

        /// <summary>
        /// Parent GameObject
        /// </summary>
        [Browsable(false)]
        public GameObject Parent;

        /// <summary>
        /// Name of the object
        /// </summary>
        [Category("Design")]
        [DefaultValue("")]
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Display name for the Designer
        /// </summary>
        [BrowsableAttribute(false)]
        public string DisplayName
        {
            get
            {

                if (String.IsNullOrEmpty(this.Name))
                    return this.GetType().FullName;
                else
                    return this.Name;
            }
        }

        /// <summary>
        /// LayerDepth of the object
        /// </summary>
        [Category("Layout")]
        [DefaultValue(0f)]
        public float LayerDepth { get; set; } = 0f;

        /// <summary>
        /// Indicate if GameObject is enabled (is disable, GameObject is not updated or drow)
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Indique si l'objet est paused
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool Paused { get; set; }

        /// <summary>
        /// Visibity of the object
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
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
        [Browsable(false)]
        public Rectangle Bounds
        {
            get { return new Rectangle((int)_location.X, (int)_location.Y, (int)_size.X, (int)_size.Y); }
            set
            {
                TranslateTo(value.GetLocation());
                _size = value.GetSize();
            }
        }

        /// <summary>
        /// Position X
        /// </summary>
        [Browsable(false)]
        public float X
        {
            get { return _location.X; }
            set
            {
                TranslateX(value - _location.X);
            }
        }

        /// <summary>
        /// Position Y
        /// </summary>
        [Browsable(false)]
        public float Y
        {
            get { return _location.Y; }
            set
            {
                TranslateY(value - _location.Y);
            }
        }

        /// <summary>
        /// Width
        /// </summary>
        [Browsable(false)]
        public float Width
        {
            get { return _size.X; }
            set { ResizeWidth(value - _size.X); }
        }

        /// <summary>
        /// Height
        /// </summary>
        [Browsable(false)]
        public float Height
        {
            get { return (int)_size.Y; }
            set { ResizeHeight(value - _size.Y); }
        }

        /// <summary>
        /// Right
        /// </summary>
        [Browsable(false)]
        public float Right
        {
            get { return _location.X + _size.X; }
            set { TranslateX((value - _size.X) - _location.X); }
        }

        /// <summary>
        /// Bottom
        /// </summary>
        [Browsable(false)]
        public float Bottom
        {
            get { return _location.Y + _size.Y; }
            set { TranslateY((value - _size.Y) - _location.Y); }
        }

        /// <summary>
        /// Location
        /// </summary>
        [Category("Layout")]
        public Vector2 Location
        {
            get { return _location; }
            set { TranslateTo(value); }
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
        [Category("Layout")]
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }

        }

        /// <summary>
        /// Center en X
        /// </summary>
        [Browsable(false)]
        public float CenterX
        {
            get { return _location.X + (_size.X / 2); }
        }

        /// <summary>
        /// Center en Y
        /// </summary>
        [Browsable(false)]
        public float CenterY
        {
            get { return _location.Y + (_size.Y / 2); }
        }


        ///// <summary>
        ///// Scale
        ///// </summary>
        //public float Scale = 1f;

        

        /// <summary>
        /// Count des childrens
        /// </summary>
        [Browsable(false)]
        public int NbChildren
        {
            get { return _childrens.Count; }
        }


        /// <summary>
        /// Layer of the game object
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(Layers.Layer1)]
        public Layers LayerMask { get; set; } = Layers.Layer1;

        /// <summary>
        /// Count des childrens
        /// </summary>
        [Category("Layout")]
        [DefaultValue(0)]
        public int ChildIndex
        {
            get
            {
                if (this.Parent != null)
                    return this.Parent._childrens.IndexOf(this);
                else
                    return -1;
            }
            set
            {
                if (this.Parent != null)
                {
                    this.Parent._childrens.Remove(this);
                    this.Parent._childrens.Insert(value, this);
                }
            }
        }


        /// <summary>
        /// Collidable object
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool Collidable
        {
            get { return _collidable; }
            set
            {
                if (_collidable != value)
                {
                    if (value)
                        EnableCollider();
                    else
                        DisableCollider();
                }
            }
        }


        /// <summary>
        /// Empty constructor
        /// </summary>
        public GameObject()
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public GameObject(Rectangle bounds)
        {
            this.Bounds = bounds;
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

            if (_childrens.Count > 0)
            {
                GameObject ret = _childrens.Find(o => findFunc(o));
                if (ret == null)
                {
                    for (int index = 0; index < _childrens.Count; index++)
                    {
                        ret = _childrens[index].Find(findFunc);
                        if (ret != null)
                            break;
                    }
                }
                return ret;
            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// Find a GameObjet
        /// </summary>
        public T Find<T>(Func<T, bool> findFunc) where T : GameObject
        {
            return (T)Find(o => typeof(T).IsAssignableFrom(o.GetType()) && findFunc((T)o));

        }



        /// <summary>
        /// Find a GameObject
        /// </summary>
        public List<GameObject> FindAll(Func<GameObject, bool> findFunc)
        {
            List<GameObject> list = new List<GameObject>();

            FindAllInternal(findFunc, list);

            return list;
        }

        /// <summary>
        /// Find a GameObject
        /// </summary>
        private void FindAllInternal(Func<GameObject, bool> findFunc, List<GameObject> list)
        {
            if (_childrens.Count > 0)
            {
                for (int index = 0; index < _childrens.Count; index++)
                {
                    if (findFunc(_childrens[index]))
                        list.Add(_childrens[index]);

                    _childrens[index].FindAllInternal(findFunc, list);
                }
            }
        }

        /// <summary>
        /// Find a GameObject
        /// </summary>
        public List<T> FindAll<T>() where T : GameObject
        {
            return FindAll<T>(null);
        }

        /// <summary>
        /// Find a GameObject
        /// </summary>
        public List<T> FindAll<T>(Func<T, bool> findFunc) where T : GameObject
        {
            List<T> list = new List<T>();

            FindAllInternal(findFunc, list);

            return list;
        }

        /// <summary>
        /// Find a GameObject
        /// </summary>
        private void FindAllInternal<T>(Func<T, bool> findFunc, List<T> list) where T : GameObject
        {
            if (_childrens.Count > 0)
            {
                for (int index = 0; index < _childrens.Count; index++)
                {
                    if (typeof(T).IsAssignableFrom(_childrens[index].GetType()))
                    {
                        if (findFunc == null || findFunc((T)_childrens[index]))
                            list.Add((T)_childrens[index]);
                    }

                    _childrens[index].FindAllInternal(findFunc, list);
                }
            }
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
            if (GameHost.DevelopmentMode)
                GameContentManager.ReloadModifiedContent(this);

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

            //Check if object must be renderer by the camera...
            if ((GameHost.InternalGameHost.CurrentCamera.LayerMask & this.LayerMask) != 0)
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
            _collidable = true;

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
        /// Disable the collider
        /// </summary>
        public GameObject DisableCollider()
        {
            _collidable = false;

            //Already disabled?
            if (_collider == null)
                return this;

            //RootGameObject is never really added anywhere...
            if (this == GameHost.RootGameObject)
                throw new InvalidOperationException("Impossible to enable collider on root game object.");


            GetColliderContainer().Remove(_collider);

            _collider = null;

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

        /// <summary>
        /// Permet d'obtenir la première collision
        /// </summary>
        public Collision GetCollision(Vector2 position, Vector2 size, Type[] types)
        {
            ColliderContainer container = GetColliderContainer();

            if (container.IsEmpty)
                return null;

            return container.GetCollision(position, size, _collider, types);
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


        /// <summary>
        /// Override of ToString to help in debug.
        /// </summary>
        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
