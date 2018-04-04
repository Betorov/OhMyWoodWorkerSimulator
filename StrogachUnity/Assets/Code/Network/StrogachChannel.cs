using Assets.Code;
using ExchangeChannel.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

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


        public StrogachChannel()
        {
            
        }

        /// <summary>
        /// Данные, которые приходят от пульта управления.
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }

        //
        // Публичные методы.
        //
        
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

            Thread thread = new Thread(RunListenerTask);

            thread.Start();
        }

        //
        // Приватные методы.
        //

        // Слушатель входящих от пульта сообщений.
        private void RunListenerTask()
        {
            while (true)
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
}
