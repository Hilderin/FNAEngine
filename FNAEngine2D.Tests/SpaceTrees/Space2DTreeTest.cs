using FNAEngine2D.SpaceTrees;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests.SpaceTrees
{
    /// <summary>
    /// Tests for Space2DTree
    /// </summary>
    [TestClass]
    public class Space2DTreeTest
    {
        [TestMethod]
        public void Space2DTreeAdd1Test()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            tree.Add(0, 0, 100, 100, new DataTest("test"));

            List<DataTest> data = tree.GetValues(0, 0, 100, 100);

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual("test", data[0].Data);

        }

        [TestMethod]
        public void Space2DTreeAdd1EnumeratorTest()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            tree.Add(0, 0, 100, 100, new DataTest("test"));

            List<DataTest> data = new List<DataTest>(tree.Search(0, 0, 100, 100));

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual("test", data[0].Data);

        }

        [TestMethod]
        public void Space2DTreeAdd2Test()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            tree.Add(0, 0, 100, 100, new DataTest("test"));
            tree.Add(10, 10, 100, 100, new DataTest("test2"));

            List<DataTest> data = tree.GetValues(0, 0, 100, 100);

            Assert.AreEqual(2, data.Count);
            Assert.AreEqual("test", data[0].Data);
            Assert.AreEqual("test2", data[1].Data);

        }

        [TestMethod]
        public void Space2DTreeAdd2EnumeratorTest()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            tree.Add(0, 0, 100, 100, new DataTest("test"));
            tree.Add(10, 10, 100, 100, new DataTest("test2"));

            List<DataTest> data = new List<DataTest>(tree.Search(0, 0, 100, 100));

            Assert.AreEqual(2, data.Count);
            Assert.AreEqual("test", data[0].Data);
            Assert.AreEqual("test2", data[1].Data);

        }

        [TestMethod]
        public void Space2DTreeAdd20SameXTest()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            for(int index = 0; index < 20; index++)
                tree.Add(index * 10, 0, 100, 100, new DataTest("test" + index.ToString("00")));

            List<DataTest> data = tree.GetValues(100, 0, 100, 100);

            Assert.AreEqual(20, data.Count);

            data.Sort((a, b) => a.Data.CompareTo(b.Data));
            for (int index = 0; index < 20; index++)
                Assert.AreEqual("test" + index.ToString("00"), data[index].Data);

        }

        [TestMethod]
        public void Space2DTreeAdd20SameXEnumeratorTest()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            for (int index = 0; index < 20; index++)
                tree.Add(index * 10, 0, 100, 100, new DataTest("test" + index.ToString("00")));

            List<DataTest> data = new List<DataTest>(tree.Search(100, 0, 100, 100));

            Assert.AreEqual(20, data.Count);

            data.Sort((a, b) => a.Data.CompareTo(b.Data));
            for (int index = 0; index < 20; index++)
                Assert.AreEqual("test" + index.ToString("00"), data[index].Data);

        }

        [TestMethod]
        public void Space2DTreeAdd20SameXSmallerSearchTest()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            for (int index = 0; index < 20; index++)
                tree.Add(index * 10, 0, 100, 100, new DataTest("test" + index));

            List<DataTest> data = tree.GetValues(100, 0, 1, 1);

            Assert.AreEqual(11, data.Count);

        }

        [TestMethod]
        public void Space2DTreeAdd2DifferentPlacesWideSearchTest()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            for (int index = 0; index < 20; index++)
                tree.Add(index * 100, -500, 100, 100, new DataTest("test" + index));

            tree.Add(20, 20, 20, 20, new DataTest("test"));
            tree.Add(1000, 2000, 100, 100, new DataTest("test2"));

            List<DataTest> data = tree.GetValues(100, 100, 10000, 10000);

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual("test2", data[0].Data);

        }

        [TestMethod]
        public void Space2DTreeAdd2DifferentPlacesWideSearchEnumeratorTest()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            for (int index = 0; index < 20; index++)
                tree.Add(index * 100, -500, 100, 100, new DataTest("test" + index));

            tree.Add(20, 20, 20, 20, new DataTest("test"));
            tree.Add(1000, 2000, 100, 100, new DataTest("test2"));

            List<DataTest> data = new List<DataTest>(tree.Search(100, 100, 10000, 10000));

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual("test2", data[0].Data);

        }

        [TestMethod]
        public void Space2DTreeAdd2DifferentPlacesSmallSearchTest()
        {
            Space2DTree<DataTest> tree = new Space2DTree<DataTest>();

            for (int index = 0; index < 20; index++)
                tree.Add(index * 100, -500, 100, 100, new DataTest("test" + index));

            tree.Add(20, 20, 20, 20, new DataTest("test"));
            tree.Add(1000, 2000, 100, 100, new DataTest("test2"));

            List<DataTest> data = tree.GetValues(1005, 2005, 10, 10);

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual("test2", data[0].Data);

        }


        /// <summary>
        /// Data test
        /// </summary>
        private class DataTest
        {
            public string Data { get; set; }
            public DataTest(string data)
            {
                this.Data = data;
            }
        }
    }
}
