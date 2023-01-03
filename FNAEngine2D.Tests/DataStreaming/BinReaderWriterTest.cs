using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests.DataStreaming
{
    /// <summary>
    /// Tests for BinReader and BinWriter
    /// </summary>
    [TestClass]
    public class BinReaderWriterTest
    {
        private byte[] _buffer = new byte[4096];

        [TestMethod]
        public void BooleanTest()
        {
            new BinWriter(_buffer).Write(true);
            Assert.AreEqual(true, new BinReader(_buffer).ReadBoolean());

            new BinWriter(_buffer).Write(false);
            Assert.AreEqual(false, new BinReader(_buffer).ReadBoolean());
        }

        [TestMethod]
        public void ByteTest()
        {
            new BinWriter(_buffer).Write((byte)128);
            Assert.AreEqual((byte)128, new BinReader(_buffer).ReadByte());
        }

        [TestMethod]
        public void ByteArrayTest()
        {
            new BinWriter(_buffer).Write(System.Text.Encoding.UTF8.GetBytes("testit"));
            Assert.AreEqual("testit", System.Text.Encoding.UTF8.GetString(new BinReader(_buffer).ReadBytes(6)));
        }

        [TestMethod]
        public void Int16Test()
        {
            new BinWriter(_buffer).Write((Int16)128);
            Assert.AreEqual((Int16)128, new BinReader(_buffer).ReadInt16());

            new BinWriter(_buffer).Write((Int16)(-5465));
            Assert.AreEqual((Int16)(-5465), new BinReader(_buffer).ReadInt16());
        }

        [TestMethod]
        public void Int32Test()
        {
            new BinWriter(_buffer).Write((Int32)128);
            Assert.AreEqual((Int32)128, new BinReader(_buffer).ReadInt32());

            new BinWriter(_buffer).Write((Int32)(-5465));
            Assert.AreEqual((Int32)(-5465), new BinReader(_buffer).ReadInt32());
        }

        [TestMethod]
        public void Int64Test()
        {
            new BinWriter(_buffer).Write((Int64)128);
            Assert.AreEqual((Int64)128, new BinReader(_buffer).ReadInt64());

            new BinWriter(_buffer).Write((Int64)(-5465));
            Assert.AreEqual((Int64)(-5465), new BinReader(_buffer).ReadInt64());
        }

        [TestMethod]
        public void CharTest()
        {
            new BinWriter(_buffer).Write('c');
            Assert.AreEqual('c', new BinReader(_buffer).ReadChar());
        }

        [TestMethod]
        public void CharArrayTest()
        {
            new BinWriter(_buffer).Write(System.Text.Encoding.UTF8.GetChars(System.Text.Encoding.UTF8.GetBytes("testit")));
            Assert.AreEqual("testit", new String(new BinReader(_buffer).ReadChars()));
        }

        [TestMethod]
        public void StringTest()
        {
            new BinWriter(_buffer).Write("This is a test");
            Assert.AreEqual("This is a test", new BinReader(_buffer).ReadString());
        }

        [TestMethod]
        public void DateTimeTest()
        {
            new BinWriter(_buffer).Write(new DateTime(1900, 1, 12, 15, 33, 24));
            Assert.AreEqual(new DateTime(1900, 1, 12, 15, 33, 24), new BinReader(_buffer).ReadDateTime());
        }

        [TestMethod]
        public void DecimalTest()
        {
            new BinWriter(_buffer).Write(2316.1222M);
            Assert.AreEqual(2316.1222M, new BinReader(_buffer).ReadDecimal());
        }

        [TestMethod]
        public void DoubleTest()
        {
            new BinWriter(_buffer).Write(2316.1222D);
            Assert.AreEqual(2316.1222D, new BinReader(_buffer).ReadDouble());
        }

        [TestMethod]
        public void SingleTest()
        {
            new BinWriter(_buffer).Write(2316.122f);
            Assert.AreEqual(2316.122f, new BinReader(_buffer).ReadSingle());
        }

        [TestMethod]
        public void GuidTest()
        {
            Guid guid = Guid.NewGuid();
            new BinWriter(_buffer).Write(guid);
            Assert.AreEqual(guid, new BinReader(_buffer).ReadGuid());
        }

        [TestMethod]
        public void Vector2Test()
        {
            Vector2 vector = new Vector2(123.2f, 333.1f);

            new BinWriter(_buffer).Write(vector);
            Assert.AreEqual(vector, new BinReader(_buffer).ReadVector2());
        }

        [TestMethod]
        public void RectangleTest()
        {
            Rectangle rectangle = new Rectangle(-22, 22, 111, 3444);

            new BinWriter(_buffer).Write(rectangle);
            Assert.AreEqual(rectangle, new BinReader(_buffer).ReadRectangle());
        }

        [TestMethod]
        public void PointTest()
        {
            Point point = new Point(123, 333);

            new BinWriter(_buffer).Write(point);
            Assert.AreEqual(point, new BinReader(_buffer).ReadPoint());
        }

        [TestMethod]
        public void ObjectTest()
        {
            TestBinObj obj = new TestBinObj()
            {
                Data = "test it"
            };

            BinWriter writer = new BinWriter(_buffer);
            writer.WriteObject(obj);
            writer.Write(1233);

            BinReader reader = new BinReader(_buffer);
            TestBinObj obj2 = reader.ReadObject<TestBinObj>();

            Assert.AreEqual("test it", obj2.Data);
            Assert.AreEqual(1233, reader.ReadInt32());
        }



        private class TestBinObj
        {
            public string Data = "test";
        }
    }
}
