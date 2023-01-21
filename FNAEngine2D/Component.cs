using Microsoft.Xna.Framework;
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
        /// GameObject
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public GameObject GameObject { get; set; }

        /// <summary>
        /// LayerMask from the game object
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public Layers LayerMask { get { return this.GameObject.LayerMask; } }

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
        /// UpdateOrder when this component is an IUpdate
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public float UpdateOrder { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Component()
        {
            _updateable = this as IUpdate;
            _drawable = this as IDraw;
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
            
            AddActiveUpdateable();
            AddActiveDrawable();
            

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
            RemoveActiveUpdateable();
            RemoveActiveDrawable();

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
            AddActiveUpdateable();
            AddActiveDrawable();

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
            RemoveActiveUpdateable();
            RemoveActiveDrawable();

            OnDisabled();
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
            RemoveActiveUpdateable();
            OnPaused();
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
            AddActiveUpdateable();
            OnUnpaused();
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
            AddActiveDrawable();

            OnShowed();
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
            RemoveActiveDrawable();

            OnHided();
        }


        /// <summary>
        /// Add the current component to the active list
        /// </summary>
        private void AddActiveUpdateable()
        {
            if (_updateable == null)
                return;

            if (!_presentInActiveUpdateList)
            {
                if (this.GameObject.Enabled && !this.GameObject.Paused)
                {
                    this.GameObject.Game.AddUpdateable(_updateable);
                    _presentInActiveUpdateList = true;
                }
            }

        }

        /// <summary>
        /// Add the current component to the active list
        /// </summary>
        private void AddActiveDrawable()
        {
            if (_drawable == null)
                return;

            if (!_presentInActiveDrawList)
            {
                if (this.GameObject.Enabled && this.GameObject.Visible)
                {
                    this.GameObject.Game.AddDrawable(_drawable);
                    _presentInActiveDrawList = true;
                }
            }
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
                this.GameObject.Game.RemoveUpdateable(_updateable);
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
                this.GameObject.Game.RemoveDrawable(_drawable);
                _presentInActiveDrawList = false;
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
