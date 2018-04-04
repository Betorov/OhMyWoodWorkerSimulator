using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strogach.Network
{
    public enum EState
    {
        AutoMove,
        ManualMove,
        Stop
    }

    /// <summary>
    /// Перечисление доступных направлений прохода ножа в ручном режиме.
    /// </summary>
    public enum EDirection
    {
        // Вверх.
        Up = 0x8,
        // Вниз.
        Down = 0x2,
        // Вправо.
        Right = 0x4,
        // Влево.
        Left = 0x1
    }
}
