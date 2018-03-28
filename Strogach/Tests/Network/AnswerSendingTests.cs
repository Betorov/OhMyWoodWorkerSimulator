using ExchangeChannel.Network;
using NUnit.Framework;
using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Tests.Network
{
    [TestFixture]
    public class AnswerSendingTests
    {
        [Test]
        public void SendCommandTest()
        {
            var exchangeChannel = new StrogachChannel();
            var localAddress = "127.0.0.1";

            Starter.SetTestMode();
            Starter.StartServer();

            var secondClient = new TcpClient();
            secondClient.Connect(IPAddress.Parse(localAddress), 25565);
            NetworkStream secondClientStream = secondClient.GetStream();

            exchangeChannel.ConnectToServer(IPAddress.Parse(localAddress), 25565);
            var exchanger = new Exchanger(exchangeChannel);
            exchanger.SendOk();


            byte[] ok = new byte[1];
            secondClientStream.Read(ok, 0, 1);

            Assert.AreEqual(ok[0], (byte)EErrors.Ok);
        }

    }
}
