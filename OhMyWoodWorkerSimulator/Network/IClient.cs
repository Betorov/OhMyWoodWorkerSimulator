using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Network
{
    public interface IClient
    {
        bool SendAnswerTo();
        bool SendParametersTo();
        bool SendErrorTo();
    }
}
