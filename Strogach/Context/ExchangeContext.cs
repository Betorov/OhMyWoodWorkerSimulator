using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Context
{
    public class ExchangeContext
    {
        public float XCoordinate = 2;
        public float YCoordinate = 5;
        public float BrickLength = 10;
        public float BrickWidth = 10;

        public float newXCoordinate = 4;
        public float newYCoordinate = 4;
        public float CutWidth = 1;
        public EDirection Direction;
        public float CutStep = 5;
        public ExchangeContext()
        {

        }

        //Auto or Manual or Stop
        public int State(int _state)
        {
            return _state;
        }

        //param[]
        public float[] GetCoordinatesFromData(float[] _param)
        {
            _param = new float[5];
            return _param;
        }


    }
}
