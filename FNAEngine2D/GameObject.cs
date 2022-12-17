using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// RenderObject
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// Gère le loading des childrens
        /// </summary>
        private bool _loaded = false;

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
        /// RootGameObject
        /// </summary>
        public GameObject RootGameObject;

        /// <summary>
        /// Parent GameObject
        /// </summary>
        public GameObject Parent;

        /// <summary>
        /// Bounds
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Childrens
        /// </summary>
        public List<GameObject> Childrens { get; private set; } = new List<GameObject>();

        /// <summary>
        /// Indique si l'objet est paused
        /// </summary>
        public bool Paused;

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
        /// Position X
        /// </summary>
        public int X
        {
            get { return this.Bounds.X; }
            set { this.Bounds.X = value; }
        }

        /// <summary>
        /// Position Y
        /// </summary>
        public int Y
        {
            get { return this.Bounds.Y; }
            set { this.Bounds.Y = value; }
        }

        /// <summary>
        /// Width
        /// </summary>
        public int Width
        {
            get { return this.Bounds.Width; }
            set { this.Bounds.Width = value; }
        }

        /// <summary>
        /// Height
        /// </summary>
        public int Height
        {
            get { return this.Bounds.Height; }
            set { this.Bounds.Height = value; }
        }

        /// <summary>
        /// Right
        /// </summary>
        public int Right
        {
            get { return this.Bounds.Right; }
            set { this.Bounds.X = value - this.Width; }
        }

        /// <summary>
        /// Bottom
        /// </summary>
        public int Bottom
        {
            get { return this.Bounds.Bottom; }
            set { this.Bounds.Y = value - this.Height; }
        }

        /// <summary>
        /// Position
        /// </summary>
        public Vector2 Location
        {
            get { return new Vector2(this.X, this.Y); }
            set
            {
                this.Bounds.X = (int)value.X;
                this.Bounds.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// Position
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(this.X, this.Y); }
            set
            {
                this.Bounds.X = (int)value.X;
                this.Bounds.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// Rectangle
        /// </summary>
        public Rectangle Rectangle
        {
            get { return this.Bounds; }
            set { this.Bounds = value; }
        }

        /// <summary>
        /// Size
        /// </summary>
        public Point Size
        {
            get { return new Point(this.Width, this.Height); }
            set
            {
                this.Bounds.Width = (int)value.X;
                this.Bounds.Height = (int)value.Y;
            }
        }

        /// <summary>
        /// Center en X
        /// </summary>
        public int CenterX
        {
            get { return this.Bounds.X + (this.Bounds.Width / 2); }
        }

        /// <summary>
        /// Center en Y
        /// </summary>
        public int CenterY
        {
            get { return this.Bounds.Y + (this.Bounds.Height / 2); }
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
        /// Ajout d'un render object enfant
        /// </summary>
        public T Add<T>(T gameObject) where T : GameObject
        {
            gameObject.Parent = this;
            gameObject.RootGameObject = this.RootGameObject;
                      

            this.Childrens.Add(gameObject);

            //Already a collider? can happen if remove and readded...
            AddColliders(gameObject);

            //If not loaded already... can happen if remove and readded...
            if (!gameObject._loaded)
                gameObject.Load();

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

            this.Childrens.Remove(gameObject);

            MouseManager.RemoveGameObject(gameObject);
        }

        /// <summary>
        /// Remove all the gameobject of a certain type
        /// </summary>
        public void Remove(Type gameObjectType)
        {
            for (int index = this.Childrens.Count - 1; index >= 0; index--)
            {
                if (this.Childrens[index].GetType() == gameObjectType)
                {
                    Remove(this.Childrens[index]);
                }
            }
        }

        /// <summary>
        /// Remove all the gameobject
        /// </summary>
        public void RemoveAll()
        {
            for (int index = this.Childrens.Count - 1; index >= 0; index--)
            {
                Remove(this.Childrens[index]);
            }
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
            //Paused?
            if (this.Paused)
                return;

            this.Update();

            if (this.Childrens.Count == 0)
                return;

            for (int index = 0; index < this.Childrens.Count; index++)
                this.Childrens[index].UpdateWithChildren();
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
            this.Draw();

            if (this.Childrens.Count == 0)
                return;

            for (int index = 0; index < this.Childrens.Count; index++)
            {
                if (this.Childrens[index].Visible)
                    this.Childrens[index].DrawWithChildren();
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
            this.Parent.Childrens.Remove(this);

            //Retrait du collider...
            if (_collider != null)
                GetColliderContainer().Remove(_collider);
        }


        /// <summary>
        /// Permet de déplacer en X l'objet et tous ses enfants
        /// </summary>
        public void TranslateX(int offsetX)
        {
            this.X += offsetX;

            if (this.Childrens.Count == 0)
                return;

            for (int index = 0; index < this.Childrens.Count; index++)
                this.Childrens[index].TranslateX(offsetX);

        }

        /// <summary>
        /// Permet de déplacer en X l'objet et tous ses enfants
        /// </summary>
        public void TranslateX(float offsetX)
        {
            TranslateX((int)offsetX);
        }

        /// <summary>
        /// Permet de déplacer en Y l'objet et tous ses enfants
        /// </summary>
        public void TranslateY(int offsetY)
        {
            this.Y += offsetY;

            if (this.Childrens.Count == 0)
                return;

            for (int index = 0; index < this.Childrens.Count; index++)
                this.Childrens[index].TranslateY(offsetY);

        }

        /// <summary>
        /// Permet de déplacer en Y l'objet et tous ses enfants
        /// </summary>
        public void TranslateY(float offsetY)
        {
            TranslateY((int)offsetY);
        }


        /// <summary>
        /// Permet de déplacer en X et Y l'objet et tous ses enfants
        /// </summary>
        public void Translate(int offsetX, int offsetY)
        {
            this.X += offsetX;
            this.Y += offsetY;


            if (this.Childrens.Count == 0)
                return;

            for (int index = 0; index < this.Childrens.Count; index++)
                this.Childrens[index].Translate(offsetX, offsetY);

        }

        /// <summary>
        /// Permet de déplacer en X et Y l'objet et tous ses enfants
        /// </summary>
        public void Translate(float offsetX, float offsetY)
        {
            Translate((int)offsetX, (int)offsetY);
        }

        /// <summary>
        /// Permet de déplacer en X et Y l'objet et tous ses enfants
        /// </summary>
        public void Translate(Vector2 translation)
        {
            this.Translate((int)translation.X, (int)translation.Y);
        }

        /// <summary>
        /// Permet de déplacer vers une coordonnées
        /// </summary>
        public void TranslateTo(Vector2 destination)
        {
            this.Translate(destination.X - this.X, destination.Y - this.Y);
        }

        /// <summary>
        /// Permet de déplacer vers une coordonnées
        /// </summary>
        public void TranslateTo(Point destination)
        {
            this.Translate(destination.X - this.X, destination.Y - this.Y);
        }

        /// <summary>
        /// Active le collider
        /// </summary>
        public void EnableCollider()
        {
            //Already enabled?
            if (_collider != null)
                return;

            //RootGameObject is never really added anywhere...
            if (this == GameHost.RootGameObject)
                throw new InvalidOperationException("Impossible to enable collider on root game object.");


            _collider = new Collider(this);

            //If not already added, we will add it in the ColliderContainer on Add
            if (this.Parent == null)
                return;

            //Add to container...
            GetColliderContainer().Add(_collider);
        }

        /// <summary>
        /// Permet d'obtenir la première collision
        /// </summary>
        public Collision GetCollision()
        {
            ColliderContainer container = GetColliderContainer();

            if (container.IsEmpty)
                return null;

            if (_collider == null)
                return container.GetCollision(new Rectangle(this.X, this.Y, this.Width, this.Height), _collider);
            else
                return container.GetCollision(_collider.Bounds, _collider);
        }

        /// <summary>
        /// Permet d'obtenir la première collision
        /// </summary>
        public Collision GetCollision(int nextX, int nextY)
        {
            ColliderContainer container = GetColliderContainer();

            if (container.IsEmpty)
                return null;

            return container.GetCollision(new Rectangle(nextX, nextY, this.Width, this.Height), _collider);
        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public List<Collision> GetCollisions()
        {
            ColliderContainer container = GetColliderContainer();

            if (container.IsEmpty)
                return CollisionHelper.EMPTY_COLLISIONS;

            if (_collider == null)
                return container.GetCollisions(new Rectangle(this.X, this.Y, this.Width, this.Height), _collider);
            else
                return container.GetCollisions(_collider.Bounds, _collider);

        }

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public List<Collision> GetCollisions(int nextX, int nextY)
        {
            ColliderContainer container = GetColliderContainer();

            if (container.IsEmpty)
                return CollisionHelper.EMPTY_COLLISIONS;

            return container.GetCollisions(new Rectangle(nextX, nextY, this.Width, this.Height), _collider);
        }

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

            if (gameObject.Childrens.Count > 0)
            {
                for (int index = 0; index < gameObject.Childrens.Count; index++)
                {
                    AddColliders(gameObject.Childrens[index]);
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

            if (gameObject.Childrens.Count > 0)
            {
                for (int index = 0; index < gameObject.Childrens.Count; index++)
                {
                    RemoveColliders(gameObject.Childrens[index]);
                }
            }

        }


    }
}
