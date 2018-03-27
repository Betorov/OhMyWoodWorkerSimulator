using System.Net;
using System.Net.Sockets;

namespace ExchangeChannel.Network
{
    /// <summary>
    /// Класс для передачи данных между пультом и строгальным станком.
    /// </summary>
    public class Channel
    {
        //
        // Публичные переменные.
        //

        /// <summary>
        /// Считываемые данные.
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }

        //
        // Защищённые перменные.
        //

        /// <summary>
        /// Поток, по которому будет производиться передача данных.
        /// </summary>
        protected NetworkStream _stream;

        //
        // Приватные переменные.
        //

        /// <summary>
        /// Данные о подключённом клиенте.
        /// </summary>
        private TcpClient _tcpClient;

        //
        // Конструкторы.
        //

        public Channel()
        {
            _tcpClient = new TcpClient();
        }


        /// <summary>
        /// Подключение к серверу - ретранслятору, 
        /// через который будет происходить обмен данными.
        /// </summary>
        /// <param name="address">IP-адрес сервера.</param>
        /// <param name="port">Порт сервера.</param>
        public virtual void ConnectToServer(
            IPAddress address,
            int port)
        {
            IPEndPoint endPoint =
                new IPEndPoint(
                    address,
                    port);

            _tcpClient =
                new TcpClient(
                    endPoint);

            _stream =
                _tcpClient.GetStream();
        }

        /// <summary>
        /// Запись информации в поток.
        /// </summary>
        /// <param name="request">Данные, записываемые в поток в байтовом виде.</param>
        public void Write(byte[] request)
        {
            _stream.Write(
                request,
                0,
                request.Length);
        }

        /// <summary>
        /// Чтение данных, записанных в поток.
        /// </summary>
        /// <returns>Байтовые данные.</returns>
        public byte[] Read()
        {
            byte[] data = new byte[64];

            do
            {
                _stream.Read(
                    data,
                    0,
                    data.Length);
            }
            while (_stream.DataAvailable);

            return data;
        }
    }
}
