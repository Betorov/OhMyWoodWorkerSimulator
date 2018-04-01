using System;
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
            try
            {
                _tcpClient =
                    new TcpClient();

                _tcpClient.Connect(address, port);

                _stream =
                    _tcpClient.GetStream();
            }
            catch
            {
                ConnectToServer(address, port);
            }
        }



        /// <summary>
        /// Запись информации в поток.
        /// </summary>
        /// <param name="request">Данные, записываемые в поток в байтовом виде.</param>
        public void Write(byte[] request)
        {
            try
            {
                _stream.Write(
                    request,
                    0,
                    request.Length);
            }
            catch
            {
                Console.WriteLine("Ошибка при передаче данных.");
            }
        }

        /// <summary>
        /// Чтение данных, записанных в поток.
        /// </summary>
        /// <returns>Байтовые данные.</returns>
        public byte[] Read(int answerLength)
        {

            byte[] data = new byte[answerLength];

            try
            {
                _stream.Read(
                   data,
                   0,
                   answerLength);
            }
            catch
            {
                Console.WriteLine("Ошибка при чтении данных.");
            }


            return data;
        }
    }
}
