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
        /// Drawing context
        /// </summary>
        public DrawingContext DrawingContext
        {
            get { return this.GameObject.DrawingContext; }
        }

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
        /// Execute the Load
        /// </summary>
        internal void DoLoad()
        {
            Load();
        }

        /// <summary>
        /// Loading
        /// </summary>
        protected virtual void Load()
        {

        }

        /// <summary>
        /// Execute the Update
        /// </summary>
        internal void DoUpdate()
        {
            Update();
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        protected virtual void Update()
        {

        }

        /// <summary>
        /// Execute the Draw
        /// </summary>
        internal void DoDraw()
        {
            Draw();
        }

        /// <summary>
        /// Draw each frame
        /// </summary>
        protected virtual void Draw()
        {

        }

        /// <summary>
        /// Execute the OnAdded
        /// </summary>
        internal virtual void DoOnAdded()
        {
            OnAdded();
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
        internal virtual void DoOnRemoved()
        {
            OnRemoved();
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
        internal virtual void DoOnResized()
        {
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
        internal virtual void DoOnMoved()
        {
            OnMoved();
        }

        /// <summary>
        /// Called when the game object has moved
        /// </summary>
        protected virtual void OnMoved()
        {

        }


        /// <summary>
        /// Get a content
        /// </summary>
        public Content<T> GetContent<T>(string assetName)
        {
            return this.GameObject.GetContent<T>(assetName);
        }
    }
}
