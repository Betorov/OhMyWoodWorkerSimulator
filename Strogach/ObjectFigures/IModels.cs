using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.ObjectFigures
{
    interface IModels : IDisposable
    {
        /// <summary>
        /// Отрисовка модели
        /// </summary>
        void drawModel(SharpDevice device, Camera camera);

        /// <summary>
        /// Подготовка текстур, сплайнов, шейдеров
        /// </summary>
        void prepareModel();
    }
}
