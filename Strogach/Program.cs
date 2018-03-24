using SharpDX.Windows;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Strogach
{
    class Program
    {
       
        static void Main(string[] args)
        {
            // ExchangeContext to exchange with remote Control.
            DataContext data = new DataContext();

            Scene scene = new Scene(data);

            // Task to visualise scene strogach
            Task task = new Task(new Action(() => 
            {
                scene.renderSceneFor(initVizualizeForm());
            }));
            
            
            task.Start();

            float x = 1, y = 1;
            while (true)
            {
                Thread.Sleep(100);
                Console.WriteLine((x++).ToString() + (y++).ToString());
                data.setXAndY(x++, y++);
            }
        }

        private static RenderForm initVizualizeForm()
        {
            // Сцена должна иметь в себе deviceD11 для изменения возможно даже наследование
            if (!SharpDevice.IsDirectX11Supported())
                return null;

            RenderForm form = new RenderForm();

            //Set title application;
            form.Text = "TestApplication";
            form.IsFullscreen = true;

            //Set events
            form.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                    Application.Exit();
            };

            return form;
        }


    }
}
