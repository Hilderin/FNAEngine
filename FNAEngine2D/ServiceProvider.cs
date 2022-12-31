using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Service provider
    /// </summary>
    public static class ServiceProvider
    {
        /// <summary>
        /// Container
        /// </summary>
        public static GameServiceContainer Container { get; set; }

        /// <summary>
        /// Get a service
        /// </summary>
        public static T GetService<T>()
        {
            if (Container == null)
                throw new InvalidOperationException("Container not initialized.");

            return (T)Container.GetService(typeof(T));
        }
    }
}
