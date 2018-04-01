using Strogach.Context;
using System;
using ExchangeChannel.Network;

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
        private enum EModeState
        {
            auto = 1,
            manual = 2,
            stop = 3
        }
        // Канал для обмена данными с пультом управления.
        private StrogachChannel _exchangeChannel;

        // Канал для обмена данными со строгачем.
        private ExchangeContext _exchangeContext;

        //
        // Конструкторы.
        //

        public Exchanger(StrogachChannel exchangeChannel)
        {
            _exchangeChannel = exchangeChannel;
        }

        public Exchanger(ExchangeContext exchangeContext)
        {
            _exchangeContext = exchangeContext;
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
                    _exchangeContext.XCoordinate,
                    _exchangeContext.YCoordinate,
                    _exchangeContext.BrickLength,
                    _exchangeContext.BrickWidth
                    );
            }
            else if (frame.Command == ECommands.Auto)
            {
                SetCoordinatesFromData(frame.Data);
                _exchangeContext.State((int)EModeState.auto);
            }
            else if (frame.Command == ECommands.Manual)
            {
                SetManualStepper(frame.Data);
                _exchangeContext.State((int)EModeState.manual);
                // TODO: Notify system to go with some step
            }
            else if (frame.Command == ECommands.Stop)
            {
                _exchangeContext.State((int)EModeState.stop);
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
                float[] getParam = new float[5];
                getParam[1] = (_exchangeContext.XCoordinate = BitConverter.ToSingle(data, 0));
                getParam[2] = (_exchangeContext.YCoordinate = BitConverter.ToSingle(data, 4));

                getParam[3] = (_exchangeContext.newXCoordinate = BitConverter.ToSingle(data, 8));
                getParam[4] = (_exchangeContext.newYCoordinate = BitConverter.ToSingle(data, 12));

                getParam[5] = (_exchangeContext.CutWidth = BitConverter.ToSingle(data, 16));
                _exchangeContext.GetCoordinatesFromData(getParam);
        }

        // Устанавлявает координаты для ручного прохода по бруску.
        private void SetManualStepper(byte[] data)
        {
            float[] getParam = new float[3];
            getParam[1] = (float)(_exchangeContext.Direction = (EDirection)data[0]);
            getParam[2] = (_exchangeContext.CutStep = BitConverter.ToSingle(data, 1));
            getParam[3] = (_exchangeContext.CutWidth = BitConverter.ToSingle(data, 5));
            _exchangeContext.GetCoordinatesFromData(getParam);
        }
    }
}
