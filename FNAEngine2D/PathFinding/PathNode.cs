using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.PathFinding
{
    /// <summary>
    /// Path node
    /// </summary>
    public class PathNode
    {

        /// <summary>
        /// Location to walk to
        /// </summary>
        public Vector2 Location { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PathNode(Vector2 location)
        {
            this.Location = location;
        }

        /// <summary>
        /// ToString override to debug
        /// </summary>
        public override string ToString()
        {
            return this.Location.ToString();
        }

    }
}
