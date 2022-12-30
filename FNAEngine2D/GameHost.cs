using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Velentr.Font;
using System.Windows.Forms;
using System.IO;
using System.Runtime.CompilerServices;

namespace FNAEngine2D
{
    /// <summary>
    /// Game host
    /// </summary>
    public static class GameHost
    {
       
        /// <summary>
        /// Internal GameHost
        /// </summary>
        private static InternalGame _internalGame;

        /// <summary>
        /// Run done?
        /// </summary>
        private static bool _runDone = false;


        ///// <summary>
        ///// Sprite batch pour le renderer
        ///// </summary>
        //public static SpriteBatch SpriteBatch { get { return _internalGame.CurrentCamera.SpriteBatch; } }

        /// <summary>
        /// Current Game
        /// </summary>
        public static GameObject RootGameObject
        {
            get { return _internalGame.RootGameObject; }
            set { _internalGame.RootGameObject = value; }
        }

        /// <summary>
        /// Number of pixels = 1 meter
        /// </summary>
        public static float NbPixelPerMeter { get; set; } = 100;

        ///// <summary>
        ///// Current GameTime
        ///// </summary>
        //public static GameTime GameTime { get; private set; }

        ///// <summary>
        ///// Indique si le contenu est en train d'être loadé
        ///// </summary>
        //public static bool ContentLoading { get; internal set; }

        ///// <summary>
        ///// Indique si le contenu a été loadé
        ///// </summary>
        //public static bool ContentLoaded { get; internal set; }

        /// <summary>
        /// ElapsedGameTimeSeconds
        /// </summary>
        public static float ElapsedGameTimeSeconds { get; private set; }

        /// <summary>
        /// ElapsedGameTimeMilliseconds
        /// </summary>
        public static float ElapsedGameTimeMilliseconds { get; private set; }

        /// <summary>
        /// Time for the last Update
        /// </summary>
        public static decimal LastFrameUpdateTimeMilliseconds { get; set; }

        /// <summary>
        /// Time for the last Draw
        /// </summary>
        public static decimal LastFrameDrawTimeMilliseconds { get; set; }

        /// <summary>
        /// Time for the last Update+Draw
        /// </summary>
        public static decimal LastFrameTimeMilliseconds { get; set; }

        /// <summary>
        /// Width du jeu
        /// </summary>
        public static int Width { get { return _internalGame.Width; } }

        /// <summary>
        /// Center X
        /// </summary>
        public static int CenterX { get { return _internalGame.Width / 2; } }

        /// <summary>
        /// Height du jeu
        /// </summary>
        public static int Height { get { return _internalGame.Height; } }

        /// <summary>
        /// Center Y
        /// </summary>
        public static int CenterY { get { return _internalGame.Height / 2; } }

        /// <summary>
        /// Size of the Game (Internal size)
        /// </summary>
        public static Point Size { get { return _internalGame.Size; } }

        /// <summary>
        /// Real size of the screen
        /// </summary>
        public static Point ScreenSize { get { return _internalGame.ScreenSize; } }

        /// <summary>
        /// Rectangle of the game
        /// </summary>
        public static Rectangle Rectangle { get { return _internalGame.Rectangle; } }

        /// <summary>
        /// Bounds of the game
        /// </summary>
        public static Rectangle Bounds { get { return _internalGame.Bounds; } }

        /// <summary>
        /// Background color
        /// </summary>
        public static Color BackgroundColor { get; set; } = Color.Gray;

        /// <summary>
        /// The scale result of merging Internal size with Screen size.
        /// </summary>
        public static Vector2 ScreenScale { get { return _internalGame.ScreenScale; } }

        /// <summary>
        /// Indicate if we are the server process
        /// </summary>
        public static bool IsServer { get; set; } = false;

        /// <summary>
        /// Indicate if we are the client process
        /// </summary>
        public static bool IsClient { get; set; } = false;

        /// <summary>
        /// Default Camera
        /// </summary>
        public static Camera MainCamera
        {
            get { return _internalGame.MainCamera; }
            set { _internalGame.MainCamera = value; }
        }

        /// <summary>
        /// List of the other cameras on the scene
        /// </summary>
        public static List<Camera> ExtraCameras { get { return _internalGame.ExtraCameras; } }

        /// <summary>
        /// Indicate if in dev mode
        /// </summary>
        public static bool DevelopmentMode { get; set; } = true;

        /// <summary>
        /// Indicate if in edit mode
        /// </summary>
        public static bool EditMode
        {
            get { return EditModeHelper.EditMode; }
            set { EditModeHelper.EditMode = value; }
        }

        /// <summary>
        /// InternalGame
        /// </summary>
        internal static InternalGame InternalGame { get { return _internalGame; } }



        /// <summary>
        /// Constructeur
        /// </summary>
        static GameHost()
        {
            //_internalGame = new InternalGame();
        }


        /// <summary>
        /// Set the resolution
        /// </summary>
        /// <param name="screenWidth">Width on screen (real window width)</param>
        /// <param name="screenHeight">Height on screen (real window height)</param>
        /// <param name="internalWidth">Internal width, number of pixels everything use internally</param>
        /// <param name="internalHeight">Internal height, number of pixels everything use internally</param>
        public static void SetResolution(int screenWidth, int screenHeight, int internalWidth, int internalHeight, bool isFullScreen)
        {
            EnsureInternalGame();
            _internalGame.SetResolution(screenWidth, screenHeight, internalWidth, internalHeight, isFullScreen);
        }

        /// <summary>
        /// Permet d'obtenir un élément du Content
        /// </summary>
        public static Content<T> GetContent<T>(string assetName)
        {
            return _internalGame.ContentManager.GetContent<T>(assetName);
        }


        /// <summary>
        /// Exécution du GameHost
        /// </summary>
        public static void Run(GameObject rootGameObject)
        {
            try
            {
                //We dispose of the _internalGame at the end, Run cannot be called multiple times
                if (_runDone)
                    throw new InvalidOperationException("Run can be executed only once.");

                _runDone = true;

                EnsureInternalGame();

                RunInternal(rootGameObject);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        /// <summary>
        /// Get the camera to use for a layermask
        /// </summary>
        public static Camera GetCameraForObject(GameObject gameObject)
        {
            if (GameHost.ExtraCameras.Count == 0)
                return GameHost.MainCamera;

            if ((GameHost.MainCamera.LayerMask & gameObject.LayerMask) != 0)
                return GameHost.MainCamera;

            if (GameHost.ExtraCameras.Count > 0)
            {
                foreach (Camera camera in GameHost.ExtraCameras)
                {
                    if ((camera.LayerMask & gameObject.LayerMask) != 0)
                        return camera;
                }
            }

            return GameHost.MainCamera;

        }

        /// <summary>
        /// Exécution du GameHost
        /// </summary>
        private static void RunInternal(GameObject rootGameObject)
        {
            try
            {
                //We are a client...
                GameHost.IsClient = true;

                _internalGame.RootGameObject = rootGameObject;

                _internalGame.Run();
            }
            finally
            {
                _internalGame.Dispose();
            }
        }


        /// <summary>
        /// Ensure that the InternalGame is created
        /// </summary>
        private static void EnsureInternalGame()
        {
            if(_internalGame == null)
                _internalGame = new InternalGame();
        }

        ///// <summary>
        ///// CurrentGameTime
        ///// </summary>
        //internal static void SetGameTime(GameTime gameTime)
        //{
        //    //GameTime = gameTime;
        //    ElapsedGameTimeSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    ElapsedGameTimeMilliseconds = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        //}

        /// <summary>
        /// CurrentGameTime
        /// </summary>
        public static void SetGameTime(TimeSpan elapsedGameTime)
        {
            ElapsedGameTimeSeconds = (float)elapsedGameTime.TotalSeconds;
            ElapsedGameTimeMilliseconds = (float)elapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public static void Quit()
        {
            EnsureInternalGame();

            _internalGame.Quit();
        }

    }
}
