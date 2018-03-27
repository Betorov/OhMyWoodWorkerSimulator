using ExchangeChannel.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Network
{
    /// <summary>
    /// Канал для обмена данными между пультом и строгальным станком.
    /// </summary>
    public class StrogachChannel : Channel
    {
        //
        // Публичные переменные.
        //

        /// <summary>
        /// Данные, которые приходят от пульта управления.
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }
<<<<<<< HEAD:Strogach/Network/StrogachCHannel.cs

        //
        // Публичные методы.
        //
=======
        
>>>>>>> 13413f1bf3c040c4edca1a35f4f4e74ce9293d89:Strogach/Network/StrogachChannel.cs

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        /// <param name="address">IP-адрес сервера.</param>
        /// <param name="port">Порт сервера.</param>
        public override void ConnectToServer(
            IPAddress address,
            int port)
        {
            base.ConnectToServer(address, port);

            Task listenerRunner =
                new Task(
                    new Action(
                        () => RunListenerTask()));
        }

<<<<<<< HEAD:Strogach/Network/StrogachCHannel.cs
        //
        // Приватные методы.
        //

        // Слушатель входящих от пульта сообщений.
=======
>>>>>>> 13413f1bf3c040c4edca1a35f4f4e74ce9293d89:Strogach/Network/StrogachChannel.cs
        private void RunListenerTask()
        {
            Data = new byte[64];

            do
            {
                _stream.Read(
                    Data,
                    0,
                    Data.Length);
            }
            while (_stream.DataAvailable);

            var exchanger = new Exchanger(this);
            exchanger.React(Data);
        }
    }
}
