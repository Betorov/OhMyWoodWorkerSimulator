using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Models
{
    class Hacksaw
    {
        private float x0 = 0;
        private float y0 = 0;
        private float xEnd = 0;
        private float yEnd = 0;
        private float width = 0;
        private float lengthStep = 0;

        public Hacksaw(int kX0,int kY0,int kXEnd,int kYEnd,int kWidth,int kLegthStep)
        {
            X0 = kX0;
            Y0 = kY0;
            XEnd = kXEnd;
            YEnd = kYEnd;
            Width = kWidth;
            LengthStep = kLegthStep;
        }
        /// <summary>
        /// Начальные координаты рубанка по оси Х
        /// </summary>
        public float X0 { get => x0; set => x0 = value; }
        /// <summary>
        /// Начальные координаты рубанка по оси Y
        /// </summary>
        public float Y0 { get => y0; set => y0 = value; }
        /// <summary>
        /// Конечная координаты рубанка по оси Х
        /// </summary>
        public float XEnd { get => xEnd; set => xEnd = value; }
        /// <summary>
        /// Конечная координаты рубанка по оси Y
        /// </summary>
        public float YEnd { get => yEnd; set => yEnd = value; }
        /// <summary>
        /// Ширина прорези
        /// </summary>
        public float Width { get => width; set => width = value; }
        /// <summary>
        /// Длина шага рубанка
        /// </summary>
        public float LengthStep { get => lengthStep; set => lengthStep = value; }
    }
}
