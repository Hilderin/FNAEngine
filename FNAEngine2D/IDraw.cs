using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public interface IDraw
    {
        /// <summary>
        /// Layermask to determine the layers on which the IDraw should be drawn
        /// </summary>
        Layers LayerMask { get; }

        /// <summary>
        /// Do the drawing
        /// </summary>
        void Draw();
    }
}
