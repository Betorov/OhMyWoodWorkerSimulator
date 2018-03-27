using ExchangeChannel.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeChannel
{
    class Program
    {
        static Server _server;
        static CancellationTokenSource _tokenSource;
        static void Main(string[] args)
        {
            Starter.StartServer();
        }


    }
}


// For client tests
//using ExchangeChannel.Network;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ExchangeChannelTestClient
//{
//    class Program
//    {
//        static string _host = "localhost";
//        static int _port = 8080;
//        static TcpClient _tcpClient;
//        static NetworkStream _stream;
//        static void Main(string[] args)
//        {
//            _tcpClient = new TcpClient(_host, _port);
//            var lol = new TcpClient(_host, _port);

//            _stream = _tcpClient.GetStream();

//            Thread.Sleep(1000);

//            byte[] message = new byte[] { 0x01, 0x1, 0x00 };
//            _stream.Write(message, 0, message.Length);

//            var testStream = lol.GetStream();

//            byte[] result = new byte[3];

//            do
//            {
//                testStream.Read(result, 0, 3);
//            } while (testStream.DataAvailable);

//            foreach (var kek in result)
//            {
//                Console.Write(kek + ": ");
//            }

//            message = new byte[] { 0x01, 0x1, 0x00 };
//            testStream.Write(message, 0, message.Length);

//            result = new byte[3];

//            do
//            {
//                _stream.Read(result, 0, 3);
//            } while (testStream.DataAvailable);

//            foreach (var kek in result)
//            {
//                Console.Write(kek + ": ");
//            }

//            Console.Read();
//        }
//    }
//}
