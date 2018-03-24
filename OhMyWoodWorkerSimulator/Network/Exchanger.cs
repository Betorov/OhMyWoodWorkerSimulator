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
    public class Exchanger
    {
        private Channel _exchangeChannel;
        public Exchanger(Channel exchangeChannel)
        {
            _exchangeChannel = exchangeChannel;
        }

        public void SendHandshakeRequestAsync()
        {
            Frame frame = new Frame();
            byte request = frame.GetHandshakeRequest();

            _exchangeChannel.Write(new[] { request });

            byte[] answer = 
                _exchangeChannel.Read();

            frame.ValidateAnswerAndFillSelf(answer);
        }

        public Brick GetBrickParams()
        {
            Frame frame = new Frame();
            byte request = frame.GetBrickParamsRequest();
            _exchangeChannel.Write(new[] { request });

            byte[] answer = 
                _exchangeChannel.Read();

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

        public void SendAutoCutRequest(
            float startPointX,
            float startPointY,
            float endPointX,
            float endPointY,
            float widht)
        {
            Frame frame = new Frame();

            byte[] request = 
                frame.GetAutoCuttingRequest(
                    startPointX, 
                    startPointY, 
                    endPointX, 
                    endPointY, 
                    widht);

            _exchangeChannel.Write(request);

            byte[] answer = 
                _exchangeChannel.Read();

            frame.ValidateAnswerAndFillSelf(answer);
        }

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
                _exchangeChannel.Read();

            frame.ValidateAnswerAndFillSelf(answer);
        }

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
