using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Network
{
    /// <summary>
    /// Перечисление доступных пульту управления команд для взаимодействия со строгальным станком.
    /// </summary>
    public enum ECommands
    {
        // Команда "рукопожатия" с устройством.
        Handshake = 0x0,
        // Команда извлечения параметров бруска.
        BrickParameters = 0xF,
        // Команда автоматического прохода ножа по бруску.
        Auto = 0xA,
        // Команда ручного прохода ножа по бруску.
        Manual = 0xB,
        // Команда остановки автоматического прохода по бруску.
        Stop = 0xC
    }
}
