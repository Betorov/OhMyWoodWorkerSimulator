using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strogach.Network
{
    internal class Frame
    {
        public ECommands Command { get; private set; }

        public byte[] Data { get; private set; }
        public void FillSelfFromRequest(byte[] request)
        {
            _command = (ECommands)request[0];

            Data = 
                new byte[request.Length - 1];

            Array.Copy(
                request, 
                1, 
                Data, 
                0, 
                Data.Length);
        }
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

        public byte GetOkAnswer()
        {
            return (byte)EErrors.Ok;
        }

        public byte GetAutoErrorAnswer()
        {
            return (byte)EErrors.ManualError;
        }

        public byte GetManualErrorAnswer()
        {
            return (byte)EErrors.AutoError;
        }

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
