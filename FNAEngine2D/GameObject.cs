using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Vector2Int Size
        {
            get { return new Vector2Int(this.Width, this.Height); }
            set
            {
                this.Bounds.Width = (int)value.X;
                this.Bounds.Height = (int)value.Y;
            }
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

            return gameObject;
        }


        /// <summary>
        /// Chargement du content
        /// </summary>
        public virtual void Load()
        {
            //if (this.Childrens.Count == 0)
            //    return;

            //for (int index = 0; index < this.Childrens.Count; index++)
            //    this.Childrens[index].LoadContent();

        }

        /// <summary>
        /// Chargement du content
        /// </summary>
        internal void LoadWithChildren()
        {
            this.Load();

            if (this.Childrens.Count == 0)
                return;

            for (int index = 0; index < this.Childrens.Count; index++)
                this.Childrens[index].LoadWithChildren();

        }

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
                this.Childrens[index].DrawWithChildren();
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
        /// Active le collider
        /// </summary>
        public void EnableCollider()
        {
            if (this.RootGameObject == null)
                throw new InvalidOperationException("Impossible to enable collider on root game object.");

            if (this.RootGameObject._colliderContainer == null)
                this.RootGameObject._colliderContainer = new ColliderContainer();

            _collider = new Collider(this);
            AddCollider(_collider);
        }

        /// <summary>
        /// Active le collider
        /// </summary>
        public void AddCollider(Collider collider)
        {
            GetColliderContainer().Add(collider);
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
        /// Permet de trouver le collider container où tout mettre les collider
        /// </summary>
        private ColliderContainer GetColliderContainer()
        {
            if (this.RootGameObject._colliderContainer == null)
                this.RootGameObject._colliderContainer = new ColliderContainer();

            return this.RootGameObject._colliderContainer;
        }


    }
}
