using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;

namespace FNAEngine2D.Tests
{
    [TestClass]
    public class CollisionTest
    {
        [TestMethod]
        public void IntersectsRectTest()
        {
            Collider rectCollider = new ColliderRectangle(5, -5, 20, 20);

            CollisionDirection direction;
            Vector2 stopLocation;

            bool result = rectCollider.Intersects(new Vector2(0, 0), new ColliderRectangle(0, 0, 10, 10), out direction, out stopLocation);

            Assert.IsTrue(result);
            Assert.AreEqual(CollisionDirection.MovingColliderOnLeft, direction);

        }

        [TestMethod]
        public void IntersectsRectWidthRadiusTest()
        {
            Collider rectCollider = new ColliderRectangle(5, -5, 20, 20);

            CollisionDirection direction;
            Vector2 stopLocation;

            bool result = rectCollider.Intersects(new Vector2(0, 0), new ColliderCircle(new Vector2(5, 5), 10f), out direction, out stopLocation);

            Assert.IsTrue(result);
            Assert.AreEqual(CollisionDirection.MovingColliderOnLeft, direction);

        }

    }
}
