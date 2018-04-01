using ExchangeChannel.Network;
using NUnit.Framework;
using Strogach.Context;
using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Strogach.Tests
{
    [TestFixture]
    public class CoordinatesUpdaterTest
    {
        [Test]
        public void UpdateCoordinatesTest()
        {
            var exchangeChannel = new StrogachChannel();
            var localAddress = "127.0.0.1";

            Starter.SetTestMode();
            Starter.StartServer();

            var secondClient = new TcpClient();
            secondClient.Connect(IPAddress.Parse(localAddress), 25565);
            NetworkStream secondClientStream = secondClient.GetStream();

            exchangeChannel.ConnectToServer(
                IPAddress.Parse(localAddress), 
                25565);

            int i = 0; // ДА ПРОСТЯТ МЕНЯ НА РАБОТЕ ЗА ЭТО (Миша поймёт)
            while(i != 5)
            {
                byte[] request = 
                    GetNewRequest(out float[] parameters);

                secondClientStream.Write(
                    request, 
                    0, 
                    request.Length);

                Thread.Sleep(2000); // Пока нет нотификации об изменении параметров - я хз, как иначе.
                
                Assert.AreEqual(parameters[0], ExchangeContext.XCoordinate);
                Assert.AreEqual(parameters[1], ExchangeContext.YCoordinate);
                Assert.AreEqual(parameters[2], ExchangeContext.NewXCoordinate);
                Assert.AreEqual(parameters[3], ExchangeContext.NewYCoordinate);
                Assert.AreEqual(parameters[4], ExchangeContext.CutWidth);
                Assert.AreEqual(parameters[5], ExchangeContext.Speed);

                i++;
            }
        }

        private byte[] GetNewRequest(out float[] parameters)
        {
            var request = new List<byte>();

            request.Add(0xA);

            Random rand = new Random();
            parameters =
                new float[]
                {
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble(),
                };

            foreach(var param in parameters)
            {
                byte[] val = 
                    BitConverter.GetBytes(param);

                request.AddRange(val);
            }

            return request.ToArray();
        }
    }
}
