using ExchangeChannel.Network;
using OhMyWoodWorkerSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Network
{
    /// <summary>
    /// Основной класс для общения.
    /// </summary>
    public class Exchanger
    {
        //
        // Приватные переменные.
        //

        // Канал для обмена данными со строгальным станком.
        private Channel _exchangeChannel;

        //
        // Конструкторы.
        //

        public Exchanger(Channel exchangeChannel)
        {
            _exchangeChannel = exchangeChannel;
        }

        //
        // Публичные методы.
        //

        /// <summary>
        /// Послать запрос на "рукопожатие".
        /// </summary>
        public void SendHandshakeRequestAsync()
        {
            Frame frame = new Frame();
            byte request = frame.GetHandshakeRequest();

            _exchangeChannel.Write(new[] { request });

            byte[] answer =
                _exchangeChannel.Read(1);

            frame.ValidateAnswerAndFillSelf(answer);
        }

        /// <summary>
        /// Запрашивает параметры бруска, с которыми в дальнейшем будет работать пульт управления.
        /// </summary>
        public Brick GetBrickParams()
        {
            Frame frame = new Frame();
            byte request = frame.GetBrickParamsRequest();
            _exchangeChannel.Write(new[] { request });

            byte[] answer =
                _exchangeChannel.Read(17);

            frame.ValidateAnswerAndFillSelf(answer);

            float[] parameters =
                GetBrickParamsFromAnswer(
                    frame.Data);

            Brick brick =
                new Brick
                {
                    X = parameters[0],
                    Y = parameters[1],
                    Legth = parameters[2],
                    Width = parameters[3]
                };

            return brick;
        }

        /// <summary>
        /// Запрашивает автоматическую резку ножом бруска.
        /// </summary>
        /// <param name="startPointX">Начальная координата X</param>
        /// <param name="startPointY">Начальная координата Y</param>
        /// <param name="endPointX">Конечная координата X</param>
        /// <param name="endPointY">Конечная координата Y</param>
        /// <param name="widht">Ширина ножа, который будет вытачивать брусок.</param>
        public void SendAutoCutRequest(
            float startPointX,
            float startPointY,
            float endPointX,
            float endPointY,
            float widht,
            float speed)
        {
            Frame frame = new Frame();

            byte[] request =
                frame.GetAutoCuttingRequest(
                    startPointX,
                    startPointY,
                    endPointX,
                    endPointY,
                    widht,
                    speed);

            _exchangeChannel.Write(request);

            byte[] answer =
                _exchangeChannel.Read(1);

            frame.ValidateAnswerAndFillSelf(answer);
        }

        /// <summary>
        /// Посылка запроса на ручной проход ножом по бруску.
        /// </summary>
        /// <param name="direction">Направление, по которому нож должен пойти.</param>
        /// <param name="length">Длина шага, с которой должен пройти нож.</param>
        /// <param name="width">Ширина ножа.</param>
        public void SendManualCutRequest(
            EDirection direction,
            float length,
            float width)
        {
            Frame frame = new Frame();

            byte[] request =
                frame.GetManualCuttingRequest(
                    direction,
                    length,
                    width);

            _exchangeChannel.Write(
                request);

            byte[] answer =
                _exchangeChannel.Read(1);

            frame.ValidateAnswerAndFillSelf(answer);
        }

        //
        // Приватные методы.
        //

        // Интерпретация пришедшего ответа с параметрами бруска от строгального станка.
        private float[] GetBrickParamsFromAnswer(byte[] answer)
        {
            float x = BitConverter.ToSingle(answer, 0);
            float y = BitConverter.ToSingle(answer, 4);
            float length = BitConverter.ToSingle(answer, 8);
            float wigth = BitConverter.ToSingle(answer, 12);

            var parameters = new List<float>
            {
                x,
                y,
                length,
                wigth
            };

            return parameters.ToArray();
        }
    }
}
