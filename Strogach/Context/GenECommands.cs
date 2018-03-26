using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Context
{
    class GenECommands : StrogachChannel
    {
        private Exchanger _exchanger;
       GenECommands()
        {
            _exchanger = new Exchanger(this);
        }

        public void GenECommanddsAuto() { }
    }
}
