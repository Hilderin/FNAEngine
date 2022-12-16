using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAEngine2D
{
    /// <summary>
    /// Game interne XNA avec le Initialize, LoadContent, Update et Draw
    /// </summary>
    internal class InternalGameHost : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Graphic Manager
        /// </summary>
        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// Graphic device
        /// </summary>
        private GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Sprite batch pour le renderer
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Game actuellement en train de rouler
        /// </summary>
        private GameObject _rootGameObject;

        /// <summary>
        /// Taille de l'écran
        /// </summary>
        private Vector2Int _screenSize;


        /// <summary>
        /// Sprite batch pour le renderer
        /// </summary>
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

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
        /// Width
        /// </summary>
        public int Width { get { return _screenSize.X; } }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get { return _screenSize.Y; } }

        /// <summary>
        /// Size
        /// </summary>
        public Vector2Int Size { get { return _screenSize; } }



        /// <summary>
        /// Constructeur
        /// </summary>
        public InternalGameHost() //This is the constructor, this function is called whenever the game class is created.
        {
            _graphics = new GraphicsDeviceManager(this);

            //Création du Content Manager..
            this.Content = new ContentManager(this.Services, ContentHelper.ContentFolder);


            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            _screenSize = new Vector2Int(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
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

        }

        /// <summary>
        /// Automatically called when your game launches to load any game assets (_graphics, audio etc.)
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Chargement du contenu...
            _rootGameObject.LoadWithChildren();


            //Base LoadContent (i guess there's nothing there)
            base.LoadContent();

            //Maintenant, on peut commencer à regarder les modifications...
#if DEBUG
            ContentHelper.StartWatchUpdateContent();
#endif

        }

        /// <summary>
        /// Called each frame to update the game. Games usually runs 60 frames per second.
        /// Each frame the Update function will run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            //On update le gametime pour l'avoir partout...
            GameHost.SetGameTime(gameTime);


            //On regarde si on a du content à reloader...
            ContentHelper.ReloadModifiedContent();


            //Update des inputs...
            Input.Update();

            //Call du update du current game...
            _rootGameObject.UpdateWithChildren();

            //Update the things FNA handles for us underneath the hood:
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game is ready to draw to the screen, it's also called each frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            //On update le gametime pour l'avoir partout...
            GameHost.SetGameTime(gameTime);

            //This will clear what's on the screen each frame, if we don't clear the screen will look like a mess:
            _graphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin();

            //Render des enfants...
            _rootGameObject.DrawWithChildren();

            _spriteBatch.End();

            //Draw the things FNA handles for us underneath the hood:
            base.Draw(gameTime);
        }


    }
}
