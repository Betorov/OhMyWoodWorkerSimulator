using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhMyWoodWorkerSimulator.Network
{
    internal class Frame
    {
        public byte[] Data
        {
            get;
            private set;
        }

        private ECommands _currentCommand;



        public byte GetHandshakeRequest()
        {
            return 
                (byte)ECommands.Handshake;
        }

        public byte GetBrickParamsRequest()
        {
            _currentCommand = ECommands.BrickParameters;
            return 
                (byte)_currentCommand;
        }

        public byte[] GetAutoCuttingRequest(
            float startPointX, 
            float startPointY, 
            float endPointX, 
            float endPointY, 
            float widht)
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
                    widht);

            request.AddRange(usefulData);

            return request.ToArray();
        }

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

        public void ValidateAnswerAndFillSelf(byte[] answer)
        {
            if (answer.Length == 1)
                EnsureResultCode(answer.First());

            if (answer[0] != (byte)_currentCommand)
                throw new Exception("Неправильный ответ на команду.");

            Data = new byte[answer.Length - 1];
            Array.Copy(answer, 1, Data, 0, Data.Length);
        }
        private byte[] GetDataFromParams(params float[] settings)
        {
            var data = new List<byte>();

            foreach(var value in settings)
            {
                byte[] val = 
                    BitConverter.GetBytes(
                        value);

                data.AddRange(val);
            }

            return data.ToArray();
        }

        private void EnsureResultCode(byte answer)
        {
            if (answer != (byte)EErrors.Ok)
                throw new Exception("Ошибка ответа.");
        }
    }
}
