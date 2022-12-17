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
        private static InternalGameHost _internalGameHost;


        /// <summary>
        /// Sprite batch pour le renderer
        /// </summary>
        public static SpriteBatch SpriteBatch { get { return _internalGameHost.SpriteBatch; } }

        /// <summary>
        /// Current Game
        /// </summary>
        public static GameObject RootGameObject
        {
            get { return _internalGameHost.RootGameObject; }
            set { _internalGameHost.RootGameObject = value; }
        }

        /// <summary>
        /// Current GameTime
        /// </summary>
        public static GameTime GameTime { get; private set; }

        /// <summary>
        /// Indique si le contenu est en train d'être loadé
        /// </summary>
        public static bool ContentLoading { get; internal set; }

        /// <summary>
        /// Indique si le contenu a été loadé
        /// </summary>
        public static bool ContentLoaded { get; internal set; }

        /// <summary>
        /// ElapsedGameTimeSeconds
        /// </summary>
        public static float ElapsedGameTimeSeconds { get; private set; }

        /// <summary>
        /// Width du jeu
        /// </summary>
        public static int Width { get { return _internalGameHost.Width; } }

        /// <summary>
        /// Height du jeu
        /// </summary>
        public static int Height { get { return _internalGameHost.Height; } }

        /// <summary>
        /// Size du jeu
        /// </summary>
        public static Point Size { get { return _internalGameHost.Size; } }

        /// <summary>
        /// Rectangle of the game
        /// </summary>
        public static Rectangle Rectangle { get { return _internalGameHost.Rectangle; } }

        /// <summary>
        /// Bounds of the game
        /// </summary>
        public static Rectangle Bounds { get { return _internalGameHost.Bounds; } }

        /// <summary>
        /// InternalGameHost
        /// </summary>
        internal static InternalGameHost InternalGameHost { get { return _internalGameHost; } }



        /// <summary>
        /// Constructeur
        /// </summary>
        static GameHost()
        {
            _internalGameHost = new InternalGameHost();
        }

        /// <summary>
        /// Permet d'obtenir un élément du Content
        /// </summary>
        public static T GetContent<T>(string assetName)
        {
            if (assetName == "pixel" && typeof(T) == typeof(Texture2D))
            {
                object ret = Texture2D.FromStream(_internalGameHost.GraphicsDevice, new MemoryStream(Resource.pixelBin));
                return (T)ret;
            }

            return _internalGameHost.Content.Load<T>(assetName);
        }


        /// <summary>
        /// Exécution du GameHost
        /// </summary>
        public static void Run(GameObject rootGameObject)
        {
            try
            {
                RunInternal(rootGameObject);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Exécution du GameHost
        /// </summary>
        private static void RunInternal(GameObject rootGameObject)
        {
            try
            {
                _internalGameHost.RootGameObject = rootGameObject;

                _internalGameHost.Run();
            }
            finally
            {
                _internalGameHost.Dispose();
            }
        }


        /// <summary>
        /// CurrentGameTime
        /// </summary>
        internal static void SetGameTime(GameTime gameTime)
        {
            GameTime = gameTime;
            ElapsedGameTimeSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
