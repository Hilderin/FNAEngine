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
        /// Childrens
        /// </summary>
        public List<GameObject> Childrens { get; private set; } = new List<GameObject>();


        /// <summary>
        /// Ajout d'un render object enfant
        /// </summary>
        public T Add<T>(T gameObject) where T : GameObject
        {
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

    }
}
