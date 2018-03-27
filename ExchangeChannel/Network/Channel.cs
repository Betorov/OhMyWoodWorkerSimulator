using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeChannel.Network
{
    public class Channel
    {
        protected NetworkStream _stream;

        private TcpClient _tcpClient;

        public byte[] Data
        {
            get;
            private set;
        }

        public Channel()
        {
            _tcpClient = new TcpClient();
        }

        public virtual void ConnectToServer(
            IPAddress address,
            int port)
        {
            IPEndPoint endPoint =
                new IPEndPoint(
                    address,
                    port);

            _tcpClient =
                new TcpClient(endPoint);

            _stream = 
                _tcpClient.GetStream();
        }

        public void Write(byte[] request)
        {
            _stream.Write(
                request,
                0,
                request.Length);
        }

        public byte[] Read()
        {
            MemoryStream response = new MemoryStream();

            _stream.CopyTo(response);

            var answer =
                new byte[response.Position];

            return answer;
        }
    }
}
