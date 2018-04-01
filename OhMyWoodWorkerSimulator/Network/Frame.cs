using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Network
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
        /// Данные, которые обрабатывает фрейм.
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }


        /// <summary>
        /// Команда, по которой текущий фрейм работает.
        /// </summary>
        private ECommands _currentCommand;

        //
        // Публичные методы
        //

        /// <summary>
        /// Формирует запрос на рукопожатие.
        /// </summary>
        public byte GetHandshakeRequest()
        {
            return
                (byte)ECommands.Handshake;
        }

        /// <summary>
        /// Формирует запрос на параметры бруска.
        /// </summary>
        public byte GetBrickParamsRequest()
        {
            _currentCommand = ECommands.BrickParameters;
            return
                (byte)_currentCommand;
        }

        /// <summary>
        /// Формирует запрос на автоматическую резку бруска.
        /// </summary>
        /// <param name="startPointX">Начальная координата X.</param>
        /// <param name="startPointY">Начальная координата Y.</param>
        /// <param name="endPointX">Конечная координата X.</param>
        /// <param name="endPointY">Конечная координата Y.</param>
        /// <param name="widht">Ширина ножа.</param>
        public byte[] GetAutoCuttingRequest(
            float startPointX,
            float startPointY,
            float endPointX,
            float endPointY,
            float widht,
            float speed)
        {
            _currentCommand = ECommands.Auto;
            var request = new List<byte>();

            request.Add(
                (byte)_currentCommand);

            byte[] usefulData =
                GetDataFromParams(
                    startPointX,
                    startPointY,
                    endPointX,
                    endPointY,
                    widht,
                    speed);

            request.AddRange(usefulData);

            return request.ToArray();
        }

        /// <summary>
        /// Формирует запрос на ручную пошаговую резку бруска.
        /// </summary>
        /// <param name="direction">Направление резки.</param>
        /// <param name="length">Длина шага, с которым пойдёт нож по бруску.</param>
        /// <param name="width">Ширина ножа</param>
        public byte[] GetManualCuttingRequest(
            EDirection direction,
            float length,
            float width)
        {
            _currentCommand = ECommands.Manual;
            var request = new List<byte>();

            request.Add((byte)_currentCommand);
            request.Add((byte)direction);

            byte[] usefulData =
                GetDataFromParams(
                    length,
                    width);

            return request.ToArray();
        }

        /// <summary>
        /// Проверка входящего ответа от устройства.
        /// </summary>
        /// <param name="answer">Ответ от устройства на команду.</param>
        public void ValidateAnswerAndFillSelf(byte[] answer)
        {
            if (answer.First() == (byte)EErrors.Ok)
                EnsureResultCode(answer.First());
            else if (answer[0] != (byte)_currentCommand)
                throw new Exception("Неправильный ответ на команду.");

            Data = new byte[answer.Length - 1];
            Array.Copy(answer, 1, Data, 0, Data.Length);
        }

        //
        // Приветные методы.
        //

        // Преобразует данные в байтовый вид.
        private byte[] GetDataFromParams(params float[] settings)
        {
            var data = new List<byte>();

            foreach (var value in settings)
            {
                byte[] val =
                    BitConverter.GetBytes(
                        value);

                data.AddRange(val);
            }

            return data.ToArray();
        }

        // Производит проверку входящего ответа.
        private void EnsureResultCode(byte answer)
        {
            if (answer != (byte)EErrors.Ok)
                throw new Exception("Ошибка ответа.");
        }
    }
}
