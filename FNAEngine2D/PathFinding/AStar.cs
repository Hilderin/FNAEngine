using FNAEngine2D.SpaceTrees;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FNAEngine2D.PathFinding
{
    /// <summary>
    /// A* PathFinding
    /// </summary>
    public class AStar
    {
        /// <summary>
        /// Number of nodes open max in memory
        /// </summary>
        private const int NB_OPEN_NODE_MAX = 1000;

        /// <summary>
        /// Max index of nodes open max
        /// </summary>
        private const int INDEX_OPEN_NODE_MAX = NB_OPEN_NODE_MAX - 1;

        /// <summary>
        /// Number of nodes closed max in memory
        /// </summary>
        private const int NB_CLOSED_NODE_MAX = 10000;

        /// <summary>
        /// Max index of nodes closed
        /// </summary>
        private const int INDEX_CLOSED_NODE_MAX = NB_CLOSED_NODE_MAX - 1;

        /// <summary>
        /// Offset for the successors
        /// </summary>
        private static readonly OffsetIndex[] SUCCESSORS_OFFSETS = new OffsetIndex[] { new OffsetIndex(0, -1),      //Up
                                                                                       new OffsetIndex(1, 0),       //Right
                                                                                       new OffsetIndex(0, 1),       //Down
                                                                                       new OffsetIndex(-1, 0),      //Left
        };

        /// <summary>
        /// Number of successors
        /// </summary>
        private static readonly int NB_SUCCESSORS = SUCCESSORS_OFFSETS.Length;


        /// <summary>
        /// Open nodes
        /// We keep the same array at each search to limit the memory usage
        /// </summary>
        private AStarNode[] _openNodes = new AStarNode[NB_OPEN_NODE_MAX];

        /// <summary>
        /// Closed nodes
        /// We keep the same array at each search to limit the memory usage
        /// </summary>
        private AStarNode[] _closedNodes = new AStarNode[NB_CLOSED_NODE_MAX];


        /// <summary>
        /// SearchableSpace
        /// </summary>
        private ISearchableSpace _searchableSpace;

        /// <summary>
        /// Cell size
        /// </summary>
        private float _cellSize;

        /// <summary>
        /// Cell size for checking to be sure the object fit in the space
        /// </summary>
        private float _cellSizeCheck;

        /// <summary>
        /// Middle of the cell
        /// </summary>
        private float _cellSizeMiddle;

        /// <summary>
        /// Constructor
        /// </summary>
        public AStar(ISearchableSpace searchableSpace, float cellSize)
        {
            _searchableSpace = searchableSpace;
            _cellSize = cellSize;
            _cellSizeCheck = cellSize - (GameMath.EPSILON * 2);
            _cellSizeMiddle = cellSize * 0.5f;

        }

        /// <summary>
        /// Search a path
        /// </summary>
        public Path Search(Vector2 startLocation, Vector2 target)
        {
            int targetX = LocationToIndex(target.X);
            int targetY = LocationToIndex(target.Y);

            

            // A* Search Algorithm
            //Initialize the lists
            AStarNode[] openNodes = _openNodes;
            AStarNode[] closedNodes = _closedNodes;
            int indexOpenNodes = 0;
            int indexClosedNodes = -1;

            //Put the start node in the open nodes
            openNodes[indexOpenNodes] = new AStarNode(LocationToIndex(startLocation.X), LocationToIndex(startLocation.Y));


            AStarNode destinationNode;

            //While the open list is not empty
            while (indexOpenNodes >= 0)
            {
                //find the node with the least f on the open list, call it "q"
                int qIndex = -1;
                for (int index = 0; index <= indexOpenNodes; index++)
                {
                    if (qIndex == -1 || openNodes[qIndex].F > openNodes[index].F)
                    {
                        qIndex = index;
                    }
                }


                //pop q off the open list
                AStarNode qNode = openNodes[qIndex];
                if (qIndex < indexOpenNodes)
                {
                    Array.Copy(openNodes, qIndex + 1, openNodes, qIndex, indexOpenNodes - qIndex);                    
                }
                indexOpenNodes--;


                //generate q's 4 successors and set their parents to q
                //for each successor
                for (int indexSuccessor = 0; indexSuccessor < NB_SUCCESSORS; indexSuccessor++)
                {
                    
                    int successorX = qNode.X + SUCCESSORS_OFFSETS[indexSuccessor].X;
                    int successorY = qNode.Y + SUCCESSORS_OFFSETS[indexSuccessor].Y;

                    if (successorX == targetX && successorY == targetY)
                    {
                        //Found!
                        destinationNode = qNode;
                        //I use a goto to quickly skip the 2 loops
                        goto destination_found;
                    }


                    if (indexOpenNodes == INDEX_OPEN_NODE_MAX)
                        //We have enough, we can't go above the max node to check...
                        continue;


                    //Check if the cell is available
                    if (!IsCellAvailable(successorX, successorY))
                        continue;



                    AStarNode successor = new AStarNode(successorX, successorY);
                    
                    //compute both g and h for successor                   
                    successor.G = qNode.G + 1;                    
                    successor.H = HeuristicFunction(successor.X, successor.Y, targetX, targetY);                    
                    successor.F = successor.G + successor.H;
                    successor.Parent = qNode;


                    //if a node with the same position as successor is in the OPEN list which has a lower f than successor, skip this successor
                    bool skipSuccessor = false;
                    for (int indexOpen = 0; indexOpen <= indexOpenNodes; indexOpen++)
                    {
                        if (openNodes[indexOpen].X == successor.X && openNodes[indexOpen].Y == successor.Y && openNodes[indexOpen].F < successor.F)
                        {
                            //Skip the successor...
                            skipSuccessor = true;
                            break;
                        }

                    }

                    if (skipSuccessor)
                        continue;


                    //if a node with the same position as successor is in the CLOSED list which has a lower f than successor, skip this successor
                    for (int indexClosed = 0; indexClosed <= indexClosedNodes; indexClosed++)
                    {
                        if (closedNodes[indexClosed].X == successor.X && closedNodes[indexClosed].Y == successor.Y && closedNodes[indexClosed].F < successor.F)
                        {
                            //Skip the successor...
                            skipSuccessor = true;
                            break;
                        }

                    }

                    if (skipSuccessor)
                        continue;


                    //We keep this successor
                    openNodes[++indexOpenNodes] = successor;
                }

                //push q on the closed list
                if (indexClosedNodes == INDEX_CLOSED_NODE_MAX)
                    //We have reach the maximum number of nodes to check...
                    break;

                closedNodes[++indexClosedNodes] = qNode;

            }

            //We did not found a path!
            return null;

        destination_found:

            //We found a path, we will create it from the finish line...
            Path path = new Path();

            AStarNode travelNode = destinationNode;
            while (travelNode.Parent != null)
            {
                path.Nodes.Add(new PathNode(new Vector2(IndexToLocation(travelNode.X), IndexToLocation(travelNode.Y))));
                travelNode = travelNode.Parent;
            }

            //We reverse the order so the first element is the first place to travel
            path.Nodes.Reverse();

            return path;
        }

        /// <summary>
        /// Check is there is some collision at a coord
        /// </summary>
        private bool IsCellAvailable(int x, int y)
        {
            return !_searchableSpace.Any(x * _cellSize + GameMath.EPSILON, y * _cellSize + GameMath.EPSILON, _cellSizeCheck, _cellSizeCheck);
        }

        /// <summary>
        /// Calculte the H
        /// </summary>
        private float HeuristicFunction(int currentX, int currentY, int targetX, int targetY)
        {
            //Manhattan Distance...
            //It is nothing but the sum of absolute values of differences in the goal’s x and y coordinates and the current cell’s x and y coordinates respectively
            return Math.Abs(currentX - targetX) + Math.Abs(currentY - targetY);

        }


        /// <summary>
        /// Convert a location (x or y) (in float) to an index
        /// </summary>
        private int LocationToIndex(float location)
        {
            return (int)(location / _cellSize);
        }

        /// <summary>
        /// Convert an index to a location (x or y)
        /// </summary>
        private float IndexToLocation(int index)
        {
            return index * _cellSize;
        }


        /// <summary>
        /// A* Node
        /// </summary>
        private class AStarNode
        {
            /// <summary>
            /// Index X on the grid
            /// </summary>
            public int X;

            /// <summary>
            /// Index Y on the grid
            /// </summary>
            public int Y;

            /// <summary>
            /// g = the movement cost to move from the starting point to a given square on the grid, following the path generated to get there
            /// </summary>
            public float G;

            /// <summary>
            /// h = the estimated movement cost to move from that given square on the grid to the final destination.
            /// This is often referred to as the heuristic, which is nothing but a kind of smart guess. 
            /// We really don’t know the actual distance until we find the path, because all sorts of things can be in the way (walls, water, etc.).
            /// </summary>
            public float H;

            /// <summary>
            /// f which is a parameter equal to the sum of two other parameters – g and h. 
            /// </summary>
            public float F;

            /// <summary>
            /// Parent node
            /// </summary>
            public AStarNode Parent;

            /// <summary>
            /// Constructor
            /// </summary>
            public AStarNode(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override string ToString()
            {
                return this.X + ", " + this.Y + " G: " + this.G + " H: " + this.H + " F: " + this.F;

            }
        }


        /// <summary>
        /// An offset on the grid
        /// </summary>
        private struct OffsetIndex
        {
            /// <summary>
            /// Index X on the grid
            /// </summary>
            public int X;

            /// <summary>
            /// Index Y on the grid
            /// </summary>
            public int Y;

            /// <summary>
            /// Constructor
            /// </summary>
            public OffsetIndex(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }


    }
}
