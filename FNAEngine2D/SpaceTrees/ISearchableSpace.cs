using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.SpaceTrees
{
    public interface ISearchableSpace
    {
        ///// <summary>
        ///// Search in the space
        ///// </summary>
        //IEnumerable<T> Search(float x, float y, float width, float height);

        /// <summary>
        /// Check if there is something in a rect
        /// </summary>
        bool Any(float x, float y, float width, float height);
    }
}
