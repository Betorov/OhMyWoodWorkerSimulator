using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach
{
    interface IScene
    {
        void prepareFigureResource();

        /// <summary>
        /// Отрисовка сцена для этой формы
        /// </summary>
        /// <param name="form"></param>
        void renderSceneFor(RenderForm form);
    }
}
