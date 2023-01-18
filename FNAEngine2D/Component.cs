using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Component
    /// </summary>
    public class Component
    {
        /// <summary>
        /// Indicate if the component is present in the active list
        /// </summary>
        private bool _presentInActiveList = false;

        /// <summary>
        /// GameObject
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public GameObject GameObject { get; set; }

        /// <summary>
        /// Drawing context
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public DrawingContext DrawingContext
        {
            get { return this.GameObject.DrawingContext; }
        }

        /// <summary>
        /// ElapsedGameTimeSeconds
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float ElapsedGameTimeSeconds { get { return this.GameObject.ElapsedGameTimeSeconds; } }

        /// <summary>
        /// ElapsedGameTimeMilliseconds
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float ElapsedGameTimeMilliseconds { get { return this.GameObject.ElapsedGameTimeMilliseconds; } }

        /// <summary>
        /// Number of pixels = 1 meter
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float NbPixelPerMeter { get { return this.GameObject.NbPixelPerMeter; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public Component()
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

        ///// <summary>
        ///// Execute the Update
        ///// </summary>
        //internal void DoUpdate()
        //{
        //    Update();
        //}

        ///// <summary>
        ///// Update each frame
        ///// </summary>
        //protected virtual void Update()
        //{

        //}

        ///// <summary>
        ///// Execute the Draw
        ///// </summary>
        //internal void DoDraw()
        //{
        //    Draw();
        //}

        ///// <summary>
        ///// Draw each frame
        ///// </summary>
        //protected virtual void Draw()
        //{

        //}

        /// <summary>
        /// Execute the OnAdded
        /// </summary>
        internal void DoOnAdded()
        {
            AddActiveComponent();

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
        internal void DoOnRemoved()
        {
            RemoveActiveComponent();

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
        internal void DoOnResized()
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
        internal void DoOnMoved()
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
            AddActiveComponent();
            OnEnabled();
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
            RemoveActiveComponent();
            OnDisabled();
        }

        /// <summary>
        /// Add the current component to the active list
        /// </summary>
        private void AddActiveComponent()
        {
            if (!_presentInActiveList)
            {
                this.GameObject.Game.AddActiveComponent(this);

                _presentInActiveList = true;
            }
        }

        /// <summary>
        /// Remove the current component from the active list
        /// </summary>
        private void RemoveActiveComponent()
        {
            if (_presentInActiveList)
            {
                this.GameObject.Game.RemoveActiveComponent(this);

                _presentInActiveList = false;
            }
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
