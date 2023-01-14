using FNAEngine2D.Physics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests.Physics
{
    /// <summary>
    /// Tests for forces
    /// </summary>
    [TestClass]
    public class ForceTest
    {
        [TestMethod]
        public void ApplyForce()
        {
            using (Game game = new Game())
            {
                game.NbPixelPerMeter = 10;
                GameObject gameObject = new GameObject();

                Force force = new Force(new Vector2(10, 0), 1, gameObject);

                //0.016f = 60fs
                Vector2 applied = force.Apply(Vector2.Zero, 0.016f);

                Assert.AreEqual(Math.Round(0.16f, 2).ToString("0.00"), Math.Round(applied.X, 2).ToString("0.00"));
                Assert.AreEqual(0f, applied.Y);


                //Second frame...
                applied = force.Apply(applied, 0.016f);

                Assert.AreEqual(Math.Round(0.32f, 2).ToString("0.00"), Math.Round(applied.X, 2).ToString("0.00"));
                Assert.AreEqual(0f, applied.Y);
            }

        }


        [TestMethod]
        public void ApplyForce60frames()
        {
            using (Game game = new Game())
            {
                game.NbPixelPerMeter = 10;
                GameObject gameObject = new GameObject();

                Vector2 target = new Vector2(10, 0);
                Force force = new Force(target, 1, gameObject);

                Vector2 applied = Vector2.Zero;
                for (int cpt = 0; cpt < 61; cpt++)
                {
                    //0.016f = 60fs
                    applied = force.Apply(applied, 1f / 60);

                    float expected = (cpt + 1) * (10f / 60);
                    if (expected > 10)
                    {
                        expected = 10;
                        Assert.AreEqual(true, force.IsCompleted);
                    }
                    else
                    {
                        Assert.AreEqual(false, force.IsCompleted);
                    }
                    Assert.AreEqual(Math.Round(expected, 2).ToString("0.00"), Math.Round(applied.X, 2).ToString("0.00"));
                    Assert.AreEqual(0f, applied.Y);

                }

                //After 1 secs, should have arrived
                Assert.AreEqual(target.X, applied.X);
                Assert.AreEqual(target.Y, applied.Y);
                Assert.AreEqual(true, force.IsCompleted);
            }
        }

        [TestMethod]
        public void ApplyForce60framesNegative()
        {
            using (Game game = new Game())
            {
                game.NbPixelPerMeter = 10;
                GameObject gameObject = new GameObject();

                Vector2 target = new Vector2(-10, 0);
                Force force = new Force(target, 1, gameObject);

                Vector2 applied = Vector2.Zero;
                for (int cpt = 0; cpt < 61; cpt++)
                {
                    //0.016f = 60fs
                    applied = force.Apply(applied, 1f / 60);

                    float expected = (cpt + 1) * (-10f / 60);
                    if (expected < -10)
                    {
                        expected = -10;
                        Assert.AreEqual(true, force.IsCompleted);
                    }
                    else
                    {
                        Assert.AreEqual(false, force.IsCompleted);
                    }
                    Assert.AreEqual(Math.Round(expected, 2).ToString("0.00"), Math.Round(applied.X, 2).ToString("0.00"));
                    Assert.AreEqual(0f, applied.Y);

                }

                //After 1 secs, should have arrived
                Assert.AreEqual(target.X, applied.X);
                Assert.AreEqual(target.Y, applied.Y);
                Assert.AreEqual(true, force.IsCompleted);
            }

        }
    }
}
