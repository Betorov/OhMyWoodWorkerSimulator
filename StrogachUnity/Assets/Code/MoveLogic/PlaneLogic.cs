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
            ExchangeContext.YCoordinate = vectorPlaneNow.y;

            _exchanger.SendParams(vectorPlaneNow.x, vectorPlaneNow.z, brickLength, brickWidht);        
        }

        //TODO: Logic Moving Plane
        public Vector3 nextPointPlane(Vector3 vectorPlaneNow)
        {
            if (!ExchangeContext.hasRunning)
                return vectorPlaneNow;

            if (ExchangeContext.hasManualRunning)
                return nextManualPoint(vectorPlaneNow);

            if (ExchangeContext.hasAutoRunning)
                return nextAutoPoint(vectorPlaneNow);

            //Debug.Log(ExchangeContext.BrickLength);
           // Debug.Log(ExchangeContext.Speed);

           

            //_exchanger.SendAutoError();
            // Update Изменение положения ножа на каждый фрейм
            return new Vector3(1, 1, 1);
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

        private Vector3 nextManualPoint(Vector3 vector)
        {
            return new Vector3(0,0,0);
        }

        private Vector3 nextAutoPoint(Vector3 vector)
        {
            return new Vector3(0, 0, 0);
        }

    }
}
