using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Context
{
    class GenECommands : StrogachChannel
    {
        public GenECommands()
        {
            var exchangeChannel = new StrogachChannel();

            exchangeChannel.ConnectToServer(IPAddress.Parse("89.179.187.119"), 25565);
            MyExchanger = new Exchanger(exchangeChannel);
            MyExchanger.SendOk();
        }

        public Exchanger MyExchanger { get; private set; }
    }


}
