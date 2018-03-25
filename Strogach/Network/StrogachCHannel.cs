﻿using ExchangeChannel.Network;
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
        public StrogachChannel()
        {
            //Client
        }

        public byte[] Data
        {
            get;
            private set;
        }

        public int ChannelPriority => throw new NotImplementedException();

        public string ChannelName => throw new NotImplementedException();

        public void RunListenerTask()
        {
            MemoryStream requestStream = new MemoryStream();

            while(true)
            {
                do
                {
                    _stream.CopyTo(requestStream);
                }
                while (_stream.DataAvailable);

                Data = new byte[requestStream.Position];

                requestStream.Read(Data, 0, Data.Length);

                var exchanger = new Exchanger(this);
                exchanger.React(Data);
            }
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

        public string Parse(string url, out string objectURI)
        {
            throw new NotImplementedException();
        }
    }
}
