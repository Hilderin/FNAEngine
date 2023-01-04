using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.SpaceTrees
{
    /// <summary>
    /// Enumerator for a search in the tree
    /// </summary>
    public class Space2DTreeResult<T> : IEnumerator<T>, IEnumerable<T>
    {
        /// <summary>
        /// Current data
        /// </summary>
        private T _current;

        /// <summary>
        /// Current data
        /// </summary>
        public T Current => _current;

        /// <summary>
        /// Current data
        /// </summary>
        object IEnumerator.Current => _current;

        private float _x;
        private float _y;
        private float _right;
        private float _bottom;
        private Space2DTree<T> _tree;
        private Space2DTreeNode<T> _currentNode;
        private List<Space2DTreeNodeData<T>> _currentBucket;
        private int _currentBucketIndex = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public Space2DTreeResult(float x, float y, float width, float height, Space2DTree<T> tree)
        {
            _x = x;
            _y = y;
            _right = x + width;
            _bottom = y + height;
            _tree = tree;
            _currentNode = tree._root;
        }

        /// <summary>
        /// Move next 
        /// </summary>
        public bool MoveNext()
        {
            Space2DTreeNode<T> previousNode = _currentNode;

            while (true)
            {
                if (_currentNode._lowerNode == null)
                {
                    //Check all the data in the bucket...
                    if (_currentBucket == null)
                    {
                        _currentBucket = _currentNode._bucket;
                        _currentBucketIndex = 0;
                    }
                    for (; _currentBucketIndex < _currentBucket.Count; _currentBucketIndex++)
                    {
                        if (_currentBucket[_currentBucketIndex].Intersects(_x, _y, _right, _bottom))
                        {
                            _current = _currentBucket[_currentBucketIndex].Data;
                            _currentBucketIndex++;
                            return true;
                        }
                    }

                    _currentBucket = null;
                }
                else
                {
                    if (_currentNode._splitOnX)
                    {
                        //Split on X
                        if (_x <= _currentNode._splitValue && previousNode != _currentNode._lowerNode && previousNode != _currentNode._upperNode && previousNode != _currentNode._intersectNode)
                        {
                            _currentNode = _currentNode._lowerNode;
                            continue;
                        }
                        if (_right >= _currentNode._splitValue && previousNode != _currentNode._upperNode && previousNode != _currentNode._intersectNode)
                        {
                            _currentNode = _currentNode._upperNode;
                            continue;
                        }

                        if (previousNode != _currentNode._intersectNode)
                        {
                            _currentNode = _currentNode._intersectNode;
                            continue;
                        }
                    }
                    else
                    {
                        //Split on Y
                        if (_y <= _currentNode._splitValue && previousNode != _currentNode._lowerNode && previousNode != _currentNode._upperNode && previousNode != _currentNode._intersectNode)
                        {
                            _currentNode = _currentNode._lowerNode;
                            continue;
                        }
                        if (_bottom >= _currentNode._splitValue && previousNode != _currentNode._upperNode && previousNode != _currentNode._intersectNode)
                        {
                            _currentNode = _currentNode._upperNode;
                            continue;
                        }

                        if (previousNode != _currentNode._intersectNode)
                        {
                            _currentNode = _currentNode._intersectNode;
                            continue;
                        }

                    }
                }

                //We need the check the other buckets...
                if (_currentNode._parent == null)
                    //The end
                    return false;

                previousNode = _currentNode;
                _currentNode = _currentNode._parent;
            }
        }

        /// <summary>
        /// Reset the search at the beginning
        /// </summary>
        public void Reset()
        {
            _currentNode = _tree._root;
            _currentBucket = null;
            _currentBucketIndex = 0;
        }


        /// <summary>
        /// Dipose
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {

        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
