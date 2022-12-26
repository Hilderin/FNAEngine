﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Collision collision = CollisionHelper.GetCollision(new Vector2(10, 10), new Vector2(10, 10), new Collider(5, 15, 20, 10));
            Assert.IsNotNull(collision);
            Assert.AreEqual(CollisionDirection.MovingColliderOnTop, collision.Direction);

        }

        [TestMethod]
        public void GetCollision_Bottom()
        {
            Collision collision = CollisionHelper.GetCollision(new Vector2(10, 15), new Vector2(10, 10), new Collider(5, 10, 20, 10));
            Assert.IsNotNull(collision);
            Assert.AreEqual(CollisionDirection.MovingColliderOnBottom, collision.Direction);

        }

        [TestMethod]
        public void GetCollision_Left()
        {
            Collision collision = CollisionHelper.GetCollision(new Vector2(10, 10), new Vector2(10, 10), new Collider(15, 5, 10, 20));
            Assert.IsNotNull(collision);
            Assert.AreEqual(CollisionDirection.MovingColliderOnLeft, collision.Direction);

        }

        [TestMethod]
        public void GetCollision_LeftWidthALittleBottom()
        {
            Collision collision = CollisionHelper.GetCollision(new Vector2(10, 10), new Vector2(10, 10), new Collider(18, 12, 4, 12));
            Assert.IsNotNull(collision);
            Assert.AreEqual(CollisionDirection.MovingColliderOnLeft, collision.Direction);

        }


        [TestMethod]
        public void GetCollision_Right()
        {
            Collision collision = CollisionHelper.GetCollision(new Vector2(15, 10), new Vector2(10, 10), new Collider(10, 5, 10, 20));
            Assert.IsNotNull(collision);
            Assert.AreEqual(CollisionDirection.MovingColliderOnRight, collision.Direction);

        }

        [TestMethod]
        public void GetCollision_RightWidthALittleBottom()
        {
            Collision collision = CollisionHelper.GetCollision(new Vector2(10, 10), new Vector2(10, 10), new Collider(7, 7, 4, 12));
            Assert.IsNotNull(collision);
            Assert.AreEqual(CollisionDirection.MovingColliderOnRight, collision.Direction);

        }

        [TestMethod]
        public void GetCollision_Over()
        {
            Collision collision = CollisionHelper.GetCollision(new Vector2(10, 10), new Vector2(100, 100), new Collider(20, 20, 10, 20));
            Assert.IsNotNull(collision);
            Assert.AreEqual(CollisionDirection.MovingColliderOver, collision.Direction);

        }

        [TestMethod]
        public void GetCollision_In()
        {
            Collision collision = CollisionHelper.GetCollision(new Vector2(20, 20), new Vector2(10, 20), new Collider(10, 10, 100, 100));
            Assert.IsNotNull(collision);
            Assert.AreEqual(CollisionDirection.MovingColliderIn, collision.Direction);

        }

        [TestMethod]
        public void GetCollision_None()
        {
            Collision collision = CollisionHelper.GetCollision(new Vector2(2000, 20), new Vector2(10, 20), new Collider(10, 10, 100, 100)); 
            Assert.IsNull(collision);
        }

        [TestMethod]
        public void GetCollisionTravel()
        {
            Collision collision = null;
            bool ret = CollisionHelper.GetCollisionTravel(new Vector2(0, 5), new Vector2(1000, 5), new Vector2(5, 5), new Collider(10, 0, 10, 100), ref collision);

            Assert.IsTrue(ret);
            Assert.AreEqual(5, collision.StopLocation.X);
            Assert.AreEqual(5, collision.StopLocation.Y);
        }
    }
}
