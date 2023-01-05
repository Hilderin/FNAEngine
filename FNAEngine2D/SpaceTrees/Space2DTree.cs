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
        /// All data in the tree
        /// </summary>
        private Dictionary<T, Space2DTreeNodeData<T>> _data = new Dictionary<T, Space2DTreeNodeData<T>>();


        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Add a data
        /// </summary>
        public Space2DTreeNodeData<T> Add(float x, float y, float width, float height, T data)
        {
            //Little validation to avoid screwing up the tree...
            if (!IsFloatValid(x))
                throw new InvalidOperationException("x value invalid: " + x);
            if (!IsFloatValid(y))
                throw new InvalidOperationException("y value invalid: " + y);
            if (!IsFloatValid(width))
                throw new InvalidOperationException("width value invalid: " + width);
            if (!IsFloatValid(height))
                throw new InvalidOperationException("height value invalid: " + width);


            //I always when a positif width and height
            if (width < 0)
            {
                x = x + width;
                //width = -width;
            }
            if (height < 0)
            {
                y = y + height;
                //height = -height;
            }

            //Console.WriteLine("Add " + x + " " + y + " " + width + " " + height + " " + data.ToString());


            var dataNode = new Space2DTreeNodeData<T>(x, y, width, height, data);
            _root.Add(dataNode);
            _data.Add(data, dataNode);

            this.Count++;

            return dataNode;
        }


        /// <summary>
        /// Remove a data
        /// </summary>
        public void Remove(T data)
        {
            if (_data.TryGetValue(data, out var dataNode))
            {
                dataNode.ContainerNode._bucket.Remove(dataNode);
                _data.Remove(data);

                this.Count--;
            }
        }

        /// <summary>
        /// Move a data
        /// </summary>
        public void Move(float x, float y, float width, float height, T data)
        {
            if (!_data.TryGetValue(data, out var dataNode))
                throw new InvalidOperationException("Data not found in the tree.");

            Move(x, y, width, height, dataNode);
        }


        /// <summary>
        /// Move a data node
        /// </summary>
        public void Move(float x, float y, float width, float height, Space2DTreeNodeData<T> dataNode)
        {

            //Little validation to avoid screwing up the tree...
            if (!IsFloatValid(x))
                throw new InvalidOperationException("x value invalid: " + x);
            if (!IsFloatValid(y))
                throw new InvalidOperationException("y value invalid: " + y);
            if (!IsFloatValid(width))
                throw new InvalidOperationException("width value invalid: " + width);
            if (!IsFloatValid(height))
                throw new InvalidOperationException("height value invalid: " + width);


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

            dataNode.X = x;
            dataNode.Y = y;
            dataNode.Right = x + width;
            dataNode.Bottom = y + height;

            //dataNode.ContainerNode._bucket.Remove(dataNode);
            //_root.Add(dataNode);

            var destinationNode = _root.GetDestinationNode(dataNode);

            if (destinationNode == dataNode.ContainerNode)
                //Already at the right place...
                return;

            dataNode.ContainerNode._bucket.Remove(dataNode);

            //And we and directly on the right node!
            destinationNode.Add(dataNode);

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


        /// <summary>
        /// Check if a float is valid for the tree
        /// </summary>
        private bool IsFloatValid(float value)
        {
            if (value == float.NaN || value == float.PositiveInfinity || value == float.NegativeInfinity || value == float.MinValue || value == float.MaxValue)
                return false;
            else
                return true;


        }

    }
}
