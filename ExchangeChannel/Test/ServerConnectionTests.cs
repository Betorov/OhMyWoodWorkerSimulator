using ExchangeChannel.Network;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeChannel.Test
{
    [TestFixture]
    public class ServerConnectionTests
    {
        [Test]
        public void ExchangeDataTest()
        {
            var expected =
                new byte[]
                {
                    0x01,
                    0x02,
                    0x03
                };

            var firstClient = new TcpClient();
            var secondClient = new TcpClient();
            IPAddress localIp = IPAddress.Parse("127.0.0.1");
            int port = 25565;

            Starter.SetTestMode();
            Starter.StartServer();
            firstClient.Connect(localIp, port);
            secondClient.Connect(localIp, port);


            NetworkStream firstClientStream = firstClient.GetStream();
            firstClientStream.Write(expected, 0, expected.Length);

            NetworkStream secondClientStream = secondClient.GetStream();

            var actual = new byte[3];
            do
            {
                secondClientStream.Read(actual, 0, actual.Length);
            }
            while (secondClientStream.DataAvailable);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
