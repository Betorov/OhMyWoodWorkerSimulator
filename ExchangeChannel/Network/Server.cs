﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeChannel.Network
{
    /// <summary>
    /// Класс для прослушивания входящих сообщений и передаче их между клиентами
    /// для взаимодействия.
    /// </summary>
    public class Server
    {
        //
        // Приватные переменные.
        //

        // Показывает, когда надо закончить поток прослушивания 
        // входящих сообщений.
        private CancellationTokenSource _cancelationTokenSource;

        // Слушатель сервера. Ведёт наблюдение за входящими
        // сообщениями.
        private static TcpListener _listener;

        // Список подключённых клиентов.
        private List<Client> _clients = new List<Client>();

        //
        // Конструкторы.
        //

        public Server(
            CancellationTokenSource cancellationTokenSource)
        {
            _cancelationTokenSource = cancellationTokenSource;
        }

        //
        // Публичные методы.
        //

        /// <summary>
        /// Запускает поток, в котором прослушиваются входящие сообщения.
        /// </summary>
        /// <param name="port">Порт сервера, на котором запускается поток.</param>
        public async Task RunServerListenerAsync(int port)
        {
            CancellationToken cancellationToken =
                _cancelationTokenSource.Token;

            _listener =
                new TcpListener(
                    IPAddress.Any, port);

            _listener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                Client firstClient =
                    await GetNewConnectedClient();
                AddNewClient(firstClient);

                Client secondClient =
                    await GetNewConnectedClient();
                AddNewClient(secondClient);

                AppendNewBound(
                    firstClient,
                    secondClient);

                Task firstClientProcess =
                    new Task(
                        new Action(
                            () =>
                            {
                                firstClient.ProcessMessage();
                            }));

                Task secondClientProcess =
                    new Task(
                        new Action(
                            () =>
                            {
                                secondClient.ProcessMessage();
                            }));

                secondClientProcess.Start();
                firstClientProcess.Start();
            }

            Disconnect();
        }

        public void Send(
            byte[] message,
            Client sender)
        {
            Client receiver = sender.BoundedClient;

            receiver.Stream.Write(
                message,
                0,
                3);

        }

        /// <summary>
        /// Добавлеяет нового клиента для обработки сервером.
        /// </summary>
        /// <param name="client">Добавляемый клиент.</param>
        public void AddNewClient(Client client)
        {
            _clients.Add(client);
        }

        /// <summary>
        /// Добавление связи между двумя клиентами.
        /// </summary>
        /// <param name="remote">Пульт управления</param>
        /// <param name="plane">Станок</param>
        public void AppendNewBound(Client remote, Client plane)
        {
            remote.AppendBound(plane);
            plane.AppendBound(remote);
        }

        /// <summary>
        /// Удаляет клиента из обработки сервером.
        /// </summary>
        /// <param name="id">Идентификационный номер клиента.</param>
        public void RemoveConnection(string id)
        {
            Client client =
                _clients.FirstOrDefault(
                    c => c.Id == id);

            if (!(client is null))
                _clients.Remove(client);
        }

        /// <summary>
        /// Останавливает прослушивание сервером входящих сообщений.
        /// </summary>
        public void StopServerListener()
        {
            _cancelationTokenSource.Cancel();
        }

        //
        // Приватные методы.
        //


        // Завершение работы сервера и отключение всех клиентов.
        private void Disconnect()
        {
            _listener.Stop();

            foreach (var client in _clients)
                client.Close();
        }

        private async Task<Client> GetNewConnectedClient()
        {
            TcpClient tcpClient =
                await _listener.AcceptTcpClientAsync();

            Client client =
                new Client(tcpClient, this);

            return client;
        }
    }
}