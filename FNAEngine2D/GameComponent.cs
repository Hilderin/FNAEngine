using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// GameComponent
    /// </summary>
    public class GameComponent
    {
        /// <summary>
        /// GameObject
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameComponent()
        {
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public GameComponent(GameObject gameObject)
        //{
        //    this.GameObject = gameObject;
        //}

        /// <summary>
        /// Loading
        /// </summary>
        public virtual void Load()
        {

        }

        /// <summary>
        /// Update each frame
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Called when the game object is added in another game object as a child
        /// </summary>
        public virtual void OnAdded()
        {
            
        }

        /// <summary>
        /// Called when the game object is removed in another game object as a child
        /// </summary>
        public virtual void OnRemoved()
        {

        }

    }
}
