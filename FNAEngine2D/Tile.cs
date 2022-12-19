using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Tile information
    /// </summary>
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// ToString
        /// </summary>
        public override string ToString()
        {
            return this.X.ToString() + ", " + this.Y.ToString();
        }
    }
}
