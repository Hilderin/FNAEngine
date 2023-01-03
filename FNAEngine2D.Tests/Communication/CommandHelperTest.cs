using FNAEngine2D.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests.Communication
{
    [TestClass]
    public class CommandHelperTest
    {
        [TestMethod]
        public void SerializeDeserializeTest()
        {
            byte[] buffer = new byte[4096];

            TestCommand cmd = new TestCommand()
            {
                Data = "test"
            };

            int len = CommandHelper.Serialize(cmd, buffer, 0);
            Assert.AreEqual(10, len);

            object cmdOut = CommandHelper.Deserialize(buffer, 0, len);

            Assert.IsInstanceOfType(cmdOut, typeof(TestCommand));
            Assert.AreEqual("test", ((TestCommand)cmdOut).Data);

        }

        [TestMethod]
        public void SerializeDeserializeWithAOffsetTest()
        {
            byte[] buffer = new byte[4096];

            TestCommand cmd = new TestCommand()
            {
                Data = "Sent by server"
            };

            int len = CommandHelper.Serialize(cmd, buffer, 10);
            Assert.AreEqual(30, len);

            object cmdOut = CommandHelper.Deserialize(buffer, 10, len);

            Assert.IsInstanceOfType(cmdOut, typeof(TestCommand));
            Assert.AreEqual("Sent by server", ((TestCommand)cmdOut).Data);

        }


    }

    public class TestCommand: ICommand
    {
        public string Data { get; set; }

        /// <summary>
        /// Serialize
        /// </summary>
        public void Serialize(BinWriter writer)
        {
            writer.Write(Data);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public void Deserialize(BinReader reader)
        {
            this.Data = reader.ReadString();
        }
    }
}
