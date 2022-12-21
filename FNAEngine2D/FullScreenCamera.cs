using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Default Camera static to see all the screen
    /// </summary>
    public class FullScreenCamera: Camera
    {

        /// <summary>
        /// Get the camera matrx
        /// </summary>
        public override Matrix GetMatrix()
        {
            //Only the scale matrix...
            return GameHost.InternalGameHost.ScaleMatrix;

        }

    }
}
