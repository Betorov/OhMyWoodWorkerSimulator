using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Models
{
    class Hacksaw
    {
        private float _x0 = 0;
        private float _y0 = 0;
        private float _xCurrent = 0;
        private float _yCurrent = 0;
        private float _xEnd = 0;
        private float _yEnd = 0;
        private float _width = 0;
        private float _lengthStep = 0;

        public Hacksaw(float kX0, float kY0, float kXEnd, float kYEnd, float kWidth, float kLegthStep)
        {
            X0 = kX0;
            Y0 = kY0;
            XEnd = kXEnd;
            YEnd = kYEnd;
            Width = kWidth;
            LengthStep = kLegthStep;
        }

        public Hacksaw()
        {
            X0 = 0;
            Y0 = 0;
            XEnd = 0;
            YEnd = 0;
            XCurrent = 0;
            YCurrent = 0;
            Width = 0;
            LengthStep = 0;
        }

        public override string ToString()
        {
            return X0.ToString() + " " + Y0.ToString() + " " + XEnd.ToString() + " " + YEnd.ToString() + " " + Width.ToString() + " " + LengthStep.ToString();
        }

        /// <summary>
        /// Начальные координаты рубанка по оси Х
        /// </summary>
        public float X0 { get => _x0; set => _x0 = value; }
        /// <summary>
        /// Начальные координаты рубанка по оси Y
        /// </summary>
        public float Y0 { get => _y0; set => _y0 = value; }
        /// <summary>
        /// Конечная координаты рубанка по оси Х
        /// </summary>
        public float XEnd { get => _xEnd; set => _xEnd = value; }
        /// <summary>
        /// Конечная координаты рубанка по оси Y
        /// </summary>
        public float YEnd { get => _yEnd; set => _yEnd = value; }
        /// <summary>
        /// Ширина прорези
        /// </summary>
        public float Width { get => _width; set => _width = value; }
        /// <summary>
        /// Длина шага рубанка
        /// </summary>
        public float LengthStep { get => _lengthStep; set => _lengthStep = value; }
        /// <summary>
        /// Текущая координаты рубанка по оси X
        /// </summary>
        public float XCurrent { get => _xCurrent; set => _xCurrent = value; }
        /// <summary>
        /// Текущая координаты рубанка по оси Y
        /// </summary>
        public float YCurrent { get => _yCurrent; set => _yCurrent = value; }
    }
}
