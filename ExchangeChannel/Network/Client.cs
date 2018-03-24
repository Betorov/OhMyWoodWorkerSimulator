using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        //
        // Приватные переменные.
        //

        // Данные о текущем клиенте.
        private TcpClient _tcpClient;

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
            _tcpClient = tcpClient;
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
                _tcpClient.GetStream();

            while(true)
            {
                byte[] message = GetMessage();
                _server.Send(message, this);
            }
            
        }

        /// <summary>
        /// Завершение работы клиента.
        /// </summary>
        public void Close()
        {
            if(Stream != null && _tcpClient != null)
            {
                Stream.Close();
                _tcpClient.Close();
            }
        }

        //
        // Приватные переменные.
        //

        // Чтение пришедшего пакета данных.
        private byte[] GetMessage()
        {
            byte[] data = new byte[1024];

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
