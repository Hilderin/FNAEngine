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

            CollisionDirection direction = CollisionDirection.Indetermined;
            Vector2 stopLocation = Vector2.Zero;

            bool result = rectCollider.Intersects(new ColliderRectangle(0, 0, 10, 10), ref direction, ref stopLocation);

            Assert.IsTrue(result);
            Assert.AreEqual(CollisionDirection.MovingColliderOnLeft, direction);

        }

        [TestMethod]
        public void IntersectsRectWidthRadiusTest()
        {
            Collider rectCollider = new ColliderRectangle(5, -5, 20, 20);

            CollisionDirection direction = CollisionDirection.Indetermined;
            Vector2 stopLocation = Vector2.Zero;

            bool result = rectCollider.Intersects(new ColliderCircle(new Vector2(5, 5), 10f), ref direction, ref stopLocation);

            Assert.IsTrue(result);
            Assert.AreEqual(CollisionDirection.MovingColliderOnLeft, direction);

        }

    }
}
