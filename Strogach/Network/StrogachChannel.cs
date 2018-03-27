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
    public class StrogachChannel : Channel
    {
        public byte[] Data
        {
            get;
            private set;
        }
        

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
