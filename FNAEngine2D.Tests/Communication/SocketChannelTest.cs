using FNAEngine2D.Communication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Tests.Communication
{
    [TestClass]
    public class SocketChannelTest
    {
        /// <summary>
        /// Create a client/server connexion
        /// </summary>
        private void ConnectClientServer(out SocketChannel clientChannel, out SocketChannel serverChannel, out SocketServer server)
        {
            int port = (new Random()).Next(5000, 6000);

            server = new SocketServer();

            server.Start(true, port);

            Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            client.Connect(new IPEndPoint(IPAddress.Loopback, port));
            clientChannel = new SocketChannel(client);

            Assert.IsTrue(client.Connected);

            for (int cpt = 0; cpt < 10; cpt++)
            {
                if (server.NewChannels.Count > 0)
                    break;

                System.Threading.Thread.Sleep(100);
            }

            Assert.AreEqual(1, server.NewChannels.Count);

            server.NewChannels.TryDequeue(out serverChannel);
        }

        [TestMethod]
        public void SocketServerAcceptConnexionTest()
        {
            int port = (new Random()).Next(5000, 6000);

            SocketServer server = new SocketServer();

            server.Start(true, port);

            Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            client.Connect(new IPEndPoint(IPAddress.Loopback, port));

            Assert.IsTrue(client.Connected);

            for (int cpt = 0; cpt < 10; cpt++)
            {
                if (server.NewChannels.Count > 0)
                    break;

                System.Threading.Thread.Sleep(100);
            }

            Assert.AreEqual(1, server.NewChannels.Count);

            server.Stop();


        }


        [TestMethod]
        public void SocketServerSendReceiveTest()
        {

            SocketChannel clientChannel;
            SocketChannel serverChannel;
            SocketServer server;
            ConnectClientServer(out clientChannel, out serverChannel, out server);


            serverChannel.Send(new TestCommandSocket() { Data = "Sent by server" });

            object received = null;
            for (int cpt = 0; cpt < 10; cpt++)
            {
                received = clientChannel.ReadObject();
                if (received != null)
                    break;

                System.Threading.Thread.Sleep(100);
            }

            Assert.IsNotNull(received);
            Assert.IsInstanceOfType(received, typeof(TestCommandSocket));
            Assert.AreEqual("Sent by server", ((TestCommandSocket)received).Data);

            server.Stop();


        }

        [TestMethod]
        public void SocketServerMultipleSendReceiveTest()
        {

            SocketChannel clientChannel;
            SocketChannel serverChannel;
            SocketServer server;
            ConnectClientServer(out clientChannel, out serverChannel, out server);

            for(int cpt = 0; cpt < 10; cpt++)
                serverChannel.Send(new TestCommandSocket() { Data = "Sent by server " + cpt });

            List<TestCommandSocket> listReceived = new List<TestCommandSocket>();

            while(listReceived.Count != 10)
            {
                listReceived.Add(clientChannel.WaitNext<TestCommandSocket>());
            }

            Assert.AreEqual(10, listReceived.Count);

            for (int cpt = 0; cpt < 10; cpt++)
                Assert.AreEqual("Sent by server " + cpt, listReceived[cpt].Data);

            server.Stop();


        }


    }

    [Command(456)]
    public class TestCommandSocket
    {
        public string Data { get; set; }
    }
}
