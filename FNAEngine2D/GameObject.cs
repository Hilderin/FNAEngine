using FNAEngine2D.Aseprite;
using FNAEngine2D.Collisions;
using FNAEngine2D.Desginer;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FNAEngine2D
{
    /// <summary>
    /// RenderObject
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// Indicate if the component is present in the active list
        /// </summary>
        private bool _presentInActiveUpdateList = false;
        private bool _presentInActiveDrawList = false;

        /// <summary>
        /// Updatable object if the component implements IUpdate
        /// </summary>
        private IUpdate _updateable;

        /// <summary>
        /// Drawable object if the component implements IDraw
        /// </summary>
        private IDraw _drawable;


        /// <summary>
        /// Link the the Internal game
        /// </summary>
        internal Game _game = Game.Current;

        /// <summary>
        /// Gère le loading des childrens
        /// </summary>
        internal bool _loaded = false;

        /// <summary>
        /// Add children autorized
        /// </summary>
        private bool _addAuthorized = false;

        /// <summary>
        /// Visible
        /// </summary>
        private bool _visibleSelf = true;

        /// <summary>
        /// Visible
        /// </summary>
        private bool _visibleParent = false;

        /// <summary>
        /// Enabled
        /// </summary>
        private bool _enabledSelf = true;

        /// <summary>
        /// Enabled
        /// </summary>
        private bool _enabledParent = false;

        /// <summary>
        /// Paused
        /// </summary>
        private bool _pausedSelf = false;

        /// <summary>
        /// Paused
        /// </summary>
        private bool _pausedParent = false;

        /// <summary>
        /// Indicate if the GameObject is on the hierarchy of the RootGameObject. True if added in a child or is the RootGameObject itself.
        /// </summary>
        private bool _isOnScene = false;

        /// <summary>
        /// Parent
        /// </summary>
        private GameObject _parent;

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
        /// Layer mask
        /// </summary>
        private Layers _layerMask = Layers.Layer1;

        /// <summary>
        /// Depth
        /// </summary>
        private float _depth = 0f;


        /// <summary>
        /// Components
        /// </summary>
        private Dictionary<Type, List<Component>> _componentsDict = new Dictionary<Type, List<Component>>();

        /// <summary>
        /// Components
        /// </summary>
        private List<Component> _componentsList = new List<Component>();

        ///// <summary>
        ///// RootGameObject
        ///// </summary>
        //[Browsable(false)]
        //[JsonIgnore]
        //public GameObject RootGameObject;

        /// <summary>
        /// Parent GameObject
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public GameObject Parent
        {
            get { return _parent; }
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    if (_parent != null)
                    {
                        _enabledParent = _parent.Enabled;
                        _visibleParent = _parent.Visible;
                        _pausedParent = _parent.Paused;
                    }
                    else
                    {
                        _enabledParent = false;
                        _visibleParent = false;
                        _pausedParent = false;
                    }
                }
            }
        }

        /// <summary>
        /// Name of the object
        /// </summary>
        [Category("Design")]
        [DefaultValue("")]
        [Description("Name of the GameObject")]
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Display name for the Designer
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
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
        /// Depth of the object
        /// Higher = Draw first
        /// Lower = In front
        /// </summary>
        [Category("Layout")]
        [DefaultValue(0f)]
        [Description("Depth of the object\r\nHigher = Draw first\r\nLower = In front")]
        public float Depth
        {
            get { return _depth; }
            set
            {
                if (_depth != value)
                {

                    //Update children that were on the same layer...
                    if (this._childrens.Count > 0)
                    {

                        for (int index = 0; index < this._childrens.Count; index++)
                        {
                            if (this._childrens[index].Depth == _depth)
                                this._childrens[index].Depth = value;
                        }

                    }

                    _depth = value;
                }
            }
        }

        /// <summary>
        /// Indicate if GameObject itself is enabled. If disable, GameObject is not updated or drew.
        /// When disabling a GameObject, all children will be disabled but still EnabledSelf true.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Indicate if GameObject is enabled. If disable, GameObject is not updated or drew.")]
        public bool EnabledSelf
        {
            get { return _enabledSelf; }
            set
            {
                if (_enabledSelf != value)
                {
                    _enabledSelf = value;

                    if (value)
                    {
                        DoOnEnabled();
                    }
                    else
                    {
                        DoOnDisabled();
                    }
                }
            }
        }

        /// <summary>
        /// Indicate if GameObject is enabled globally (selft and parents). If disable, GameObject is not updated or drew.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool Enabled
        {
            get { return _enabledSelf && _enabledParent; }
        }

        /// <summary>
        /// Indicate if GameObject is itselft paused. If paused, GameObject is not updated.
        /// When paused a GameObject, all children will be paused but still PausedSelf false.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicate if GameObject is paused. If paused, GameObject is not updated.")]
        public bool PausedSelf
        {
            get { return _pausedSelf; }
            set
            {
                if (_pausedSelf != value)
                {
                    _pausedSelf = value;

                    if (value)
                    {
                        DoOnPaused();
                    }
                    else
                    {
                        DoOnUnpaused();
                    }
                }
            }
        }

        /// <summary>
        /// Indicate if GameObject is paused globally (selft and parents). If paused, GameObject is not updated.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool Paused
        {
            get { return _pausedSelf && _pausedParent; }
        }

        /// <summary>
        /// Visibity of the object
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Description("Indicate if GameObject is visible. If not visible, GameObject is not drew.")]
        public bool VisibleSelf
        {
            get { return _visibleSelf; }
            set
            {
                if (_visibleSelf != value)
                {
                    if (value)
                        Show();
                    else
                        Hide();
                }
            }
        }

        /// <summary>
        /// Indicate if GameObject is visible globally (selft and parents). If not visible, GameObject is not drew but is updated.
        /// When disabling a GameObject, all children will be disabled but still EnabledSelf true.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool Visible
        {
            get { return _visibleSelf && _visibleParent; }
        }

        /// <summary>
        /// Indicate if the GameObject is on the hierarchy of the RootGameObject. True if added in a child or is the RootGameObject itself.
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool IsOnScene
        {
            get { return _isOnScene; }
        }


        /// <summary>
        /// Bounds
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public Rectangle Bounds
        {
            get { return new Rectangle((int)_location.X, (int)_location.Y, (int)_size.X, (int)_size.Y); }
            set
            {
                TranslateTo(value.GetLocation());

                Vector2 newSize = value.GetSize();
                if (_size != newSize)
                {
                    ResizeTo(newSize.X, newSize.Y);
                }
            }
        }

        /// <summary>
        /// Position X
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
        public float Width
        {
            get { return _size.X; }
            set { ResizeWidth(value - _size.X); }
        }

        /// <summary>
        /// Height
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float Height
        {
            get { return (int)_size.Y; }
            set { ResizeHeight(value - _size.Y); }
        }

        /// <summary>
        /// Right
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float Right
        {
            get { return _location.X + _size.X; }
            set { TranslateX((value - _size.X) - _location.X); }
        }

        /// <summary>
        /// Bottom
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float Bottom
        {
            get { return _location.Y + _size.Y; }
            set { TranslateY((value - _size.Y) - _location.Y); }
        }

        /// <summary>
        /// Location
        /// </summary>
        [Category("Layout")]
        [Description("Position in the world of the GameObject.")]
        public Vector2 Location
        {
            get { return _location; }
            set { TranslateTo(value); }
        }

        /// <summary>
        /// Size
        /// </summary>
        [Category("Layout")]
        [Description("Size of the GameObject")]
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    ResizeTo(value.X, value.Y);
                }
            }

        }

        /// <summary>
        /// Center vector
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public Vector2 Center
        {
            get { return new Vector2(this.CenterX, this.CenterY); }
        }

        /// <summary>
        /// Center en X
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float CenterX
        {
            get { return _location.X + (_size.X / 2); }
        }

        /// <summary>
        /// Center en Y
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
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
        [JsonIgnore]
        public int NbChildren
        {
            get { return _childrens.Count; }
        }


        /// <summary>
        /// Layer of the game object
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(Layers.Layer1)]
        [Description("Layer on which the GameObject should be drew.")]
        public Layers LayerMask
        {
            get { return _layerMask; }
            set
            {
                if (_layerMask != value)
                {

                    //Update children that were on the same layer...
                    if (this._childrens.Count > 0)
                    {

                        for (int index = 0; index < this._childrens.Count; index++)
                        {
                            if (this._childrens[index].LayerMask == _layerMask)
                                this._childrens[index].LayerMask = value;
                        }

                    }

                    _layerMask = value;
                }
            }
        }

        /// <summary>
        /// Count des childrens
        /// </summary>
        [Category("Layout")]
        [DefaultValue(0)]
        [JsonIgnore]
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
        [Description("Indicate of a collider should be activated for this GameObject.")]
        public virtual bool Collidable
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

                    //To be sure because EnableCollider and DisableCollider can be overrided
                    _collidable = value;
                }
            }
        }

        /// <summary>
        /// Collider for this Game object. Null if no collider has been added
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public Collider Collider
        {
            get { return _collider; }
        }

        ///// <summary>
        ///// Update version
        ///// </summary>
        //[Browsable(false)]
        //[JsonIgnore]
        //public int Version { get; set; }

        /// <summary>
        /// DrawingContext to draw on screen
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public DrawingContext DrawingContext { get { return _game.DrawingContext; } }

        /// <summary>
        /// GameContentManager
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public GameContentManager GameContentManager { get { return _game.GameContentManager; } }

        /// <summary>
        /// Mouse
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public MouseManager Mouse { get { return _game.Mouse; } }

        /// <summary>
        /// Input
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public InputManager Input { get { return _game.Input; } }

        /// <summary>
        /// Game
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public Game Game { get { return _game; } internal set { _game = value; } }

        /// <summary>
        /// ElapsedGameTimeSeconds
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float ElapsedGameTimeSeconds { get { return _game.ElapsedGameTimeSeconds; } }

        /// <summary>
        /// ElapsedGameTimeMilliseconds
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float ElapsedGameTimeMilliseconds { get { return _game.ElapsedGameTimeMilliseconds; } }

        /// <summary>
        /// Number of pixels = 1 meter
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float NbPixelPerMeter { get { return _game.NbPixelPerMeter; } }



        /// <summary>
        /// Empty constructor
        /// </summary>
        public GameObject()
        {
            _updateable = this as IUpdate;
            _drawable = this as IDraw;
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
            //gameObject.RootGameObject = this.RootGameObject;

            _game = Game.Current;

            //Propagate the layermask...
            if (gameObject.LayerMask == Layers.Layer1 && this.LayerMask != Layers.Layer1)
                gameObject.LayerMask = this.LayerMask;

            //Propagate the despth...
            if (gameObject.Depth == 0f && this.Depth != 0f)
                gameObject.Depth = this.Depth;


            this._childrens.Insert(index, gameObject);

            //Already a collider? can happen if remove and readded...
            //AddColliders(gameObject);

            //If not loaded already... can happen if remove and readded...
            if (!gameObject._loaded)
            {
                gameObject._addAuthorized = true;
                gameObject.DoLoad();
                gameObject.DoOnAdded(false);
            }
            else
            {
                //Already loaded so we will reexecute all the OnAdded
                gameObject.DoOnAdded(true);
            }

            return gameObject;
        }

        /// <summary>
        /// Remove the gameobject
        /// </summary>
        public void Remove(GameObject gameObject)
        {
            gameObject.Parent = null;
            //gameObject.RootGameObject = null;

            ////Remove de colliders for the gameobject and all children...
            //RemoveColliders(gameObject);


            gameObject.DoOnRemoved();

            this._childrens.Remove(gameObject);

        }

        /// <summary>
        /// Remove the gameobject
        /// </summary>
        public void RemoveAt(int index)
        {
            GameObject gameObject = _childrens[index];

            gameObject.Parent = null;
            //gameObject.RootGameObject = null;

            gameObject.DoOnRemoved();

            this._childrens.RemoveAt(index);


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
        /// Add a component
        /// </summary>
        public T AddComponent<T>() where T : Component
        {
            return AddComponent<T>((T)Activator.CreateInstance(typeof(T)));
        }

        /// <summary>
        /// Add a component
        /// </summary>
        public T AddComponent<T>(T component) where T : Component
        {
            component.GameObject = this;
            AddComponentTypes(typeof(T), component);
            _componentsList.Add(component);

            if (_loaded)
            {
                component.DoLoad();
                component.DoOnAdded();
            }

            if (component is Collider)
                _collider = component as Collider;

            return component;
        }

        /// <summary>
        /// Add all component types
        /// </summary>
        private void AddComponentTypes(Type componentType, Component gameComponent)
        {
            if (componentType == typeof(Component))
                return;

            List<Component> list;
            if (!_componentsDict.TryGetValue(componentType, out list))
            {
                list = new List<Component>();
                _componentsDict[componentType] = list;
            }
            list.Add(gameComponent);

            if (componentType.BaseType != null)
                AddComponentTypes(componentType.BaseType, gameComponent);

            foreach (Type interfaceType in componentType.GetInterfaces())
                AddComponentTypes(interfaceType, gameComponent);
        }

        /// <summary>
        /// Remove a component
        /// </summary>
        public void RemoveComponent<T>() where T : Component
        {
            List<Component> list;
            if (_componentsDict.TryGetValue(typeof(T), out list))
            {
                for (int index = list.Count - 1; index >= 0; index--)
                {
                    RemoveComponent(list[index]);
                }
            }

        }

        /// <summary>
        /// Remove a component
        /// </summary>
        public void RemoveComponent(Component component)
        {
            _componentsList.Remove(component);
            RemoveComponentTypes(component.GetType(), component);

            component.DoOnRemoved();

            if (component is Collider)
                _collider = null;
        }

        /// <summary>
        /// Remove all component types
        /// </summary>
        private void RemoveComponentTypes(Type componentType, Component gameComponent)
        {
            if (componentType == typeof(Component))
                return;

            List<Component> list;
            if (_componentsDict.TryGetValue(componentType, out list))
            {
                list.Remove(gameComponent);
            }

            if (componentType.BaseType != null)
                RemoveComponentTypes(componentType.BaseType, gameComponent);

            foreach (Type interfaceType in componentType.GetInterfaces())
                RemoveComponentTypes(interfaceType, gameComponent);
        }

        /// <summary>
        /// Get a component
        /// </summary>
        public T GetComponent<T>() where T : Component
        {
            if (_componentsDict.TryGetValue(typeof(T), out List<Component> list))
            {
                if (list.Count > 0)
                    return (T)list[0];
            }
            return default(T);
        }

        /// <summary>
        /// Execute an action for each GameComponent of the game object
        /// </summary>
        public void ForEachComponent(Action<Component> action)
        {
            if (_componentsList.Count == 0)
                return;

            for (int index = 0; index < _componentsList.Count; index++)
            {
                action(_componentsList[index]);
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
        /// Find a GameObjet by type only
        /// </summary>
        public T Find<T>() where T : GameObject
        {
            return (T)Find(o => typeof(T).IsAssignableFrom(o.GetType()));
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
        /// Find a Component by type only anywhere on the object or his children
        /// </summary>
        public T FindComponent<T>() where T : Component
        {
            T component = GetComponent<T>();

            if (component != null)
                return component;


            if (_childrens.Count > 0)
            {
                for (int index = 0; index < _childrens.Count; index++)
                {
                    component = _childrens[index].GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }

            return null;

        }

        /// <summary>
        /// Execute an action for each GameObjet in childrens
        /// </summary>
        public void ForEachChild(Action<GameObject> action, bool recursive = false)
        {
            if (_childrens.Count == 0)
                return;

            for (int index = 0; index < _childrens.Count; index++)
            {
                action(_childrens[index]);

                if (recursive)
                    _childrens[index].ForEachChild(action, true);
            }
        }

        /// <summary>
        /// Find a parent GameObject of a certin type
        /// </summary>
        public T FindParent<T>()
        {
            object obj = FindParent(o => typeof(T).IsAssignableFrom(o.GetType()));
            return (T)obj;
        }

        /// <summary>
        /// Find a parent GameObject
        /// </summary>
        public GameObject FindParent(Func<GameObject, bool> findFunc)
        {
            if (this.Parent == null)
                return null;

            if (findFunc(this.Parent))
                return this.Parent;

            return this.Parent.FindParent(findFunc);

        }


        /// <summary>
        /// Loading content
        /// </summary>
        protected abstract void Load();


        /// <summary>
        /// Execute the loading of the GameObject
        /// </summary>
        public void DoLoad()
        {
            if (this == _game.RootGameObject)
            {
                _addAuthorized = true;
                _visibleParent = true;
                _enabledParent = true;
                _pausedParent = false;
            }

            this.Load();

            //Call OnRemove for components...
            if (_componentsList.Count > 0)
            {
                foreach (Component component in _componentsList)
                    component.DoLoad();
            }

            _loaded = true;
        }

        ///// <summary>
        ///// Update logic for the server
        ///// </summary>
        //protected virtual void Update()
        //{

        //}

        ///// <summary>
        ///// Logique d'update
        ///// </summary>
        //internal void DoUpdate()
        //{
        //    //Paused or disabled?
        //    if (this.Paused || !this.Enabled)
        //        return;

        //    //Update GameContent if needed...
        //    if (GameManager.DevelopmentMode)
        //        GameContentManager.ReloadModifiedContent(this);

        //    ForEachComponent(o => o.DoUpdate());

        //    this.Update();

        //    ForEachChild(o => o.DoUpdate());

        //}

        ///// <summary>
        ///// Draw à l'écran
        ///// </summary>
        //protected virtual void Draw()
        //{

        //}

        ///// <summary>
        ///// Draw à l'écran
        ///// </summary>
        //internal void DoDraw()
        //{
        //    //Disabled?
        //    if (!this.Enabled)
        //        return;

        //    //If not visible...
        //    if (!this.Visible)
        //        return;


        //    //Check if object must be renderer by the camera...
        //    if ((DrawingContext.Camera.LayerMask & this.LayerMask) != 0)
        //    {
        //        ForEachComponent(o => o.DoDraw());
        //        this.Draw();
        //    }

        //    ForEachChild(o => o.DoDraw());
        //}



        /// <summary>
        /// Called when the game object is enabled
        /// </summary>
        protected virtual void OnEnabled()
        {

        }

        /// <summary>
        /// Execute the OnEnabled
        /// </summary>
        internal void DoOnEnabled()
        {
            //If the parent is disabled, then, we are not changing our state
            if (!_loaded || !_enabledParent)
                return;

            //Ensure the object is updatable and drawable
            AddActiveUpdateable();
            AddActiveDrawable();

            ForEachComponent(c => c.DoOnEnabled());
            OnEnabled();

            ForEachChild(c =>
            {
                c._enabledParent = true;
                if (c._enabledSelf)
                    c.DoOnEnabled();
            });
        }

        /// <summary>
        /// Called when the game object is disabled
        /// </summary>
        protected virtual void OnDisabled()
        {

        }

        /// <summary>
        /// Execute the OnDisabled
        /// </summary>
        internal void DoOnDisabled()
        {
            //If the parent is disabled, then, we are not changing our state
            if (!_loaded || !_enabledParent)
                return;

            //Ensure the object is not updatable and drawable
            RemoveActiveUpdateable();
            RemoveActiveDrawable();

            ForEachComponent(c => c.DoOnDisabled());
            OnDisabled();

            ForEachChild(c =>
            {
                c.DoOnDisabledParent();
            });
        }

        /// <summary>
        /// Execute the OnDisabled
        /// </summary>
        internal void DoOnDisabledParent()
        {
            _enabledParent = false;

            if (_enabledSelf)
            {
                //Ensure the object is not updatable and drawable
                RemoveActiveUpdateable();
                RemoveActiveDrawable();

                ForEachComponent(c => c.DoOnDisabled());
                OnDisabled();

                ForEachChild(c =>
                {
                    c.DoOnDisabledParent();
                });
            }
        }


        /// <summary>
        /// Called when the game object is Paused
        /// </summary>
        protected virtual void OnPaused()
        {

        }

        /// <summary>
        /// Execute the OnPaused
        /// </summary>
        internal void DoOnPaused()
        {
            //Si the parent is already paused, we are not changing our state
            if (!_loaded || _pausedParent)
                return;

            //Ensure the object is not updatable
            RemoveActiveUpdateable();

            ForEachComponent(c => c.DoOnPaused());
            OnPaused();


            ForEachChild(c =>
            {
                c.DoOnPausedParent();
            });
        }

        /// <summary>
        /// Execute the OnPaused
        /// </summary>
        internal void DoOnPausedParent()
        {
            _pausedParent = true;

            //We the object was already paused, it's not changing it's state or the state of his children
            if (!_pausedSelf)
            {
                //Ensure the object is not updatable
                RemoveActiveUpdateable();

                ForEachComponent(c => c.DoOnPaused());
                OnPaused();


                ForEachChild(c =>
                {
                    c.DoOnPausedParent();
                });
            }
        }

        /// <summary>
        /// Called when the game object is Unpaused
        /// </summary>
        protected virtual void OnUnpaused()
        {

        }

        /// <summary>
        /// Execute the OnUnpaused
        /// </summary>
        internal void DoOnUnpaused()
        {
            if (!_loaded || _pausedParent)
                return;

            //Ensure the object is updatable and drawable
            AddActiveUpdateable();

            ForEachComponent(c => c.DoOnUnpaused());
            OnUnpaused();

            ForEachChild(c =>
            {
                c._pausedParent = false;
                if(!_pausedSelf)
                    c.DoOnUnpaused();
            });
        }


        /// <summary>
        /// Called when the game object is Showed
        /// </summary>
        protected virtual void OnShowed()
        {

        }

        /// <summary>
        /// Execute the OnShowed
        /// </summary>
        internal void DoOnShowed()
        {
            //If the parent is hidden, then, we are not changing our state
            if (!_loaded || !_visibleParent)
                return;


            //Ensure the object is drawable
            AddActiveDrawable();

            ForEachComponent(c => c.DoOnShowed());
            OnShowed();

            ForEachChild(c =>
            {
                c._visibleParent = true;
                if (c._visibleSelf)
                    c.DoOnShowed();
            });
        }

        /// <summary>
        /// Called when the game object is Hided
        /// </summary>
        protected virtual void OnHided()
        {

        }

        /// <summary>
        /// Execute the OnHided
        /// </summary>
        internal void DoOnHided()
        {
            //If the parent was already hidden, then, we are not changing our state
            if (!_loaded || !_visibleParent)
                return;

            //Ensure the object is not drawable
            RemoveActiveDrawable();

            ForEachComponent(c => c.DoOnHided());
            OnHided();

            ForEachChild(c =>
            {
                c.DoOnHidedParent();
            });
        }

        /// <summary>
        /// Execute the OnHided when the visibility of the parent change
        /// </summary>
        internal void DoOnHidedParent()
        {
            _visibleParent = false;

            if (_visibleSelf)
            {
                //Ensure the object is not drawable
                RemoveActiveDrawable();

                ForEachComponent(c => c.DoOnHided());
                OnHided();
            }

            ForEachChild(c =>
            {
                c.DoOnHidedParent();
            });
        }



        /// <summary>
        /// Execute the OnAdded
        /// </summary>
        internal void DoOnAdded(bool processChildren)
        {
            _isOnScene = true;

            //Ensure the object is updatable and drawable
            AddActiveUpdateable();
            AddActiveDrawable();

            if (processChildren)
            {
                //And the children because they are not really removed but we will want to known if the parent is removed.
                ForEachChild(o => o.DoOnAdded(true));
            }



            //Call OnAdded for components...
            ForEachComponent(o => o.DoOnAdded());

            //Little event OnAdded...
            this.OnAdded();
        }

        /// <summary>
        /// Called when the game object is added in another game object as a child
        /// </summary>
        protected virtual void OnAdded()
        {

        }
        /// <summary>
        /// Execute the OnRemoved
        /// </summary>
        internal void DoOnRemoved()
        {
            _isOnScene = false;

            //Ensure the object is not updatable and not drawable
            RemoveActiveUpdateable();
            RemoveActiveDrawable();

            //And the children because they are not really removed but we will want to known if the parent is removed.
            ForEachChild(o => o.DoOnRemoved());

            Mouse.RemoveGameObject(this);

            //Call OnRemoved for components...
            ForEachComponent(o => o.DoOnRemoved());

            //Little event OnRemoved...
            this.OnRemoved();
        }

        /// <summary>
        /// Called when the game object is removed in another game object as a child
        /// </summary>
        protected virtual void OnRemoved()
        {

        }

        /// <summary>
        /// Execute the OnResized
        /// </summary>
        internal void DoOnResized()
        {
            if (!_loaded)
                return;

            OnResized();
        }

        /// <summary>
        /// Called when the game object is resized
        /// </summary>
        protected virtual void OnResized()
        {

        }

        /// <summary>
        /// Execute the OnMoved
        /// </summary>
        internal void DoOnMoved()
        {
            if (!_loaded)
                return;

            OnMoved();
        }

        /// <summary>
        /// Called when the game object has moved
        /// </summary>
        protected virtual void OnMoved()
        {

        }

        /// <summary>
        /// Add the current component to the active list
        /// </summary>
        private void AddActiveUpdateable()
        {

            if (_updateable != null)
            {
                if (!_presentInActiveUpdateList)
                {
                    if (this.Enabled && !this.Paused)
                    {
                        _game.AddUpdateable(_updateable);
                        _presentInActiveUpdateList = true;
                    }
                }
            }

            //if (processChildren)
            //    ForEachChild(c => c.AddActiveUpdateable(true));

        }

        /// <summary>
        /// Add the current component to the active list
        /// </summary>
        private void AddActiveDrawable()
        {
            if (_drawable != null)
            {
                if (!_presentInActiveDrawList)
                {
                    if (this.Enabled && this.Visible)
                    {
                        _game.AddDrawable(_drawable);
                        _presentInActiveDrawList = true;
                    }
                }
            }

            //if (processChildren)
            //    ForEachChild(c => c.AddActiveUpdateable(true));

        }

        /// <summary>
        /// Remove the current component from the active list
        /// </summary>
        private void RemoveActiveUpdateable()
        {
            if (_updateable == null)
                return;

            if (_presentInActiveUpdateList)
            {
                _game.RemoveUpdateable(_updateable);
                _presentInActiveUpdateList = false;
            }
        }

        /// <summary>
        /// Remove the current component from the active list
        /// </summary>
        private void RemoveActiveDrawable()
        {
            if (_drawable == null)
                return;

            if (_presentInActiveDrawList)
            {
                _game.RemoveDrawable(_drawable);
                _presentInActiveDrawList = false;
            }
        }

        /// <summary>
        /// Destruction du game object
        /// </summary>
        public virtual void Destroy()
        {
            if (this.Parent == null)
                throw new InvalidOperationException("Impossible to destroy the root game object or already been destroyed.");

            //Simply removing this...
            this.Parent.Remove(this);

        }


        /// <summary>
        /// Permet de déplacer en X l'objet et tous ses enfants
        /// </summary>
        public void TranslateX(float offsetX)
        {
            if (offsetX == 0)
                return;
            if (offsetX == float.NaN)
                throw new InvalidOperationException("offsetX cannot be NaN.");

            _location.X += offsetX;

            if (_loaded)
                OnMoved();

            ForEachChild(o => o.TranslateX(offsetX));

            ForEachComponent(c => c.DoOnMoved());

        }

        /// <summary>
        /// Permet de déplacer en Y l'objet et tous ses enfants
        /// </summary>
        public void TranslateY(float offsetY)
        {
            if (offsetY == 0)
                return;
            if (offsetY == float.NaN)
                throw new InvalidOperationException("offsetY cannot be NaN.");

            _location.Y += offsetY;

            if (_loaded)
                OnMoved();

            ForEachChild(o => o.TranslateY(offsetY));

            ForEachComponent(c => c.DoOnMoved());

        }


        /// <summary>
        /// Permet de déplacer en X et Y l'objet et tous ses enfants
        /// </summary>
        public GameObject Translate(float offsetX, float offsetY)
        {
            if (offsetX == 0 && offsetY == 0)
                return this;

            if (offsetX == float.NaN)
                throw new InvalidOperationException("offsetX cannot be NaN.");
            if (offsetY == float.NaN)
                throw new InvalidOperationException("offsetY cannot be NaN.");

            _location.X += offsetX;
            _location.Y += offsetY;

            if (_loaded)
                OnMoved();

            ForEachChild(o => o.Translate(offsetX, offsetY));

            ForEachComponent(c => c.DoOnMoved());

            return this;
        }

        /// <summary>
        /// Permet de déplacer en X et Y l'objet et tous ses enfants
        /// </summary>
        public GameObject Translate(Vector2 translation)
        {
            return this.Translate(translation.X, translation.Y);
        }

        /// <summary>
        /// Permet de déplacer vers une coordonnées
        /// </summary>
        public GameObject TranslateTo(float x, float y)
        {
            return this.Translate(x - _location.X, y - _location.Y);
        }

        /// <summary>
        /// Permet de déplacer vers une coordonnées
        /// </summary>
        public GameObject TranslateTo(Vector2 destination)
        {
            return this.Translate(destination.X - _location.X, destination.Y - _location.Y);
        }

        /// <summary>
        /// Permet de déplacer vers une coordonnées
        /// </summary>
        public GameObject TranslateTo(Point destination)
        {
            return this.Translate(destination.X - _location.X, destination.Y - _location.Y);
        }


        /// <summary>
        /// Resize width of an offset
        /// </summary>
        public GameObject ResizeWidth(float offsetX)
        {
            if (offsetX == 0)
                return this;
            if (offsetX == float.NaN)
                throw new InvalidOperationException("offsetX cannot be NaN.");

            _size.X += offsetX;

            if (_loaded)
                OnResized();

            ForEachChild(o => o.ResizeWidth(offsetX));

            ForEachComponent(c => c.DoOnResized());

            return this;

        }

        /// <summary>
        /// Resize height of an offset
        /// </summary>
        public GameObject ResizeHeight(float offsetY)
        {
            if (offsetY == 0)
                return this;
            if (offsetY == float.NaN)
                throw new InvalidOperationException("offsetX cannot be NaN.");

            _size.Y += offsetY;

            if (_loaded)
                OnResized();

            ForEachChild(o => o.ResizeHeight(offsetY));

            ForEachComponent(c => c.DoOnResized());

            return this;

        }


        /// <summary>
        /// Resize width and height of an offset
        /// </summary>
        public GameObject Resize(float offsetX, float offsetY)
        {
            if (offsetX == 0 && offsetY == 0)
                return this;
            if (offsetX == float.NaN)
                throw new InvalidOperationException("offsetX cannot be NaN.");
            if (offsetY == float.NaN)
                throw new InvalidOperationException("offsetY cannot be NaN.");

            _size.X += offsetX;
            _size.Y += offsetY;

            if (_loaded)
                OnResized();

            ForEachChild(o => o.Resize(offsetX, offsetY));

            ForEachComponent(c => c.DoOnResized());

            return this;

        }

        /// <summary>
        /// Resize to a width and height
        /// </summary>
        public GameObject ResizeTo(float offsetX, float offsetY)
        {
            return this.Resize(offsetX - _size.X, offsetY - _size.Y);
        }



        /// <summary>
        /// Set the layer mask
        /// </summary>
        public GameObject SetLayerMask(Layers layerMask)
        {
            this.LayerMask = layerMask;
            return this;

        }

        /// <summary>
        /// Set the depth
        /// </summary>
        public GameObject SetDepth(float depth)
        {
            this.Depth = depth;
            return this;
        }

        /// <summary>
        /// Enable the default collider rectangle
        /// </summary>
        public virtual GameObject EnableCollider()
        {
            if (!_collidable)
            {
                _collidable = true;

                //Already enabled?
                if (_collider != null)
                    throw new Exception("Already a collider attached to this game object.");

                AddComponent<ColliderRectangle>();
            }

            return this;

        }

        /// <summary>
        /// Enable the default collider rectangle
        /// </summary>
        public virtual GameObject EnableColliderCircle()
        {
            if (!_collidable)
            {
                _collidable = true;

                //Already enabled?
                if (_collider != null)
                    throw new Exception("Already a collider attached to this game object.");

                //Add a collider at the center of the game object
                AddComponent(new ColliderCircle(new Vector2(this.Width * 0.5f), this.Width * 0.5f));
            }

            return this;

        }

        /// <summary>
        /// Disable the collider
        /// </summary>
        public virtual GameObject DisableCollider()
        {
            _collidable = false;

            //Already disabled?
            if (_collider != null)
                RemoveComponent<Collider>();

            return this;
        }


        /// <summary>
        /// Permet d'obtenir la première collision
        /// </summary>
        public Collision GetCollision(Type[] types)
        {
            ColliderContainer container = _game.ColliderContainer;

            if (container.IsEmpty)
                return null;

            Collider collider = _collider;
            if (collider == null)
                collider = new ColliderRectangle(this);

            collider.OrignalMovingLocation = this._location;
            collider.MovingLocation = this._location;

            return container.GetCollision(collider, types);
        }

        /// <summary>
        /// Permet d'obtenir la première collision
        /// </summary>
        public Collision GetCollision(Vector2 position, Type[] types)
        {
            ColliderContainer container = _game.ColliderContainer;

            if (container.IsEmpty)
                return null;

            Collider collider = _collider;
            if (collider == null)
                collider = new ColliderRectangle(this);

            collider.OrignalMovingLocation = position;
            collider.MovingLocation = position;

            return container.GetCollision(collider, types);
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
            ColliderContainer container = _game.ColliderContainer;

            if (container.IsEmpty)
                return null;

            ColliderRectangle collider = new ColliderRectangle(this);
            collider.OrignalMovingLocation = position;
            collider.MovingLocation = position;
            collider.Size = size;

            return container.GetCollision(collider, types);
        }


        /// <summary>
        /// Hide a object
        /// </summary>
        public virtual void Hide()
        {
            if (_visibleSelf)
            {
                _visibleSelf = false;

                DoOnHided();
            }
        }

        /// <summary>
        /// Show a object
        /// </summary>
        public virtual void Show()
        {
            if (!_visibleSelf)
            {
                _visibleSelf = true;

                DoOnShowed();
            }
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
        /// Get a service
        /// </summary>
        public T GetService<T>()
        {
            return (T)_game.Services.GetService(typeof(T));
        }

        /// <summary>
        /// Get a content
        /// </summary>
        public Content<T> GetContent<T>(string assetName)
        {
            return _game.ContentManager.GetContent<T>(assetName);
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
