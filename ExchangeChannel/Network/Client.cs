using System;
using System.Net.Sockets;

namespace ExchangeChannel.Network
{
    /// <summary>
    /// Класс, представляющий собой клиента, взаимодействующего
    /// с остальными клиентами по сети интернет.
    /// </summary>
    public class Client
    {
        //
        // Публичные переменные.
        //

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Связанный с данным клиент, с которым ведётся "общение"
        /// </summary>
        public Client BoundedClient
        {
            get;
            private set;
        }

        /// <summary>
        /// Поток передачи данных в сети.
        /// </summary>
        public NetworkStream Stream
        {
            get;
            private set;
        }

        /// <summary>
        /// Данные о текущем клиенте
        /// </summary>
        public TcpClient NetClient
        {
            get;
            private set;
        }

        //
        // Приватные переменные.
        //

        // Объект сервера для связи с ним.
        private Server _server;

        //
        // Конструкторы.
        //

        public Client(
            TcpClient tcpClient,
            Server server)
        {
            Id = Guid.NewGuid().ToString();
            NetClient = tcpClient;
            _server = server;
            server.AddNewClient(this);
        }

        //
        // Публичные методы.
        //

        public void AppendBound(Client client)
        {
            BoundedClient = client;
        }

        /// <summary>
        /// Обработка пришедшего сообщения и пересылка 
        /// связанному клиенту.
        /// </summary>
        public void ProcessMessage()
        {
            Stream =
                NetClient.GetStream();

            while (true)
            {
                byte[] message = GetMessage();
                Console.WriteLine("Передача сообщения от клиента " + NetClient.Client.LocalEndPoint);
                _server.Send(message, this);
            }

        }

        /// <summary>
        /// Завершение работы клиента.
        /// </summary>
        public void Close()
        {
            if (Stream != null && NetClient != null)
            {
                Stream.Close();
                NetClient.Close();
            }
        }

        //
        // Приватные переменные.
        //

        // Чтение пришедшего пакета данных.
        private byte[] GetMessage()
        {
            byte[] data = new byte[64];

            do
            {
                Stream.Read(
                    data,
                    0,
                    data.Length);
            }
            while (Stream.DataAvailable);

            return data;
        }
    }
}
