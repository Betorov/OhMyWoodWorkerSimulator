using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Context
{
    public static class ExchangeContext
    {
        public static EState State { get; set; }

        public static float XCoordinate { get; set; }
        public static float YCoordinate { get; set; }
        public static float BrickLength { get; set; }
        public static float BrickWidth { get; set; }

        public static float NewXCoordinate { get; set; }
        public static float NewYCoordinate { get; set; }
        public static float CutWidth { get; set; }
        public static EDirection Direction { get; set; }
        public static float CutStep { get; set; }
        public static float Speed { get; set; }
    }
}
