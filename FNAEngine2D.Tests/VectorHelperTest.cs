using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests
{
    [TestClass]
    public class VectorHelperTest
    {
        [TestMethod]
        public void Rotation_OriginZero()
        {
            Vector2 toRotate = Vector2.Normalize(new Vector2(1, 1));

            Vector2 rotated = VectorHelper.Rotate(toRotate, Vector2.Zero, GameMath.DegToRad(15));

            Assert.AreEqual(0.8660253f, rotated.X);
            Assert.AreEqual((0.5f).ToString(), rotated.Y.ToString());

        }

        [TestMethod]
        public void Rotation_OriginNonZero()
        {
            Vector2 toRotate = Vector2.Normalize(new Vector2(1, 1));

            Vector2 rotated = VectorHelper.Rotate(toRotate, new Vector2(10, 20), GameMath.DegToRad(15));

            Assert.AreEqual(-3.969614f, rotated.X);
            Assert.AreEqual(3.7696743f, rotated.Y);

        }

        [TestMethod]
        public void ToAngle()
        {
            Assert.AreEqual(0f, VectorHelper.ToAngle(new Vector2(1, 0)));
            Assert.AreEqual("1.5708", Math.Round(VectorHelper.ToAngle(new Vector2(0, 1)), 4).ToString("0.0000"));
            Assert.AreEqual("-1.5708", Math.Round(VectorHelper.ToAngle(new Vector2(0, -1)), 4).ToString("0.0000"));
            Assert.AreEqual("3.1416", Math.Round(VectorHelper.ToAngle(new Vector2(-1, 0)), 4).ToString("0.0000"));

        }
    }
}
