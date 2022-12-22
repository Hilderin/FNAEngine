using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Information about a frame in the sprite animation
    /// </summary>
    public class SpriteAnimationFrame
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public int Duration { get ;set; }
    }
}
