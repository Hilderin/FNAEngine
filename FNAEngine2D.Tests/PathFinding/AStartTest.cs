using FNAEngine2D.PathFinding;
using FNAEngine2D.SpaceTrees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests.PathFinding
{
    /// <summary>
    /// Tests for AStartTest
    /// </summary>
    [TestClass]
    public class AStartTest
    {
        [TestMethod]
        public void AlreadyAtDestination()
        {
            Space2DTree<string> space2DTree = new Space2DTree<string>();

            AStar aStar = new AStar(space2DTree, 25);

            Path path = aStar.Search(new Vector2(0, 0), new Vector2(25, 0));

            Assert.IsNotNull(path);
            Assert.AreEqual(0, path.Nodes.Count);
        }

        [TestMethod]
        public void StraitLine()
        {
            Space2DTree<string> space2DTree = new Space2DTree<string>();

            AStar aStar = new AStar(space2DTree, 25);

            Path path = aStar.Search(new Vector2(0, 0), new Vector2(50, 0));

            Assert.IsNotNull(path);
            Assert.AreEqual(1, path.Nodes.Count);
            Assert.AreEqual(25f, path.Nodes[0].Location.X);
            Assert.AreEqual(0f, path.Nodes[0].Location.Y);

        }

        [TestMethod]
        public void ObstableOnTheRight()
        {
            const float cellSize = 25;
            /*
            S = Start, D = Destination

            SA  D
             A

            */
            Space2DTree<string> space2DTree = new Space2DTree<string>();
            space2DTree.Add(cellSize, 0, cellSize, cellSize * 2, "A");

            AStar aStar = new AStar(space2DTree, cellSize);

            Path path = aStar.Search(new Vector2(0, 0), new Vector2(cellSize * 4, 0));

            //Best path shoud be on up, right, right
            Assert.IsNotNull(path);
            Assert.AreEqual(5, path.Nodes.Count);
            Assert.AreEqual(0f, path.Nodes[0].Location.X);
            Assert.AreEqual(-cellSize, path.Nodes[0].Location.Y);

        }


        [TestMethod]
        public void ObstableOnTheRightLForm()
        {
            const float cellSize = 1;
            /*
            S = Start, D = Destination

            S ABB 
              A D
              A
            */
            Space2DTree<string> space2DTree = new Space2DTree<string>();
            space2DTree.Add(cellSize * 2, 0, cellSize, cellSize * 3, "A");
            space2DTree.Add(cellSize * 3, 0, cellSize * 2, cellSize, "B");

            AStar aStar = new AStar(space2DTree, cellSize);

            Path path = aStar.Search(new Vector2(0, 0), new Vector2(cellSize * 4, cellSize));

            //Best path shoud be on up, right, right
            Assert.IsNotNull(path);
            Assert.AreEqual(8, path.Nodes.Count);

        }

        [TestMethod]
        public void ObstableOnTheRightLForm2()
        {
            const float cellSize = 1;
            /*
            S = Start, D = Destination

            S ABB 
              A D
              A
            */
            Space2DTree<string> space2DTree = new Space2DTree<string>();
            space2DTree.Add(cellSize * 2, 0, cellSize, cellSize * 3, "A");
            space2DTree.Add(cellSize * 3, 0, cellSize * 2, cellSize, "B");

            AStar aStar = new AStar(space2DTree, cellSize);

            Path path = aStar.Search(new Vector2(0, 0), new Vector2(cellSize * 4, cellSize * 2));

            //Best path shoud be on up, right, right
            Assert.IsNotNull(path);
            Assert.AreEqual(7, path.Nodes.Count);

        }

    }
}
