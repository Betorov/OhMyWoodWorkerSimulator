using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpHelper;
using Strogach.ObjectFigures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Strogach
{
    //struct used to set shader constant buffer
    struct Camera
    {
        public Matrix world;
        public Matrix worldViewProjection;
        public Vector4 lightDirection;
    }

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
                IModels table = new Table(device);

                fpsCounter.Reset();

                RenderLoop.Run(form, () =>
                {
                    //resize if form was resized
                    if (device.MustResize)
                    {
                        device.Resize();
                    }

                    //clear color
                    device.Clear(color);

                    device.UpdateAllStates();

                    Camera sceneInformation = getCameraScene(form);

                    //Draw Model this Scene.
                    table.drawModel(device, sceneInformation);                    

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

                table.Dispose();
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

        //В модели должна быть часть камеры. Камеры сцены только здесь начальное положение, разобью чутка позже:) Нужно почитать про 3 вида камер. За счет них и идет движуха
        private Camera getCameraScene(RenderForm form)
        {
            //set transformation matrix
            float ratio = (float)form.ClientRectangle.Width / (float)form.ClientRectangle.Height ;
            //Matrix projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1F, 100.0F);
            //Matrix projection = Matrix.PerspectiveFovLH(1, ratio, 2, -1000.576F);
            Matrix projection = Matrix.PerspectiveFovLH(1, ratio, 1, 10000);

            Vector3 from = new Vector3(-35, 125, -150); // Your Eyes
            Vector3 to = new Vector3(0, 70, 0); // ModelView
            //Vector3 from = new Vector3(0, form.ClientRectangle.Height / 2, form.ClientRectangle.Width / 2 );
           // Vector3 to = new Vector3(0, 0, 0);


            Matrix view = Matrix.LookAtLH(from, to, Vector3.UnitY);
            Matrix world = Matrix.RotationZ(MathUtil.DegreesToRadians(0));

            //light direction
            Vector3 lightDirection = new Vector3(0.5f, 0, 1);
            lightDirection.Normalize();

            Camera sceneInformation = new Camera()
            {
                world = world,
                worldViewProjection = world * view * projection,
                lightDirection = new Vector4(lightDirection, 1)
            };

            return sceneInformation;
        }

    }
}
