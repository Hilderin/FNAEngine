using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using FNAEngine2D.Desginer;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Permissions;
using FNAEngine2D.Network;
using FNAEngine2D.Collisions;

namespace FNAEngine2D
{
    /// <summary>
    /// Internal Game XNA to hide the complexity to the programmer
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Content Manager
        /// </summary>
        private ContentManager _contentManager;

        /// <summary>
        /// Graphic Manager
        /// </summary>
        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// Graphic device
        /// </summary>
        private GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Load content done?
        /// </summary>
        private bool _loadContentDone = false;

        /// <summary>
        /// Root game object
        /// </summary>
        private GameObject _rootGameObject;

        /// <summary>
        /// Default Camera
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        /// Timer to calculate time for frames
        /// </summary>
        private Stopwatch _internalTimer = Stopwatch.StartNew();

        /// <summary>
        /// Indicate if we are drawing
        /// </summary>
        private bool _inDrawing = false;

        /// <summary>
        /// Real size of the screen
        /// </summary>
        private Point _screenSize;

        /// <summary>
        /// Size of the game
        /// </summary>
        private Point _gameSize;

        /// <summary>
        /// Bounds of the game
        /// </summary>
        private Rectangle _gameRectangle;

        /// <summary>
        /// List of the other cameras on the scene
        /// </summary>
        private List<Camera> _extraCameras = new List<Camera>();


        /// <summary>
        /// Collider container
        /// </summary>
        private ColliderContainer _colliderContainer = new ColliderContainer();

        /// <summary>
        /// Game time service
        /// </summary>
        private GameTimeService _gameTimeService;

        /// <summary>
        /// Drawing context
        /// </summary>
        private DrawingContext _drawingContext;

        /// <summary>
        /// Service for the edit mode
        /// </summary>
        private EditModeService _editModeService;

        /// <summary>
        /// Game content manager
        /// </summary>
        private GameContentManager _gameContentManager;

        /// <summary>
        /// Input system
        /// </summary>
        private InputManager _input;

        /// <summary>
        /// Mouse manager
        /// </summary>
        private MouseManager _mouseManager;


        /// <summary>
        /// Updatables elements
        /// </summary>
        private List<IUpdate> _updateables = new List<IUpdate>();

        /// <summary>
        /// Updatables elements to add
        /// </summary>
        private List<IUpdate> _updateablesAdded = new List<IUpdate>();

        /// <summary>
        /// Updatables elements to remove
        /// </summary>
        private List<IUpdate> _updateablesDestroyed = new List<IUpdate>();

        /// <summary>
        /// Drawables elements
        /// </summary>
        private List<IDraw> _drawables = new List<IDraw>();

        /// <summary>
        /// Singleton instance
        /// </summary>
        internal static Game Current { get; private set; }

        /// <summary>
        /// Indique si l'objet est initialisé
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// The scale result of merging Internal size with Screen size.
        /// </summary>
        public Vector2 ScreenScale { get; private set; } = Vector2.One;

        /// <summary>
        /// The scale used for beginning the SpriteBatch.
        /// </summary>
        public Matrix ScaleMatrix { get; private set; }

        /// <summary>
        /// Number of pixels = 1 meter
        /// </summary>
        public float NbPixelPerMeter { get; set; } = 100;

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.Gray;

        /// <summary>
        /// Run in a dedicated server mode
        /// </summary>
        public bool IsDedicatedServer { get; private set; }

        ///// <summary>
        ///// Indicate if we are the client
        ///// </summary>
        //public bool IsClient { get { return GameManager.IsClient; } set { GameManager.IsClient = value; } }

        ///// <summary>
        ///// Indicate if we are the server
        ///// </summary>
        //public bool IsServer { get { return GameManager.IsServer; } set { GameManager.IsServer = value; } }


        /// <summary>
        /// Root game object
        /// </summary>
        public GameObject RootGameObject
        {
            get { return _rootGameObject; }
            set
            {
                if (_loadContentDone)
                    throw new InvalidOperationException("Impossible to change the root game object once the game has been started.");

                _rootGameObject = value;

                //if (_rootGameObject != value)
                //{
                //    //Removing the old one, in case we are changing the root game object...
                //    if (_rootGameObject != null)
                //    {
                //        _rootGameObject.ForEachChild(o => o.DoOnRemoved());
                //        _rootGameObject.DoOnRemoved();
                //    }

                //    _rootGameObject = value;

                //    //LoadContent alread done, we will do it ourself here...
                //    if (_loadContentDone)
                //    {
                //        if (!_rootGameObject._loaded)
                //        {
                //            _rootGameObject.DoLoad();
                //            _rootGameObject.DoOnAdded(false);
                //        }
                //        else
                //        {
                //            //Already loaded so we will reexecute all the OnAdded
                //            _rootGameObject.DoOnAdded(true);
                //        }
                //    }

                //    //This game objet will be the root
                //    value.RootGameObject = value;

                //}

            }
        }

        /// <summary>
        /// Default Camera
        /// </summary>
        public Camera MainCamera
        {
            get { return _mainCamera; }
            set { _mainCamera = value; }
        }

        ///// <summary>
        ///// Current camera that is rendering
        ///// </summary>
        //public Camera CurrentCamera { get { return _currentCamera; } }

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get { return _gameSize.X; } }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get { return _gameSize.Y; } }

        /// <summary>
        /// Size
        /// </summary>
        public Point Size { get { return _gameSize; } }

        /// <summary>
        /// Center X
        /// </summary>
        public int CenterX { get { return _gameSize.X / 2; } }

        /// <summary>
        /// Center Y
        /// </summary>
        public int CenterY { get { return _gameSize.Y / 2; } }

        /// <summary>
        /// ScreenSize
        /// </summary>
        public Point ScreenSize { get { return _screenSize; } }

        /// <summary>
        /// Rectangle
        /// </summary>
        public Rectangle Rectangle { get { return _gameRectangle; } }

        /// <summary>
        /// Bounds
        /// </summary>
        public Rectangle Bounds { get { return _gameRectangle; } }

        /// <summary>
        /// Access the content Manager
        /// </summary>
        public ContentManager ContentManager { get { return _contentManager; } }

        ///// <summary>
        ///// List of the other cameras on the scene
        ///// </summary>
        //public List<Camera> ExtraCameras { get { return _extraCameras; } }

        /// <summary>
        /// Handle to the Window (the Win32 handle)
        /// </summary>
        public IntPtr GameWindowHandle { get; set; }

        /// <summary>
        /// Indicate if the game has ended
        /// </summary>
        public bool Exited { get; private set; }

        /// <summary>
        /// Collider container
        /// </summary>
        public ColliderContainer ColliderContainer { get { return _colliderContainer; } }

        /// <summary>
        /// DrawingContext
        /// </summary>
        public DrawingContext DrawingContext { get { return _drawingContext; } }

        /// <summary>
        /// GameContentManager
        /// </summary>
        public GameContentManager GameContentManager { get { return _gameContentManager; } }

        /// <summary>
        /// EditModeService
        /// </summary>
        public EditModeService EditModeService { get { return _editModeService; } }

        /// <summary>
        /// Input
        /// </summary>
        public InputManager Input { get { return _input; } }

        /// <summary>
        /// MouseManager
        /// </summary>
        public MouseManager Mouse { get { return _mouseManager; } }

        /// <summary>
        /// ElapsedGameTimeSeconds
        /// </summary>
        public float ElapsedGameTimeSeconds { get; private set; }

        /// <summary>
        /// ElapsedGameTimeMilliseconds
        /// </summary>
        public float ElapsedGameTimeMilliseconds { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Game() : this(GraphicSettings.Default)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Game(GraphicSettings graphicSettings)
        {
            //Current game...
            Current = this;

            ServiceProvider.Container = this.Services;

            _graphics = new GraphicsDeviceManager(this);

            //Update graphics...
            _graphics.PreferredBackBufferWidth = graphicSettings.ScreenSize.X;
            _graphics.PreferredBackBufferHeight = graphicSettings.ScreenSize.Y;
            _graphics.IsFullScreen = graphicSettings.IsFullscreen;
            _graphics.ApplyChanges();

            //Default Camera = a static camera 
            _mainCamera = new Camera(graphicSettings.GameSize.X, graphicSettings.GameSize.Y);
            _mainCamera.Game = this;

            //Init gameSize so the SetResolution cam calculate the diff for the resize of the camera...
            _screenSize = graphicSettings.ScreenSize;
            _gameSize = graphicSettings.GameSize;

            //Création du Content Manager..
            _contentManager = new ContentManager(this.Services, ContentManager.ContentFolder);
            this.Content = _contentManager;

            //GameTime service
            _gameTimeService = new GameTimeService(this.Services);

            //Drawing context
            _drawingContext = new DrawingContext();

            //Game content manager
            _gameContentManager = new GameContentManager(this);

            //Input
            _input = new InputManager(this);

            //Mouse
            _mouseManager = new MouseManager(this);

            //Setting de default resolution...
            UpdateGraphicSettings(graphicSettings);

            //We use fixed timeframe so... that should never change...
            this.ElapsedGameTimeSeconds = (float)this.TargetElapsedTime.TotalSeconds;
            this.ElapsedGameTimeMilliseconds = (float)this.TargetElapsedTime.TotalMilliseconds;

            //Edit mode...
            if (GameManager.DevelopmentMode)
                _editModeService = new EditModeService(this);

        }


        /// <summary>
        /// Add a camera
        /// </summary>
        public void AddCamera(Camera camera)
        {
            camera.Game = this;
            _extraCameras.Add(camera);
        }

        /// <summary>
        /// Remove a camera
        /// </summary>
        public void RemoveCamera(Camera camera)
        {
            _extraCameras.Remove(camera);
        }

        /// <summary>
        /// Reset all cameras
        /// </summary>
        public void ResetCameras()
        {
            _mainCamera = new Camera(_gameSize.X, _gameSize.Y);
            _mainCamera.Game = this;
            _extraCameras.Clear();
        }

        /// <summary>
        /// Update Graphic Settings
        /// </summary>
        public void UpdateGraphicSettings(GraphicSettings graphicSettings)
        {
            //Update graphics...
            if (_graphics.PreferredBackBufferWidth != graphicSettings.ScreenSize.X
                || _graphics.PreferredBackBufferHeight != graphicSettings.ScreenSize.Y
                || _graphics.IsFullScreen != graphicSettings.IsFullscreen)
            {
                _graphics.PreferredBackBufferWidth = graphicSettings.ScreenSize.X;
                _graphics.PreferredBackBufferHeight = graphicSettings.ScreenSize.Y;
                _graphics.IsFullScreen = graphicSettings.IsFullscreen;
                _graphics.ApplyChanges();
            }

            //Basic settings...
            this.NbPixelPerMeter = graphicSettings.NbPixelPerMeter;
            this.BackgroundColor = graphicSettings.BackgroundColor;



            //Recalculate internal things...
            _screenSize = graphicSettings.ScreenSize;
            _gameSize = graphicSettings.GameSize;
            _gameRectangle = new Rectangle(0, 0, graphicSettings.GameSize.X, graphicSettings.GameSize.Y);

            if (graphicSettings.GameSize == graphicSettings.ScreenSize)
            {
                //No scaling...
                ScreenScale = Vector2.One;
                ScaleMatrix = Matrix.Identity;

            }
            else
            {

                float widthScale = (float)graphicSettings.ScreenSize.X / graphicSettings.GameSize.X;
                float heightScale = (float)graphicSettings.ScreenSize.Y / graphicSettings.GameSize.Y;

                ScreenScale = new Vector2(widthScale, heightScale);

                //ScreenAspectRatio = new Vector2(widthScale / heightScale);
                Vector3 scalingFactor = new Vector3(widthScale, heightScale, 1);
                ScaleMatrix = Matrix.CreateScale(scalingFactor);

            }


            //Update cameras...
            Point diff = new Point(graphicSettings.GameSize.X - _gameSize.X, graphicSettings.GameSize.Y - _gameSize.Y);
            _mainCamera.Size += diff;
            _mainCamera.RecalculteViewPort();

            foreach (Camera extraCamera in _extraCameras)
            {
                extraCamera.Size += diff;
                extraCamera.RecalculteViewPort();
            }


        }


        /// <summary>
        /// Get the camera to use for a layermask
        /// </summary>
        public Camera GetCameraForObject(GameObject gameObject)
        {
            if (_extraCameras.Count == 0)
                return _mainCamera;

            if ((_mainCamera.LayerMask & gameObject.LayerMask) != 0)
                return _mainCamera;

            if (_extraCameras.Count > 0)
            {
                foreach (Camera camera in _extraCameras)
                {
                    if ((camera.LayerMask & gameObject.LayerMask) != 0)
                        return camera;
                }
            }

            return _mainCamera;

        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public void Quit()
        {
            //Check if modification in the designer before closing
            if (_editModeService != null)
            {
                if (!_editModeService.Quit())
                    return;
            }

            this.Exit();
        }

        /// <summary>
        /// Run only the server
        /// </summary>
        public void RunServer()
        {
            this.IsDedicatedServer = true;

            this.LoadContent();

            GameLoop gameLoop = new GameLoop(TickServer);
            gameLoop.RunLoop();
        }

        /// <summary>
        /// Run for unit test only 
        /// </summary>
        public void RunUnitTestOneFrame()
        {  
            do
            {
                RunOneFrame();

                //We will wait if the loading has not been done, we needs to wait for the graphicdevice to be created.
                if (_loadContentDone)
                    return;

                System.Threading.Thread.Sleep(10);
            }
            while (true);
        }


        /// <summary>
        /// Get the current updates
        /// </summary>
        public IUpdate[] GetUpdateables()
        {
            return _updateables.ToArray();
        }

        /// <summary>
        /// Get the current drawables
        /// </summary>
        public IDraw[] GetDrawables()
        {
            return _drawables.ToArray();
        }

        /// <summary>
        /// Tick the server
        /// </summary>
        private void TickServer()
        {
            _internalTimer.Restart();

            if (_rootGameObject != null)
            {
                //Call du update du current game...
                for (int index = 0; index < _updateables.Count; index++)
                    _updateables[index].Update();
            }

            DoAfterUpdate();
        }

        /// <summary>
        /// Trap the Exit
        /// </summary>
        protected override void OnExiting(object sender, EventArgs args)
        {
            try
            {
                //Check if modification in the designer before closing
                if (_editModeService != null)
                    _editModeService.Quit(false);
            }
            finally
            {
                //Current game...
                Current = null;
                this.Exited = true;
            }
        }


        /// <summary>
        /// This function is automatically called when the game launches to initialize any non-graphic variables.
        /// </summary>
        protected override void Initialize()
        {
            //Rendu ici, on peut accéder au GrraphicsDevice
            _graphicsDevice = this.GraphicsDevice;

            //Le FontManager a aussi besoin du graphicsDevice...
            FontManager.SetGraphicsDevice(_graphicsDevice);


            //Base initialize...
            base.Initialize();

            //On indique si c'est initialisé
            this.IsInitialized = true;


            //Setup the mouse...
            this.IsMouseVisible = Mouse.IsMouseVisible;

        }

        /// <summary>
        /// Automatically called when your game launches to load any game assets (_graphics, audio etc.)
        /// </summary>
        protected override void LoadContent()
        {
            //Chargement du contenu...
            if (_rootGameObject != null)
            {
                //Be sure to have a game object in here...
                _rootGameObject.Game = this;
                _rootGameObject.DoLoad();
                _rootGameObject.DoOnAdded(false);
            }


            //Base LoadContent (i guess there's nothing there)
            base.LoadContent();

            //Maintenant, on peut commencer à regarder les modifications...
            if (_editModeService != null)
                _editModeService.ContentWatcher.StartWatchUpdateContent();

            //Job done!
            _loadContentDone = true;

        }


        /// <summary>
        /// On activation for the form...
        /// </summary>
        protected override void UnloadContent()
        {
            if (_rootGameObject != null)
            {
                _rootGameObject.ForEachChild(o => o.DoOnRemoved(), true);
                _rootGameObject.DoOnRemoved();
            }

            base.UnloadContent();
        }


        /// <summary>
        /// Called each frame to update the game. Games usually runs 60 frames per second.
        /// Each frame the Update function will run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {

            DoUpdate();

            //Update the things FNA handles for us underneath the hood:
            base.Update(gameTime);

            DoAfterUpdate();
        }

        /// <summary>
        /// Do update
        /// </summary>
        private void DoUpdate()
        {
            _internalTimer.Restart();

            //Update des inputs...
            _input.Update();

            //We will need the current main windows Win32
            if (this.GameWindowHandle == IntPtr.Zero)
            {
                this.GameWindowHandle = WindowHelper.GetMainWindowHandle();

                //if (_rootGameObject != null && _rootGameObject is NetworkGameObject && !((NetworkGameObject)_rootGameObject).IsClient)
                //    WindowHelper.HideWindow(this.GameWindowHandle);
            }


            //---------------
            // DEV MODE....
            if (_editModeService != null)
            {
                _editModeService.ProcessUpdateDevMode();
            }

            //Processing mouse events...
            Mouse.ProcessMouseEvents();


            //In edit mode, we dont update the objects normally...
            if (_editModeService != null && _editModeService.EditMode && !_editModeService.IsGameRunning)
            {
                _editModeService.ProcessUpdateEditMode();
            }
            else
            {
                //Call du update du current game...

                //--------
                //Removing.....
                //We remove updatables beforce the loop and not when in the loop to be sure that the _updateables list
                //will not shrink on object destruction in the .Update()
                if (_updateablesDestroyed.Count > 0)
                {
                    for (int index = 0; index < _updateablesDestroyed.Count; index++)
                        _updateables.Remove(_updateablesDestroyed[index]);
                    _updateablesDestroyed.Clear();
                }

                //--------
                //Normal updates...
                for (int index = 0; index < _updateables.Count; index++)
                    _updateables[index].Update();

                //--------
                //Sometimes when updating, some objects are added, we will Update them, otherwise for one frame they will
                //never been updated before drawing which can be bad.
                if (_updateablesAdded.Count > 0)
                {
                    int nextIndex = 0;
                    while (nextIndex < _updateablesAdded.Count)
                    {
                        
                        for (int index = nextIndex; index < _updateablesAdded.Count; index++)
                        {
                            InsertUpdateable(_updateablesAdded[index]);
                            _updateablesAdded[index].Update();
                        }

                        nextIndex = _updateablesAdded.Count;

                    }
                    _updateablesAdded.Clear();
                }
            }
        }

        /// <summary>
        /// Do after the update
        /// </summary>
        private void DoAfterUpdate()
        {
            //We keep the last Update time
            //Each tick in the DateTime.Ticks value represents one 100-nanosecond interval. Each tick in the ElapsedTicks value represents the time interval equal to 1 second divided by the Frequency.
            _gameTimeService.LastFrameUpdateTimeMilliseconds = ((decimal)_internalTimer.ElapsedTicks / Stopwatch.Frequency) * 1000;
        }


        /// <summary>
        /// This is called when the game is ready to draw to the screen, it's also called each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            _inDrawing = true;

            //This will clear what's on the screen each frame, if we don't clear the screen will look like a mess:
            _graphicsDevice.Clear(this.BackgroundColor);

            ////Removing drawables...
            //if (_drawablesDestroyed.Count > 0)
            //{
            //    for (int index = 0; index < _drawablesDestroyed.Count; index++)
            //        _drawables.Remove(_drawablesDestroyed[index]);
            //    _drawablesDestroyed.Clear();
            //}

            //Render...
            if (_rootGameObject != null && _rootGameObject.Visible)
            {

                //Starting up sprite batches...
                //Console.WriteLine("camera: " + _mainCamera.Location + " " + _mainCamera.ViewLocation);
                RenderCamera(_mainCamera);


                if (_extraCameras.Count > 0)
                {
                    for (int index = 0; index < _extraCameras.Count; index++)
                        RenderCamera(_extraCameras[index]);
                }
            }

            //Draw in edit mode...
            if (_editModeService != null && _editModeService.EditMode)
            {
                _editModeService.ProcessDrawEditMode();
            }

            //Draw the things FNA handles for us underneath the hood:
            base.Draw(gameTime);

            //We keep the last Draw time
            _internalTimer.Stop();

            //Each tick in the DateTime.Ticks value represents one 100-nanosecond interval. Each tick in the ElapsedTicks value represents the time interval equal to 1 second divided by the Frequency.
            _gameTimeService.LastFrameTimeMilliseconds = ((decimal)_internalTimer.ElapsedTicks / Stopwatch.Frequency) * 1000;
            _gameTimeService.LastFrameDrawTimeMilliseconds = _gameTimeService.LastFrameTimeMilliseconds - _gameTimeService.LastFrameUpdateTimeMilliseconds;

            _inDrawing = false;

        }

        /// <summary>
        /// On activation for the form...
        /// </summary>
        protected override void OnActivated(object sender, EventArgs args)
        {
            if (_editModeService != null && _editModeService.EditMode)
            {
                //Reopening the designer forms and refocusing on ourself...
                if (_editModeService.IsReshowWindowNeeded())
                    _editModeService.ShowDesigner(this.GameWindowHandle);
            }

            base.OnActivated(sender, args);
        }

        /// <summary>
        /// On desactivation of the form
        /// </summary>
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            //if (GameHost.EditMode)
            //    EditModeHelper.HideDesigner();

            base.OnDeactivated(sender, args);
        }

        /// <summary>
        /// Render a camera
        /// </summary>
        private void RenderCamera(Camera camera)
        {
            _drawingContext.Begin(camera);

            //Drawing children...
            //_rootGameObject.DoDraw();
            for (int index = 0; index < _drawables.Count; index++)
            {
                //Check if object must be renderer by the camera...
                if ((camera.LayerMask & _drawables[index].LayerMask) != 0)
                    _drawables[index].Draw();
            }

            _drawingContext.End();
        }

        /// <summary>
        /// Add the current component to the active list
        /// </summary>
        internal void AddUpdateable(IUpdate updateable)
        {

            if (_updateablesDestroyed.Contains(updateable))
            {
                _updateablesDestroyed.Remove(updateable);
                return;
            }

            _updateablesAdded.Add(updateable);

            ////We will insert it at the right place based on UpdateOrder...
            //for (int i = 0; i < _updateables.Count; i += 1)
            //{
            //    if (updateable.UpdateOrder < _updateables[i].UpdateOrder)
            //    {
            //        _updateables.Insert(i, updateable);
            //        return;
            //    }
            //}

            //_updateables.Add(updateable);
        }

        /// <summary>
        /// Add the current component to the active list
        /// </summary>
        internal void InsertUpdateable(IUpdate updateable)
        {
            //We will insert it at the right place based on UpdateOrder...
            for (int i = 0; i < _updateables.Count; i += 1)
            {
                if (updateable.UpdateOrder < _updateables[i].UpdateOrder)
                {
                    _updateables.Insert(i, updateable);
                    return;
                }
            }

            _updateables.Add(updateable);
        }

        /// <summary>
        /// Add the current component to the active list
        /// </summary>
        internal void AddDrawable(IDraw drawable)
        {
            if (_inDrawing)
                throw new InvalidOperationException("Cannot modify drawables during drawing.");

            //if (_drawablesDestroyed.Contains(drawable))
            //    return;

            _drawables.Add(drawable);
        }

        /// <summary>
        /// Remove the current component from the active list
        /// </summary>
        internal void RemoveUpdateable(IUpdate updateable)
        {
            //_updateables.Remove(updateable);
            _updateablesDestroyed.Add(updateable);
        }

        /// <summary>
        /// Remove the current component from the active list
        /// </summary>
        internal void RemoveDrawable(IDraw drawable)
        {
            if (_inDrawing)
                throw new InvalidOperationException("Cannot modify drawables during drawing.");

            _drawables.Remove(drawable);
        }


    }
}
