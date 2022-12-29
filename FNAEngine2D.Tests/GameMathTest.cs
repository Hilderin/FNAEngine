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
    public class GameMathTest
    {
        [TestMethod]
        public void RadToDeg()
        {
            Assert.AreEqual("0.0000", Math.Round(GameMath.RadToDeg(0), 4).ToString("0.0000"));
            Assert.AreEqual("90.0000", Math.Round(GameMath.RadToDeg(VectorHelper.ToAngle(new Vector2(0, 1))), 4).ToString("0.0000"));
            Assert.AreEqual("-90.0000", Math.Round(GameMath.RadToDeg(VectorHelper.ToAngle(new Vector2(0, -1))), 4).ToString("0.0000"));
            Assert.AreEqual("180.0000", Math.Round(GameMath.RadToDeg(VectorHelper.ToAngle(new Vector2(-1, 0))), 4).ToString("0.0000"));


        }

    }
}
