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
            _tcpClient =
                new TcpClient();

            _tcpClient.Connect(address, port);

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
        public byte[] Read(int answerLength)
        {

            byte[] data = new byte[answerLength];

             _stream.Read(
                data,
                0,
                answerLength);

            return data;
        }
    }
}
