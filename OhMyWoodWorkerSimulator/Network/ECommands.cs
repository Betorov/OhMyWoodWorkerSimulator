using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Network
{
    public enum ECommands
    {
        Handshake = 0x0,
        BrickParameters = 0xF,
        Auto = 0xA,
        Manual = 0xB,
        Stop = 0xC
    }
}
