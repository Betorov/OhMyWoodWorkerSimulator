using SharpDX;
using SharpDX.Windows;
using SharpHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Strogach
{
    class Scene : IDisposable, IScene
    {

        // Нужно понять как отрисовывать.
        //testCoordinates
        private float x = 0, y = 0;
     
        // Color font scenes
        private Color4 color;

        public Scene(DataContext dataContext)
        {
            dataContext._updateCoordinates += updateCoordinateXY;

            prepareFigureResource();
        }

        public Scene()
        {
            prepareFigureResource();
        }

        public void prepareFigureResource()
        {
            color = Color.Gray;

            // Create Figure
        }

        public void renderSceneFor(RenderForm form)
        {
            SharpFPS fpsCounter = new SharpFPS();

            using (SharpDevice device = new SharpDevice(form))
            {
                RenderLoop.Run(form, () =>
                {
                    //resize if form was resized
                    if (device.MustResize)
                    {
                        device.Resize();
                    }

                    //clear color
                    device.Clear(color);

                    //begin drawing text
                    device.Font.Begin();


                    fpsCounter.Update();
                    device.Font.DrawString("FPS: " + fpsCounter.FPS, 0, 0);

                    device.Font.DrawString("Current Time " + DateTime.Now.ToString(), (int)x, (int)y);

                    //flush text to view
                    device.Font.End();

                    //present
                    device.Present();
                });
            }
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        // May be in another connect with dataExchange.
        private void updateCoordinateXY(object sender, EventArgs e)
        {
            var context = (DataContext)sender;

            x = context.X;
            y = context.Y;
        }

    }
}
