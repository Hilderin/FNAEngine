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
        public int Col { get; set; }
        public int Row { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Tile(int col, int row)
        {
            Col = col;
            Row = row;
        }

        /// <summary>
        /// ToString
        /// </summary>
        public override string ToString()
        {
            return this.Col.ToString() + ", " + this.Row.ToString();
        }
    }
}
