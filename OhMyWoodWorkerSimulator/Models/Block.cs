using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Models
{
    class Block
    {
        private float width = 0;
        private float length = 0;
        public Block(float kWidth,float kLenth)
        {
            Width = kWidth;
            Length = kLenth;
        }
        /// <summary>
        /// Щирина бруска
        /// </summary>
        public float Width { get => width; set => width = value; }
        /// <summary>
        /// Длина бруска
        /// </summary>
        public float Length { get => length; set => length = value; }
    }
}
