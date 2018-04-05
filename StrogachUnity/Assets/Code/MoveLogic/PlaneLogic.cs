using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace Assets.Code.MoveLogic
{
    class PlaneLogic
    {
        // Канал для отправки 
        private Exchanger _exchanger = null;

        private Vector3 _vectorPlaneNow;

        public PlaneLogic(Vector3 vectorPlaneNow, float brickWidht, float brickLength)
        {
            var exchangeChannel = new StrogachChannel();
            //TODO: Как устанавливать адресс сервера
            var address = "127.0.0.1";

            exchangeChannel.ConnectToServer(IPAddress.Parse(address), 25565);
            _exchanger = new Exchanger(exchangeChannel);
            _exchanger.SendOk();

            //TODO: Убрать этот статик епаный!
            ExchangeContext.BrickLength = brickLength;
            ExchangeContext.BrickWidth = brickWidht;
            ExchangeContext.XCoordinate = vectorPlaneNow.x;
            ExchangeContext.YCoordinate = vectorPlaneNow.z;

            _exchanger.SendParams(vectorPlaneNow.x, vectorPlaneNow.z, brickLength, brickWidht);        
        }

        public void nextPointPlaneFor(GameObject plane)
        {
            var vectOrig = plane.transform.position;
            Vector3 vector = vectOrig;

            if (ExchangeContext.hasManualRunning)
                vector = nextManualPoint(vectOrig, plane);


            plane.transform.position = vector;
        }

        //TODO: Logic Moving Plane
        public Vector3 nextPointPlane(Vector3 vectorPlaneNow, GameObject plane, GameObject wood)
        {
            ExchangeContext.XCoordinate = vectorPlaneNow.x;
            ExchangeContext.YCoordinate = vectorPlaneNow.z;

            if (!ExchangeContext.hasRunning)
                return vectorPlaneNow;

            if (ExchangeContext.hasManualRunning)
                return nextManualPoint(vectorPlaneNow, plane);

            if (ExchangeContext.hasAutoRunning)
                return nextAutoPoint(vectorPlaneNow, plane, wood);

            //Debug.Log(ExchangeContext.BrickLength);
           // Debug.Log(ExchangeContext.Speed);

           

            //_exchanger.SendAutoError();
            // Update Изменение положения ножа на каждый фрейм
            return vectorPlaneNow;
        }

        /// <summary>
        /// Скорость ножа
        /// </summary>
        public float Speed
        {
            get
            {
                return ExchangeContext.Speed;
            }
        }

        private Vector3 nextManualPoint(Vector3 vector, GameObject plane)
        {
            Vector3 vectorNew = vector;

            //Изменение размера ножа
            if(ExchangeContext.CutWidth != 0)
            {
                var localScale = plane.transform.localScale;

                localScale.x = ExchangeContext.CutWidth;

                plane.transform.localScale = localScale;

                ExchangeContext.CutWidth = 0;
            }

            if (ExchangeContext.Direction == EDirection.Down)
            {
                vectorNew = new Vector3(vector.x, (vector.y - ExchangeContext.CutStep), vector.z);

                if ((vectorNew.y) != (vector.y - ExchangeContext.CutStep))
                    Debug.Log("Error" + vectorNew.y + " " + vector.y);
                ExchangeContext.CutStep = 0;
            }

            if (ExchangeContext.Direction == EDirection.Up)
            {
                vectorNew = new Vector3(vector.x, vector.y ,(vector.z + ExchangeContext.CutStep));
                ExchangeContext.CutStep = 0;
            }

            if (ExchangeContext.Direction == EDirection.Left)
            {
                vectorNew = new Vector3(vector.x - ExchangeContext.CutStep, vector.y, vector.z);
                ExchangeContext.CutStep = 0;
            }

            if (ExchangeContext.Direction == EDirection.Right)
            {
                vectorNew = new Vector3(vector.x + ExchangeContext.CutStep, vector.y, vector.z);
                ExchangeContext.CutStep = 0;
            }

            return vectorNew;
        }

        private Vector3 nextAutoPoint(Vector3 vector, GameObject plane, GameObject wood)
        {
            if (ExchangeContext.CutWidth != 0)
            {
                vector.y = vector.y - ExchangeContext.CutWidth;
                ExchangeContext.CutWidth = 0;
            }

            Vector3 newVector = new Vector3(ExchangeContext.NewXCoordinate, vector.y, ExchangeContext.NewYCoordinate);

            return newVector;
        }

    }
}
