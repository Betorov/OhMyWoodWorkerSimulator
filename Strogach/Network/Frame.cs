using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Network
{
    /// <summary>
    /// Класс - протокол.
    /// </summary>
    internal class Frame
    {
        //
        // Публичные переменные.
        //

        /// <summary>
        /// Команда, с которой работает фрейм.
        /// </summary>
        public ECommands Command { get; private set; }

        /// <summary>
        /// Данные, которые обрабатывает фрейм.
        /// </summary>
        public byte[] Data { get; private set; }

        //
        // Публичные методы.
        //

        /// <summary>
        /// Самозаполнение фрейма из запроса к строгальному станку.
        /// </summary>
        /// <param name="request">Запрос к строгальному станку.</param>
        public void FillSelfFromRequest(byte[] request)
        {
            Data =
                new byte[request.Length - 1];

            Command = (ECommands)request[0];

            Array.Copy(
                request,
                1,
                Data,
                0,
                Data.Length);
        }

        /// <summary>
        /// Формирует ответ на запрос параметров станка.
        /// </summary>
        /// <param name="startPointX">Начальная точка Х.</param>
        /// <param name="startPointY">Начальная точка Y.</param>
        /// <param name="length">Длина бруска.</param>
        /// <param name="width">Ширина бруска.</param>
        /// <returns></returns>
        public byte[] GetBrickParamsAnswer(
            float startPointX,
            float startPointY,
            float length,
            float width)
        {
            var answer = new List<byte>();

            answer.Add((byte)ECommands.BrickParameters);

            byte[] usefulData =
                GetUserfulDataFromParams(
                    startPointX,
                    startPointY,
                    length,
                    width);

            answer.AddRange(usefulData);

            return answer.ToArray();
        }

        /// <summary>
        /// Формирует ответ, который обозначает, что запрос обработался без проблем.
        /// </summary>
        public byte GetOkAnswer()
        {
            return (byte)EErrors.Ok;
        }

        /// <summary>
        /// Формирует ответ - ошибку об автоматическом проходе по бруску.
        /// </summary>
        public byte GetAutoErrorAnswer()
        {
            return (byte)EErrors.AutoError;
        }

        /// <summary>
        /// Формирует ответ - ошибку об ручном проходе по бруску.
        /// </summary>
        /// <returns></returns>
        public byte GetManualErrorAnswer()
        {
            return (byte)EErrors.ManualError;
        }

        //
        // Приватные методы.
        //

        // Преобразует данные в байтовый вид.
        private byte[] GetUserfulDataFromParams(params float[] settings)
        {
            var usefulData = new List<byte>();

            byte[] X = BitConverter.GetBytes(settings[0]);
            byte[] Y = BitConverter.GetBytes(settings[1]);
            byte[] legth = BitConverter.GetBytes(settings[2]);
            byte[] wigth = BitConverter.GetBytes(settings[3]);

            usefulData.AddRange(X);
            usefulData.AddRange(Y);
            usefulData.AddRange(legth);
            usefulData.AddRange(wigth);

            return usefulData.ToArray();
        }
    }
}
