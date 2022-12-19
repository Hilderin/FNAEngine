using FNAEngine2D.Aseprite;
using FNAEngine2D.Aseprite.Chunks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests
{
    [TestClass]
    public class AsepriteTest
    {
        [TestMethod]
        public void LoadFile()
        {

            using (MemoryStream ms = new MemoryStream(Resources.Base))
            {
                AseFile aseFile = new AseFile(ms);

                Assert.AreEqual(1, aseFile.Frames.Count);

                List<LayerChunk> layers = aseFile.GetChunks<LayerChunk>();
                Assert.AreEqual(1, layers.Count);

                int nbFrames = aseFile.Frames.Count;
                for (int index = 0; index < nbFrames; index++)
                {
                    Assert.AreEqual(100f, aseFile.Frames[index].FrameDuration);

                    List <CelChunk> cells = aseFile.Frames[index].GetChunks<CelChunk>();

                    //On ajoute les linked cells...
                    cells.AddRange(aseFile.Frames[index].GetChunks<LinkedCelChunk>().Select(l => l.LinkedCel));

                    Assert.AreEqual(1, cells.Count);

                    foreach (CelChunk cell in cells)
                    {

                        //Layer..
                        LayerChunk layer = layers[cell.LayerIndex];
                        
                        Assert.AreEqual("Layer 1", layer.LayerName);

                        
                        using (Texture2D texture = aseFile.GetTextureFromCel(cell))
                        {
                            Color[] colors = texture.GetPixels();

                            Assert.AreEqual(1024, colors.Length);
                        }

                    }
                }

            }

        }

        [TestMethod]
        public void GetTexture()
        {

            using (MemoryStream ms = new MemoryStream(Resources.Base))
            {
                AseFile aseFile = new AseFile(ms);

                using (Texture2D texture = aseFile.GetFrame(0))
                {
                    Color[] colors = texture.GetPixels();

                    Assert.AreEqual(1024, colors.Length);
                }

            }

        }
    }
}
