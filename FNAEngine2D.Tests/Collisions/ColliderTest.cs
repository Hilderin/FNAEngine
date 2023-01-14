using FNAEngine2D.Collisions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;

namespace FNAEngine2D.Tests.Collisions
{
    [TestClass]
    public class ColliderTest
    {
        [TestMethod]
        public void IntersectsRectTest()
        {
            Collider rectCollider = new ColliderRectangle(5, -5, 20, 20);

            bool result = rectCollider.Intersects(new ColliderRectangle(0, 0, 10, 10));

            Assert.IsTrue(result);

        }

        [TestMethod]
        public void IntersectsRectWidthRadiusTest()
        {
            Collider rectCollider = new ColliderRectangle(5, -5, 20, 20);

            bool result = rectCollider.Intersects(new ColliderCircle(new Vector2(5, 5), 10f));

            Assert.IsTrue(result);

        }

    }
}
