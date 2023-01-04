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
        private int _currentNodeNumber = 0;
        private Space2DTreeNode<T> _previousNode;
        private List<Space2DTreeNodeData<T>> _currentBucket;
        private int _currentBucketLength = 0;
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
            while (true)
            {
                if (_currentNode._lowerNode == null)
                {
                    //Check all the data in the bucket...
                    if (_currentBucket == null)
                    {
                        _currentBucket = _currentNode._bucket;
                        _currentBucketIndex = 0;
                        _currentBucketLength = _currentBucket.Count;
                    }
                    while(_currentBucketIndex < _currentBucketLength)
                    {
                        if (_currentBucket[_currentBucketIndex].Intersects(_x, _y, _right, _bottom))
                        {
                            _current = _currentBucket[_currentBucketIndex].Data;
                            _currentBucketIndex++;
                            return true;
                        }
                        _currentBucketIndex++;
                    }

                    _currentBucket = null;
                }
                else
                {
                    if (_currentNode._splitOnX)
                    {
                        //Split on X
                        if (_currentNodeNumber == 0 && _x <= _currentNode._splitValue)
                        {
                            _currentNode = _currentNode._lowerNode;
                            _currentNodeNumber = 0;
                            _previousNode = _currentNode;
                            continue;
                        }
                        if (_currentNodeNumber < 2 && _right >= _currentNode._splitValue)
                        {
                            _currentNode = _currentNode._upperNode;
                            _currentNodeNumber = 0;
                            _previousNode = _currentNode;
                            continue;
                        }

                        if (_currentNodeNumber < 3)
                        {
                            _currentNode = _currentNode._intersectNode;
                            _currentNodeNumber = 0;
                            _previousNode = _currentNode;
                            continue;
                        }
                    }
                    else
                    {
                        //Split on Y
                        if (_currentNodeNumber == 0 && _y <= _currentNode._splitValue)
                        {
                            _currentNode = _currentNode._lowerNode;
                            _currentNodeNumber = 0;
                            _previousNode = _currentNode;
                            continue;
                        }
                        if (_currentNodeNumber < 2 && _bottom >= _currentNode._splitValue)
                        {
                            _currentNode = _currentNode._upperNode;
                            _currentNodeNumber = 0;
                            _previousNode = _currentNode;
                            continue;
                        }

                        if (_currentNodeNumber < 3)
                        {
                            _currentNode = _currentNode._intersectNode;
                            _currentNodeNumber = 0;
                            _previousNode = _currentNode;
                            continue;
                        }

                    }
                }

                //We need the check the other buckets...
                if (_currentNode._parent == null)
                    //The end
                    return false;

                _previousNode = _currentNode;
                _currentNode = _currentNode._parent;

                if (_previousNode == _currentNode._lowerNode)
                    _currentNodeNumber = 1;
                else if (_previousNode == _currentNode._upperNode)
                    _currentNodeNumber = 2;
                else if (_previousNode == _currentNode._intersectNode)
                    _currentNodeNumber = 3;
                else
                    _currentNodeNumber = 0;
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
