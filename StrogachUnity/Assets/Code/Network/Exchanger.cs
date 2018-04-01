using Strogach.Context;
using System;

namespace Strogach.Network
{
    /// <summary>
    /// Основной класс для общения.
    /// </summary>
    public class Exchanger
    {
        //
        // Приватные переменные.
        //

        // Канал для обмена данными с пультом управления.
        private StrogachChannel _exchangeChannel;

        //
        // Конструкторы.
        //

        public Exchanger(StrogachChannel exchangeChannel)
        {
            _exchangeChannel = exchangeChannel;
        }

        //
        // Публичные методы.
        //

        /// <summary>
        /// Метод, обновляющий контекст данных строгального станка в виде реакции на запрос..
        /// </summary>
        /// <param name="request">Запрос к станку.</param>
        public void React(byte[] request)
        {
            Frame frame = new Frame();
            frame.FillSelfFromRequest(request);

            if (frame.Command == ECommands.Handshake)
            {
                SendOk();
            }
            else if (frame.Command == ECommands.BrickParameters)
            {
                SendParams(
                    ExchangeContext.XCoordinate,
                    ExchangeContext.YCoordinate,
                    ExchangeContext.BrickLength,
                    ExchangeContext.BrickWidth);
            }
            else if (frame.Command == ECommands.Auto)
            {
                SetCoordinatesFromData(frame.Data);
                // TODO: Notify system to start cut
            }
            else if (frame.Command == ECommands.Manual)
            {
                SetManualStepper(frame.Data);
                // TODO: Notify system to go with some step
            }
            else if (frame.Command == ECommands.Stop)
            {
                // TODO: NOtify system to stop auto cut.
            }
        }

        /// <summary>
        /// Отослать параметры бруска.
        /// </summary>
        /// <param name="startPointX">Начальная координата Х ножа.</param>
        /// <param name="startPointY">Начальная координата У ножа.</param>
        /// <param name="length">Длина бруска.</param>
        /// <param name="width">Ширинка бруска.</param>
        public void SendParams(
            float startPointX,
            float startPointY,
            float length,
            float width)
        {
            var frame = new Frame();
            byte[] answer =
                frame.GetBrickParamsAnswer(
                    startPointX,
                    startPointY,
                    length,
                    width);
            _exchangeChannel.Write(answer);
        }

        /// <summary>
        /// Отсылает ответ "Ок"
        /// </summary>
        public void SendOk()
        {
            var frame = new Frame();
            byte answer =
                frame.GetOkAnswer();

            _exchangeChannel.Write(new[] { answer });
        }

        /// <summary>
        /// Отсылает ошибку на запрос ручного прохода по станку.
        /// </summary>
        public void SendManualError()
        {
            var frame = new Frame();
            byte answer =
                frame.GetManualErrorAnswer();

            _exchangeChannel.Write(new[] { answer });
        }

        /// <summary>
        /// Отсылает ошибку на запрос автоматического прохода по станку.
        /// </summary>
        public void SendAutoError()
        {
            var frame = new Frame();
            byte answer =
                frame.GetAutoErrorAnswer();

            _exchangeChannel.Write(new[] { answer });
        }

        //
        // Приватные методы.
        //

        // Устанавливает координаты для контекста из данных.
        private void SetCoordinatesFromData(byte[] data)
        {
            ExchangeContext.XCoordinate = BitConverter.ToSingle(data, 0);
            ExchangeContext.YCoordinate = BitConverter.ToSingle(data, 4);

            ExchangeContext.newXCoordinate = BitConverter.ToSingle(data, 8);
            ExchangeContext.newYCoordinate = BitConverter.ToSingle(data, 12);

            ExchangeContext.CutWidth = BitConverter.ToSingle(data, 16);
        }

        // Устанавлявает координаты для ручного прохода по бруску.
        private void SetManualStepper(byte[] data)
        {
            _exchangeChannel.Context.setDCC( (float) ((EDirection)data[0]), BitConverter.ToSingle(data, 1), BitConverter.ToSingle(data, 5));

            ExchangeContext.Direction = (EDirection)data[0];
            ExchangeContext.CutStep = BitConverter.ToSingle(data, 1);
            ExchangeContext.CutWidth = BitConverter.ToSingle(data, 5);
        }
    }
}
