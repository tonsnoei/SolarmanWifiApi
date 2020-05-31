using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SolarmanWifiApi.services.Models
{
    /// <summary>
    /// The request string is build from several parts. 
    /// 1] Header: 4 bytes
    /// 2] the second part is the reversed hex notation of the s/n twice; 
    /// 3] then again a fixed string of two chars; 
    /// 4] a checksum of the double s/n with an offset; 
    /// 5] and finally a fixed ending char.
    /// #frame = (headCode) + (dataFieldLength) + (contrlCode) + (sn) + (sn) + (command) + (checksum) + (endCode)
    /// </summary>
    public class SolarmanRequestFrame
    {
        private readonly string FRAME_HEADER = "680241b1";
        private readonly string FRAME_COMMAND = "0100";
        private readonly string FRAME_DEFAULT_CHECKSUM = "87";
        private readonly string FRAME_END_CODE = "16";

        private long inverterSerialNumber;

        public SolarmanRequestFrame(long inverterSerialNumber)
        {
            this.inverterSerialNumber = inverterSerialNumber;
        }

        public byte[] Get()
        {
            byte[] frame = CreateBasicFrame();
            SetChecksum(frame);
            return frame;
        }

        public string GetAsHex()
        {
            return BitConverter.ToString(Get()).Replace("-", ":");
        }

        private void SetChecksum(byte[] basicFrame)
        {
            byte checksum = GetChecksum(basicFrame);
            basicFrame[basicFrame.Length - 2] = checksum;
        }

        private byte GetChecksum(byte[] basicFrame)
        {
            // Checksum is calculated from the second byte to the end without the last 2 bytes of the frame
            byte checksum = 0;
            for(int i=1; i<basicFrame.Length-3; i++)
            {
                checksum += basicFrame[i];
            }
            checksum &= 255;
            return BitConverter.GetBytes(checksum)[0];
        }

        private byte[] CreateBasicFrame()
        {
            byte[] header = HexStringToByteArray(FRAME_HEADER);
            byte[] reversedSerial = GetInverterSerialNumberBytes();
            byte[] command = HexStringToByteArray(FRAME_COMMAND);
            byte[] defaultChecksum = HexStringToByteArray(FRAME_DEFAULT_CHECKSUM);
            byte[] endCode = HexStringToByteArray(FRAME_END_CODE);

            byte[] frame = new byte[header.Length + reversedSerial.Length + reversedSerial.Length + command.Length + defaultChecksum.Length + endCode.Length];
            
            // Header
            int offset = 0;
            System.Buffer.BlockCopy(header, 0, frame, offset, header.Length);
            
            // Rev. Serial
            offset = offset + header.Length;
            System.Buffer.BlockCopy(reversedSerial, 0, frame, offset, reversedSerial.Length);
            
            // Rev. Serial
            offset = offset + reversedSerial.Length;
            System.Buffer.BlockCopy(reversedSerial, 0, frame, offset, reversedSerial.Length);
            
            // Command
            offset = offset + reversedSerial.Length;
            System.Buffer.BlockCopy(command, 0, frame, offset, command.Length);
            
            // Checksum
            offset = offset + command.Length;
            System.Buffer.BlockCopy(defaultChecksum, 0, frame, offset, defaultChecksum.Length);
            
            // End Code
            offset = offset + defaultChecksum.Length;
            System.Buffer.BlockCopy(endCode, 0, frame, offset, endCode.Length);

            return frame;
        }

        private byte[] GetReversedInverterSerialNumberBytes()
        {
            byte[] inverterSerialNumberBytes = GetInverterSerialNumberBytes();
            Array.Reverse(inverterSerialNumberBytes);
            return inverterSerialNumberBytes;
        }

        private byte[] GetInverterSerialNumberBytes()
        {
            byte[] longBytes = BitConverter.GetBytes(inverterSerialNumber);
            byte[] result = new byte[4];
            Buffer.BlockCopy(longBytes, 0, result, 0, 4);
            return result;
        }

        private byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
