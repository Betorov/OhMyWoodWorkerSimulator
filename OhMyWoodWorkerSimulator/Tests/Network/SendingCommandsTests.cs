using ExchangeChannel.Network;
using NUnit.Framework;
using OhMyWoodWorkerSimulator.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Tests.Network
{
    [TestFixture]
    public class SendingCommandsTests
    {
        [Test]
        public void SendCommandTest()
        {
            var exchangeChannel = new Channel();
            var localAddress = "127.0.0.1";

            Starter.SetTestMode();
            Starter.StartServer();

            exchangeChannel.ConnectToServer(IPAddress.Parse(localAddress), 25565);
            var exchanger = new Exchanger(exchangeChannel);

            var secondClient = new TcpClient();
            secondClient.Connect(IPAddress.Parse(localAddress), 25565);
            NetworkStream secondClientStream = secondClient.GetStream();

            secondClientStream.Write(new[] { (byte)EErrors.Ok }, 0, 1);
            exchanger.SendHandshakeRequestAsync();
            do
            {
                secondClientStream.ReadByte();
            }
            while (secondClientStream.DataAvailable);

            
        }
    }
}
