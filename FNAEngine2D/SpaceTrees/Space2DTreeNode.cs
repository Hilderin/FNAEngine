using SharpFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.SpaceTrees
{
    /// <summary>
    /// Node in the Space2DTree
    /// </summary>
    internal class Space2DTreeNode<T>
    {

        private bool _splitTestDone;

        internal bool _splitOnX;
        internal float _splitValue;
        

        /// <summary>
        /// Bucket with data in it
        /// </summary>
        internal List<Space2DTreeNodeData<T>> _bucket = new List<Space2DTreeNodeData<T>>(SpaceTreeConstants.BUCKET_CAPACITY);

        /// <summary>
        /// Node with lower value
        /// </summary>
        internal Space2DTreeNode<T> _lowerNode = null;

        /// <summary>
        /// Node with greater value
        /// </summary>
        internal Space2DTreeNode<T> _upperNode = null;

        /// <summary>
        /// Node with intersection with the middle
        /// </summary>
        internal Space2DTreeNode<T> _intersectNode = null;

        /// <summary>
        /// Our parent
        /// </summary>
        internal Space2DTreeNode<T> _parent;

        /// <summary>
        /// Constructor
        /// </summary>
        internal Space2DTreeNode(Space2DTreeNode<T> parent)
        {
            _parent = parent;
        }


        /// <summary>
        /// Add a data
        /// </summary>
        public void Add(ref Space2DTreeNodeData<T> dataNode)
        {
            //Time to split if _lowerNode == null...
            if (_lowerNode == null
                && (_bucket.Count < SpaceTreeConstants.BUCKET_CAPACITY
                   || (_splitTestDone && IsIntersectsSplit(ref dataNode))
                   || !Split()
                   ))
            {
                //Data cannot be splitted..
                _bucket.Add(dataNode);
            }
            else
            {
                //Right, we add in the lower ou upper values node
                AddToChildrenNode(ref dataNode);
            }


        }

        /// <summary>
        /// Get values in a rectangle
        /// </summary>
        public void GetValues(float x, float y, float right, float bottom, List<T> values)
        {
            if (_lowerNode == null)
            {
                //Check all bucket...
                foreach (Space2DTreeNodeData<T> data in _bucket)
                {
                    if (data.Intersects(x, y, right, bottom))
                        values.Add(data.Data);
                }
            }
            else
            {
                if (_splitOnX)
                {
                    //Split on X
                    if (x <= _splitValue)
                    {
                        //There could be some of our data in here
                        _lowerNode.GetValues(x, y, right, bottom, values);
                    }
                    if (right >= _splitValue)
                    {
                        _upperNode.GetValues(x, y, right, bottom, values);
                    }
                    
                    _intersectNode.GetValues(x, y, right, bottom, values);
                }
                else
                {
                    //Split on Y
                    if (y <= _splitValue)
                    {
                        _lowerNode.GetValues(x, y, right, bottom, values);
                    }
                    if (bottom >= _splitValue)
                    {
                        _upperNode.GetValues(x, y, right, bottom, values);
                    }
                   
                    _intersectNode.GetValues(x, y, right, bottom, values);
                    
                }
            }
        }

        /// <summary>
        /// Add the data node in the correct children
        /// </summary>
        private void AddToChildrenNode(ref Space2DTreeNodeData<T> dataNode)
        {
            if (_splitOnX)
            {
                //Split on X
                if (dataNode.Right < _splitValue)
                {
                    _lowerNode.Add(ref dataNode);
                }
                else if (dataNode.X > _splitValue)
                {
                    _upperNode.Add(ref dataNode);
                }
                else
                {
                    _intersectNode.Add(ref dataNode);
                }
            }
            else
            {
                //Split on Y
                if (dataNode.Bottom < _splitValue)
                {
                    _lowerNode.Add(ref dataNode);
                }
                else if (dataNode.Y > _splitValue)
                {
                    _upperNode.Add(ref dataNode);
                }
                else
                {
                    _intersectNode.Add(ref dataNode);
                }
            }
        }

        private bool IsIntersectsSplit(ref Space2DTreeNodeData<T> dataNode)
        {
            if (_splitOnX)
            {
                //Split on X
                if (dataNode.Right < _splitValue)
                {
                    return false;
                }
                else if (dataNode.X > _splitValue)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                //Split on Y
                if (dataNode.Bottom < _splitValue)
                {
                    return false;
                }
                else if (dataNode.Y > _splitValue)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Split the node in three parts... lower, upper and intersect with the middle
        /// </summary>
        private bool Split()
        {

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;


            foreach (Space2DTreeNodeData<T> data in _bucket)
            {
                if (data.Right > maxX)
                    maxX = data.Right;
                if (data.X < minX)
                    minX = data.X;
                if (data.Bottom > maxY)
                    maxY = data.Bottom;
                if (data.Y < minY)
                    minY = data.Y;
            }


            //Console.WriteLine("Split " + minX + " " + maxX + " " + minY + " " + maxY);

            if (maxX - minX == 0 && maxY - minY == 0)
                //We don't split..
                return false;


            

            if (maxX - minX > maxY - minY)
            {
                //Split on X
                _splitOnX = true;
            }
            else
            {
                //Split on Y
                _splitOnX = false;
            }

            SplitInternal(minX, minY, maxX, maxY);

            if (IsChildNodesAllInOnePlace())
            {

                _splitOnX = !_splitOnX;
                SplitInternal(minX, minY, maxX, maxY);

                if (IsChildNodesAllInOnePlace())
                {
                    _splitTestDone = true;
                    _lowerNode = null;
                    _upperNode = null;
                    _intersectNode = null;
                    return false;
                }
            }


            //No more data in here...
            _bucket = null;

            return true;
        }

        /// <summary>
        /// Check if data in children are all in one place
        /// </summary>
        /// <returns></returns>
        private bool IsChildNodesAllInOnePlace()
        {
            int lower = (_lowerNode._bucket == null ? 1 : _lowerNode._bucket.Count);
            int upper = (_upperNode._bucket == null ? 1 : _upperNode._bucket.Count);
            int intersects = (_intersectNode._bucket == null ? 1 : _intersectNode._bucket.Count);

            //We we have data in more then one bucket, it's all good...
            if((lower != 0 ? 1 : 0) + (upper != 0 ? 1 : 0) + (intersects != 0 ? 1 : 0) != 1)
                return false;

            //if (lower >= SpaceTreeConstants.BUCKET_CAPACITY || upper >= SpaceTreeConstants.BUCKET_CAPACITY || intersects >= SpaceTreeConstants.BUCKET_CAPACITY)
            return true;

        }

        /// <summary>
        /// Internal split
        /// </summary>
        private void SplitInternal(float minX, float minY, float maxX, float maxY)
        {
            if (_splitOnX)
            {
                //Split on X
                _splitValue = minX + ((maxX - minX) / 2);
            }
            else
            {
                //Split on Y
                _splitValue = minY + ((maxY - minY) / 2);
            }


            //Creation of our 2 child node...
            _lowerNode = new Space2DTreeNode<T>(this);
            _upperNode = new Space2DTreeNode<T>(this);
            _intersectNode = new Space2DTreeNode<T>(this);

            //And we split all that in out children...
            for (int index = 0; index < _bucket.Count; index++)
            {
                Space2DTreeNodeData<T> data = _bucket[index];
                AddToChildrenNode(ref data);
            }
        }
    }
}
