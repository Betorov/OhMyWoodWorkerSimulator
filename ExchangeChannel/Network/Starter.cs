using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeChannel.Network
{
    public static class Starter
    {
        static bool _isTestMode = false;

        static Server _server;
        static CancellationTokenSource _tokenSource;

        public static void StartServer()
        {
            _tokenSource =
                new CancellationTokenSource();

            _server =
                new Server(_tokenSource);

            Task task =
                new Task(
                    new Action(
                        async () =>
                            await _server.RunServerListenerAsync(25565)));

            task.Start();

            if(!_isTestMode)
            {
                Console.ReadLine();
            }
        }
        public static void SetTestMode()
        {
            _isTestMode = true;
        }
    }

 
}
