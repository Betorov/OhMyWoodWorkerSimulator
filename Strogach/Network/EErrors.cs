using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Network
{
    /// <summary>
    /// Перечисление возможных ошибок, присылаемых устройством.
    /// </summary>
    public enum EErrors
    {
        // Пакет принят на обработку.
        Ok = 0xE0,
        // Ошибка при автоматическом проходе ножом по бруску.
        AutoError = 0xE1,
        // Ошибка при пошаговом проходе бруска.
        ManualError = 0xE2
    }
}
