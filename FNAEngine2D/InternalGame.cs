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

namespace FNAEngine2D
{
    /// <summary>
    /// Game interne XNA avec le Initialize, LoadContent, Update et Draw
    /// </summary>
    internal class InternalGame : Microsoft.Xna.Framework.Game
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

        ///// <summary>
        ///// Current rendering camera
        ///// </summary>
        //private Camera _currentCamera;

        /// <summary>
        /// Game actuellement en train de rouler
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
        /// Game actuellement en train de rouler
        /// </summary>
        public GameObject RootGameObject
        {
            get { return _rootGameObject; }
            set
            { 
                _rootGameObject = value;

                //Il sera son propre root object!
                value.RootGameObject = value;
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

        /// <summary>
        /// List of the other cameras on the scene
        /// </summary>
        public List<Camera> ExtraCameras { get { return _extraCameras; } }

        /// <summary>
        /// Handle to the Window (the Win32 handle)
        /// </summary>
        public IntPtr GameWindowHandle { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public InternalGame() //This is the constructor, this function is called whenever the game class is created.
        {
            _graphics = new GraphicsDeviceManager(this);

            //Default Camera = a static camera 
            _mainCamera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            //Init gameSize so the SetResolution cam calculate the diff for the resize of the camera...
            _screenSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _gameSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            //Création du Content Manager..
            _contentManager = new ContentManager(this.Services, ContentWatcher.ContentFolder);
            this.Content = _contentManager;

            //Setting de default resolution...
            SetResolution(1200, 720, 1200, 720, false);
            
        }


        /// <summary>
        /// Set the resolution
        /// </summary>
        /// <param name="screenWidth">Width on screen (real window width)</param>
        /// <param name="screenHeight">Height on screen (real window height)</param>
        /// <param name="internalWidth">Internal width, number of pixels everything use internally</param>
        /// <param name="internalHeight">Internal height, number of pixels everything use internally</param>
        public void SetResolution(int screenWidth, int screenHeight, int internalWidth, int internalHeight, bool isFullScreen)
        {
            //Update graphics...
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.IsFullScreen = isFullScreen;
            _graphics.ApplyChanges();


            //Update cameras...
            Point diff = new Point(internalWidth - _gameSize.X, internalHeight - _gameSize.Y);
            _mainCamera.Size += diff;
            foreach (Camera extraCamera in _extraCameras)
                extraCamera.Size += diff;


            //Recalculate internal things...
            _screenSize = new Point(screenWidth, screenHeight);
            _gameSize = new Point(internalWidth, internalHeight);
            _gameRectangle = new Rectangle(0, 0, internalWidth, internalHeight);

            if (screenWidth == internalWidth && screenHeight == internalHeight)
            {
                //No scaling...
                ScreenScale = Vector2.One;
                ScaleMatrix = Matrix.Identity;

            }
            else
            {

                float widthScale = (float)screenWidth / internalWidth;
                float heightScale = (float)screenHeight / internalHeight;

                ScreenScale = new Vector2(widthScale, heightScale);

                //ScreenAspectRatio = new Vector2(widthScale / heightScale);
                Vector3 scalingFactor = new Vector3(widthScale, heightScale, 1);
                ScaleMatrix = Matrix.CreateScale(scalingFactor);

            }
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public void Quit()
        {
            //Check if modification in the designer before closing
            if (!EditModeHelper.Quit())
                return;

            this.Exit();
        }

        /// <summary>
        /// Trap the Exit
        /// </summary>
        protected override void OnExiting(object sender, EventArgs args)
        {
            //Check if modification in the designer before closing
            EditModeHelper.Quit(false);

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
            this.IsMouseVisible = MouseManager.IsMouseVisible;

        }

        /// <summary>
        /// Automatically called when your game launches to load any game assets (_graphics, audio etc.)
        /// </summary>
        protected override void LoadContent()
        {
            //Chargement du contenu...
            _rootGameObject._addAuthorized = true;
            _rootGameObject.Load();


            //Base LoadContent (i guess there's nothing there)
            base.LoadContent();

            ////On indique que le contenu est loadé!
            //GameHost.ContentLoaded = true;
            //GameHost.ContentLoading = false;

            //Maintenant, on peut commencer à regarder les modifications...
            if (GameHost.DevelopmentMode)
                ContentWatcher.StartWatchUpdateContent();



        }

        /// <summary>
        /// Called each frame to update the game. Games usually runs 60 frames per second.
        /// Each frame the Update function will run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {

            _internalTimer.Restart();


            //On update le gametime pour l'avoir partout...
            GameHost.SetGameTime(gameTime);


            //Update des inputs...
            Input.Update();

            //---------------
            // DEV MODE....
            if (GameHost.DevelopmentMode)
            {
                EditModeHelper.ProcessUpdateDevMode();
            }


            //On regarde si on a du content à reloader...
            ContentWatcher.ReloadModifiedContent();


            //Processing mouse events...
            MouseManager.ProcessMouseEvents();


            //In edit mode, we dont update the objects normally...
            if (EditModeHelper.EditMode && !EditModeHelper.IsGameRunning)
            {
                EditModeHelper.ProcessUpdateEditMode();
            }
            else
            {
                //Call du update du current game...
                _rootGameObject.UpdateWithChildren();
            }

            //Update the things FNA handles for us underneath the hood:
            base.Update(gameTime);

            //We keep the last Update time
            //Each tick in the DateTime.Ticks value represents one 100-nanosecond interval. Each tick in the ElapsedTicks value represents the time interval equal to 1 second divided by the Frequency.
            GameHost.LastFrameUpdateTimeMilliseconds = ((decimal)_internalTimer.ElapsedTicks / Stopwatch.Frequency) * 1000;
        }



        /// <summary>
        /// This is called when the game is ready to draw to the screen, it's also called each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            //On update le gametime pour l'avoir partout...
            GameHost.SetGameTime(gameTime);

            //This will clear what's on the screen each frame, if we don't clear the screen will look like a mess:
            _graphicsDevice.Clear(GameHost.BackgroundColor);

            //Render...
            if (_rootGameObject.Visible)
            {

                //Starting up sprite batches...
                RenderCamera(_mainCamera);


                if (_extraCameras.Count > 0)
                {
                    for (int index = 0; index < _extraCameras.Count; index++)
                        RenderCamera(_extraCameras[index]);
                }
            }

            //Draw in edit mode...
            if (EditModeHelper.EditMode)
            {
                EditModeHelper.ProcessDrawEditMode();
            }

            //Draw the things FNA handles for us underneath the hood:
            base.Draw(gameTime);

            //We keep the last Draw time
            _internalTimer.Stop();

            //Each tick in the DateTime.Ticks value represents one 100-nanosecond interval. Each tick in the ElapsedTicks value represents the time interval equal to 1 second divided by the Frequency.
            GameHost.LastFrameTimeMilliseconds = ((decimal)_internalTimer.ElapsedTicks / Stopwatch.Frequency) * 1000;
            GameHost.LastFrameDrawTimeMilliseconds = GameHost.LastFrameTimeMilliseconds - GameHost.LastFrameUpdateTimeMilliseconds;
            
        }

        /// <summary>
        /// On activation for the form...
        /// </summary>
        protected override void OnActivated(object sender, EventArgs args)
        {
            if (EditModeHelper.EditMode)
            {
                //Reopening the designer forms and refocusing on ourself...
                if (EditModeHelper.IsReshowWindowNeeded())
                    EditModeHelper.ShowDesigner(this.GameWindowHandle);
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
            DrawingContext.Begin(camera);

            //Drawing children...
            _rootGameObject.DrawWithChildren();

            DrawingContext.End();
        }


    }
}
