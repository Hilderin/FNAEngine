using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.SpaceTrees
{
    /// <summary>
    /// Custom KD tree implementation to store objects with 2D positions
    /// </summary>
    public class Space2DTree<T>
    {
        /// <summary>
        /// Root
        /// </summary>
        internal Space2DTreeNode<T> _root = new SpaceTrees.Space2DTreeNode<T>(null);

        /// <summary>
        /// Add a data
        /// </summary>
        public void Add(float x, float y, float width, float height, T data)
        {
            //Little validation to avoid screwing up the tree...
            if (x == float.NaN || x == float.PositiveInfinity || x == float.NegativeInfinity || x == float.MinValue || x == float.MaxValue)
                throw new InvalidOperationException("X value invalid: " + x);
            if (y == float.NaN || y == float.PositiveInfinity || y == float.NegativeInfinity || y == float.MinValue || y == float.MaxValue)
                throw new InvalidOperationException("Y value invalid: " + y);


            //I always when a positif width and height
            if (width < 0)
            {
                x = x + width;
                width = -width;
            }
            if (height < 0)
            {
                y = y + height;
                height = -height;
            }

            //Console.WriteLine("Add " + x + " " + y + " " + width + " " + height + " " + data.ToString());


            var dataNode = new Space2DTreeNodeData<T>(x, y, width, height, data);
            _root.Add(ref dataNode);
        }


        /// <summary>
        /// Get values in a rectangle
        /// </summary>
        public List<T> GetValues(float x, float y, float width, float height)
        {
            List<T> values = new List<T>();

            float right = x + width;
            float bottom = y + height;

            _root.GetValues(x, y, right, bottom, values);

            return values;
        }

        /// <summary>
        /// Get values in a rectangle
        /// </summary>
        public Space2DTreeResult<T> Search(float x, float y, float width, float height)
        {
            return new Space2DTreeResult<T>(x, y, width, height, this);
        }


    }
}
