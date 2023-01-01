using Microsoft.Xna.Framework;
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
        /// Is running?
        /// </summary>
        private static bool _isRunning = false;

        /// <summary>
        /// Indicate if in dev mode
        /// </summary>
        public static bool DevelopmentMode { get; set; } = true;

        ///// <summary>
        ///// Indicate if we are the client
        ///// </summary>
        //public static bool IsClient { get; set; } = true;

        ///// <summary>
        ///// Indicate if we are the server
        ///// </summary>
        //public static bool IsServer { get; set; } = true;

        /// <summary>
        /// Entrypoint client
        /// </summary>
        public static void Run(GameObject rootGameObject, GraphicSettings graphicSettings)
        {
            try
            {
                //We dispose of the _game at the end, Run cannot be called multiple times
                if (_isRunning)
                    throw new InvalidOperationException("Game already running.");

                _isRunning = true;

                try
                {
                    using (Game game = new Game(graphicSettings))
                    {
                        game.RootGameObject = rootGameObject;

                        game.Run();
                    }
                }
                finally
                {
                    _isRunning = false;
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Show("Error: " + ex.ToString());
            }
        }

        /// <summary>
        /// Entrypoint server
        /// </summary>
        public static void RunServer(GameObject rootGameObject, GraphicSettings graphicSettings)
        {
            try
            {
                //We dispose of the _game at the end, Run cannot be called multiple times
                if (_isRunning)
                    throw new InvalidOperationException("Game already running.");

                _isRunning = true;

                try
                {
                    using (Game game = new Game(graphicSettings))
                    {
                        game.RootGameObject = rootGameObject;

                        game.RunServer();
                    }
                }
                finally
                {
                    _isRunning = false;
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Show("Error: " + ex.ToString());
            }
        }

    }
}
