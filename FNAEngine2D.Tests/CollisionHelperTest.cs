using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;

namespace FNAEngine2D.Tests
{
    [TestClass]
    public class CollisionHelperTest
    {
        [TestMethod]
        public void GetCollision_Top()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(10, 10, 10, 10), new ColliderRectangle(5, 15, 20, 10));
            Assert.IsNotNull(collision);

        }

        [TestMethod]
        public void GetCollision_Bottom()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(10, 15, 10, 10), new ColliderRectangle(5, 10, 20, 10));
            Assert.IsNotNull(collision);

        }

        [TestMethod]
        public void GetCollision_Left()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(10, 10, 10, 10), new ColliderRectangle(15, 5, 10, 20));
            Assert.IsNotNull(collision);

        }

        [TestMethod]
        public void GetCollision_LeftWidthALittleBottom()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(10, 10, 10, 10), new ColliderRectangle(18, 12, 4, 12));
            Assert.IsNotNull(collision);

        }


        [TestMethod]
        public void GetCollision_Right()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(15, 10, 10, 10), new ColliderRectangle(10, 5, 10, 20));
            Assert.IsNotNull(collision);

        }

        [TestMethod]
        public void GetCollision_RightWidthALittleBottom()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(10, 10, 10, 10), new ColliderRectangle(7, 7, 4, 12));
            Assert.IsNotNull(collision);
        }

        [TestMethod]
        public void GetCollision_Over()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(10, 10, 100, 100), new ColliderRectangle(20, 20, 10, 20));
            Assert.IsNotNull(collision);

        }

        [TestMethod]
        public void GetCollision_In()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(20, 20, 10, 20), new ColliderRectangle(10, 10, 100, 100));
            Assert.IsNotNull(collision);

        }

        [TestMethod]
        public void GetCollision_None()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderRectangle(2000, 20, 10, 20), new ColliderRectangle(10, 10, 100, 100)); 
            Assert.IsNull(collision);
        }

        [TestMethod]
        public void GetCollisionTravel()
        {
            Collision collision = null;
            ColliderRectangle collider = new ColliderRectangle(0, 5, 5, 5);
            collider.MovingLocation = new Vector2(1000, 5);
            bool ret = CollisionHelper.GetCollisionTravel(collider, new ColliderRectangle(10, 0, 10, 100), ref collision);

            Assert.IsTrue(ret);
            Assert.AreEqual(5, collision.StopLocation.X);
            Assert.AreEqual(5, collision.StopLocation.Y);
        }

        [TestMethod]
        public void GetCollisionCircle()
        {
            Collision collision = CollisionHelper.GetCollision(new ColliderCircle(new Vector2(0, 0), new Vector2(5, 5), 10f), new ColliderRectangle(10, 10, 100, 100));
            Assert.IsNotNull(collision);
        }


        [TestMethod]
        public void IntersectsRectCircleOutside()
        {
            bool result = CollisionHelper.Intersects(new Vector2(0, 0), 10f, new Vector2(100, 100), new Vector2(20, 20));
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IntersectsRectCircleInside()
        {
            bool result = CollisionHelper.Intersects(new Vector2(110, 110), 10f, new Vector2(100, 100), new Vector2(20, 20));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IntersectsRectCircleLeft()
        {
            bool result = CollisionHelper.Intersects(new Vector2(10, 10), 10f, new Vector2(15, 0), new Vector2(100, 100));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IntersectsRectCircleRight()
        {
            bool result = CollisionHelper.Intersects(new Vector2(125, 10), 10f, new Vector2(15, 0), new Vector2(100, 100));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IntersectsRectCircleTop()
        {
            bool result = CollisionHelper.Intersects(new Vector2(0, -5), 10f, new Vector2(-10, 0), new Vector2(100, 100));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IntersectsRectCircleBottom()
        {
            bool result = CollisionHelper.Intersects(new Vector2(0, 105), 10f, new Vector2(-10, 0), new Vector2(100, 100));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IntersectsRectCircleCorner()
        {
            Vector2 hitLocation = Vector2.Zero;
            bool result = CollisionHelper.Intersects(new Vector2(10, 10), 10f, new Vector2(15, 15), new Vector2(100, 100));
            Assert.IsTrue(result);
        }
    }
}
