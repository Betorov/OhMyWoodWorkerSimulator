using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code
{
    public class ContextExchanger
    {
        public EventHandler eventHandler;

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }


        public float Direction { get; private set; }
        public float CutStep { get; private set; }
        public float CutWidth { get; private set; }

        public void setDCC(float x, float y, float z)
        {
            Direction = x;
            CutStep = y;
            CutWidth = z;


            Invoke();
        }

        public void Invoke()
        {
            if (eventHandler != null)
                eventHandler.Invoke(this, EventArgs.Empty);
        }
    }
}
