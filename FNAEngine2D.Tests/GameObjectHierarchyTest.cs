using FNAEngine2D.Aseprite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests
{
    /// <summary>
    /// Tests for hierarchy of game object/component
    /// </summary>
    [TestClass]
    public class GameObjectHierarchyTest
    {
        [TestMethod]
        public void EmptyGameObject()
        {

            EmptyGameObject obj = new EmptyGameObject();

            Assert.IsTrue(obj.VisibleSelf);
            Assert.IsFalse(obj.Visible);

            Assert.IsTrue(obj.EnabledSelf);
            Assert.IsFalse(obj.Enabled);

            Assert.IsFalse(obj.PausedSelf);
            Assert.IsFalse(obj.Paused);

            Assert.IsFalse(obj.IsOnScene);


        }

        [TestMethod]
        public void GameObject_VisibleSelfFalse()
        {

            EmptyGameObject obj = new EmptyGameObject();

            obj.VisibleSelf = false;

            Assert.IsFalse(obj.VisibleSelf);
            Assert.IsFalse(obj.Visible);

            Assert.IsTrue(obj.EnabledSelf);
            Assert.IsFalse(obj.Enabled);

            Assert.IsFalse(obj.PausedSelf);
            Assert.IsFalse(obj.Paused);

            Assert.IsFalse(obj.IsOnScene);

        }

        [TestMethod]
        public void GameObject_RootVisibleEnabled()
        {

            EmptyGameObject obj = new EmptyGameObject();

            using (Game game = new Game())
            {
                game.RootGameObject = obj;
                game.RunUnitTestOneFrame();

                Assert.IsTrue(obj.VisibleSelf);
                Assert.IsTrue(obj.Visible);

                Assert.IsTrue(obj.EnabledSelf);
                Assert.IsTrue(obj.Enabled);

                Assert.IsFalse(obj.PausedSelf);
                Assert.IsFalse(obj.Paused);

                Assert.IsTrue(obj.IsOnScene);

                Assert.AreEqual(0, game.GetDrawables().Length);
                Assert.AreEqual(0, game.GetUpdateables().Length);

            }
        }


        [TestMethod]
        public void GameObject_RootComponentVisibleEnabled()
        {
            bool rootAdded = false;

            TestGameObject root = new TestGameObject();

            root.OnLoadAction = () =>
            {
                root.AddComponent<UpdatableComponent>();
                root.AddComponent<DrawableComponent>();
            };

            root.OnAddedAction = () =>
            {
                rootAdded = true;
            };

            using (Game game = new Game())
            {
                game.RootGameObject = root;
                
                game.RunUnitTestOneFrame();

                Assert.IsTrue(rootAdded);
                Assert.AreEqual(1, game.GetDrawables().Length);
                Assert.AreEqual(1, game.GetUpdateables().Length);

            }
        }


        [TestMethod]
        public void GameObject_ChildVisibleEnabled()
        {
            bool rootAdded = false;

            TestGameObject root = new TestGameObject();
            TestGameObject child = new TestGameObject();
            child.AddComponent<UpdatableComponent>();

            root.OnLoadAction = () =>
            {
                root.AddComponent<UpdatableComponent>();
                root.AddComponent<DrawableComponent>();
                child = new TestGameObject()
                {
                    OnLoadAction = () =>
                    {
                        child.AddComponent<DrawableComponent>();
                    }
                };
                root.Add(child);
            };

            root.OnAddedAction = () =>
            {
                rootAdded = true;
            };

            using (Game game = new Game())
            {
                game.RootGameObject = root;

                Assert.IsNotNull(child.GetComponent<UpdatableComponent>());
                Assert.IsNull(child.GetComponent<DrawableComponent>());

                game.RunUnitTestOneFrame();

                Assert.IsTrue(rootAdded);
                Assert.AreEqual(2, game.GetDrawables().Length);
                Assert.AreEqual(2, game.GetUpdateables().Length);

                Assert.IsNotNull(child.GetComponent<UpdatableComponent>());
                Assert.IsNotNull(child.GetComponent<DrawableComponent>());

            }
        }


        /// <summary>
        /// Test game object easylly customizable
        /// </summary>
        private class TestGameObject : GameObject
        {
            public Action OnLoadAction;
            public Action OnAddedAction;

            protected override void Load()
            {
                if (OnLoadAction != null)
                    OnLoadAction();
            }

            protected override void OnAdded()
            {
                if (OnAddedAction != null)
                    OnAddedAction();
            }
        }

        /// <summary>
        /// A updatable component to test
        /// </summary>
        private class UpdatableComponent : Component, IUpdate
        {
            public void Update()
            {
            }

        }

        /// <summary>
        /// A drawable component to test
        /// </summary>
        private class DrawableComponent : Component, IDraw
        {
            public void Draw()
            {
            }

        }

    }
}
