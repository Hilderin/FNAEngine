using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// GameManager
    /// </summary>
    public static class GameManager
    {
        /// <summary>
        /// Internal GameHost
        /// </summary>
        private static Game _game;

        /// <summary>
        /// Is running?
        /// </summary>
        private static bool _isRunning = false;

        /// <summary>
        /// Indicate if in dev mode
        /// </summary>
        public static bool DevelopmentMode { get; set; } = true;

        /// <summary>
        /// Indicate if we are the client
        /// </summary>
        public static bool IsClient { get; set; } = true;

        /// <summary>
        /// Indicate if we are the server
        /// </summary>
        public static bool IsServer { get; set; } = true;

        /// <summary>
        /// Exécution du GameHost
        /// </summary>
        public static void Run(GameObject rootGameObject, GraphicSettings graphicSettings)
        {
            try
            {
                //We dispose of the _game at the end, Run cannot be called multiple times
                if (_isRunning)
                    throw new InvalidOperationException("Game already running.");

                _isRunning = true;

                _game = new Game(graphicSettings);

                RunInternal(rootGameObject);
            }
            catch (Exception ex)
            {
                ErrorHelper.Show("Error: " + ex.ToString());
            }
        }


        /// <summary>
        /// Quit the game
        /// </summary>
        public static void Quit()
        {
            if(_isRunning)
                _game.Quit();
        }



        /// <summary>
        /// Exécution du GameHost
        /// </summary>
        private static void RunInternal(GameObject rootGameObject)
        {
            try
            {
                _game.RootGameObject = rootGameObject;

                _game.Run();
            }
            finally
            {
                _isRunning = false;
                _game.Dispose();
                
            }
        }


    }
}
