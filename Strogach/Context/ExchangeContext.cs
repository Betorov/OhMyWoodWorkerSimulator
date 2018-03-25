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
        

        public static float XCoordinate = 0;
        public static float YCoordinate = 0;
        public static float BrickLength = 0;
        public static float BrickWidth = 0;


        public static float newXCoordinate = 0;
        public static float newYCoordinate = 0;
        public static float CutWidth = 0;
        public static EDirection Direction;
        public static float CutStep = 0;
    }
}
