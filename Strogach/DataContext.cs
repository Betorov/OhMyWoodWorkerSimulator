using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach
{
    class DataContext
    {
        public event EventHandler _updateCoordinates;

        public float X { get; private set; }
        public float Y { get; private set; }

        public void setXAndY(float X, float Y)
        {
            this.X = X;
            this.Y = Y;

            if (_updateCoordinates != null)
                _updateCoordinates.Invoke(this, EventArgs.Empty);
            //Invoke event
        }

        public void write()
        {
            Console.WriteLine("DaDaDaDaDaDaDaDaDaDa");
        }
    }
}
