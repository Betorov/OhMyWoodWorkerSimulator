using Strogach.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeChannel.Network;

namespace Strogach.Network
{
    public class Exchanger
    {
        private StrogachChannel _exchangeChannel;
        private Channel exchangeChannel;

        public Exchanger(StrogachChannel exchangeChannel)
        {
            _exchangeChannel = exchangeChannel;
        }

        public Exchanger(Channel exchangeChannel)
        {
            this.exchangeChannel = exchangeChannel;
        }

        internal void SendHandshakeRequestAsync()
        {
            throw new NotImplementedException();
        }

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
            else if(frame.Command == ECommands.Stop)
            {
                // TODO: NOtify system to stop auto cut.
            }
        }

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

        public void SendOk()
        {
            var frame = new Frame();
            byte answer =
                frame.GetOkAnswer();

            _exchangeChannel.Write(new[] { answer });
        }

        public void SendManualError()
        {
            var frame = new Frame();
            byte answer =
                frame.GetManualErrorAnswer();

            _exchangeChannel.Write(new[] { answer });
        }

        public void SendAutoError()
        {
            var frame = new Frame();
            byte answer =
                frame.GetAutoErrorAnswer();

            _exchangeChannel.Write(new[] { answer });
        }

        private void SetCoordinatesFromData(byte[] data)
        {
            ExchangeContext.XCoordinate = BitConverter.ToSingle(data, 0);
            ExchangeContext.YCoordinate = BitConverter.ToSingle(data, 4);

            ExchangeContext.newXCoordinate = BitConverter.ToSingle(data, 8);
            ExchangeContext.newYCoordinate = BitConverter.ToSingle(data, 12);

            ExchangeContext.CutWidth = BitConverter.ToSingle(data, 16);
        }

        private void SetManualStepper(byte[] data)
        {
            ExchangeContext.Direction = (EDirection)data[0];
            ExchangeContext.CutStep = BitConverter.ToSingle(data, 1);
            ExchangeContext.CutWidth = BitConverter.ToSingle(data, 5);
        }
    }
}
