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


        ///// <summary>
        ///// Liste des objets à renderer
        ///// </summary>
        //private static List<GameObject> _gameObjects = new List<GameObject>();


        ///// <summary>
        ///// Liste des objets à renderer
        ///// </summary>
        //internal static List<GameObject> GetObjectsToRender()
        //{
        //    return _gameObjects;
        //}

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
        /// CurrentGameTime
        /// </summary>
        public static GameTime GameTime;

        /// <summary>
        /// Width du jeu
        /// </summary>
        public static int Width 
        {
            get { return _internalGameHost.Width; }
        }

        /// <summary>
        /// Height du jeu
        /// </summary>
        public static int Height
        {
            get { return _internalGameHost.Height; }
        }


        /// <summary>
        /// Constructeur
        /// </summary>
        static GameHost()
        {
            _internalGameHost = new InternalGameHost();
        }

        ///// <summary>
        ///// Ajout d'un render object
        ///// </summary>
        //public static T Add<T>(T gameObject) where T: GameObject
        //{
        //    _gameObjects.Add(gameObject);

        //    return gameObject;
        //}

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

        ///// <summary>
        ///// Retourne la font demandée
        ///// </summary>
        //public static Font GetFont(string filename, int fontSize)
        //{
        //    return FontManager.GetFont(filename, fontSize);
        //}

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

    }
}
