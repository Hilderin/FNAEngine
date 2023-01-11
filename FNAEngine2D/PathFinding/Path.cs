using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.PathFinding
{
    /// <summary>
    /// A Path to walk through
    /// </summary>
    public class Path
    {
        /// <summary>
        /// Path nodes
        /// </summary>
        public List<PathNode> Nodes { get; } = new List<PathNode>();
    }
}
