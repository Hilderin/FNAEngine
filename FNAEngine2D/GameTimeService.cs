using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Service to help with the game time
    /// </summary>
    public class GameTimeService
    {
        /// <summary>
        /// Time for the last Update
        /// </summary>
        public decimal LastFrameUpdateTimeMilliseconds { get; set; }

        /// <summary>
        /// Time for the last Draw
        /// </summary>
        public decimal LastFrameDrawTimeMilliseconds { get; set; }

        /// <summary>
        /// Time for the last Update+Draw
        /// </summary>
        public decimal LastFrameTimeMilliseconds { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameTimeService(GameServiceContainer gameServiceContainer)
        {
            if (gameServiceContainer == null)
            {
                throw new ArgumentNullException("gameServiceContainer");
            }

            gameServiceContainer.AddService(typeof(GameTimeService), this);

        }
    }
}
