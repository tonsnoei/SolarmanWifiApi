using SolarmanWifiApi.services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolarmanWifiApi.services.Models
{
    public class SolarmanResponseFrame
    {
        public byte[] RawData { private set; get; }

        public SolarmanResponseFrame(byte[] rawData)
        {
            this.RawData = rawData;
        }

        public string RawDataAsHex => BitConverter.ToString(RawData).Replace("-", ":");
        public int MessageLength => GetInt(1);
        public string Message => GetString(12, MessageLength);
        public string InverterId => GetString(15, 16);
        public float Temperature => GetShortFloat(31, 10);
        /// <summary>
        /// In kWh
        /// </summary>
        public float EnergyGeneratedToday => GetShortFloat(69, 100);
        /// <summary>
        /// In kWh
        /// </summary>
        public float EnergyGeneratedTotal => GetLongFloat(71);
        /// <summary>
        /// In Hours
        /// </summary>
        public float TotalOperationTime => GetLongFloat(75, 1);
        public float RunState => GetShortFloat(79, 1);
        public PVInfo PVInfo => new PVInfo() {
            String1 = new PVStringInfo()
            {
                Voltage = GetShortFloat(33),
                Current = GetShortFloat(39),
            },
            String2 = new PVStringInfo()
            {
                Voltage = GetShortFloat(35),
                Current = GetShortFloat(41),
            },
            String3 = new PVStringInfo()
            {
                Voltage = GetShortFloat(37),
                Current = GetShortFloat(43),
            },
            VoltageFault = GetShortFloat(89, 10),             
        };

        public PowerGridInfo PowerGridInfo => new PowerGridInfo() {
            Phase1 = new PowerGridStringInfo
            {
                Voltage = GetShortFloat(51),
                Frequency = GetShortFloat(57, 100),
                Power = GetShortFloat(59, 1)
            },
            Phase2 = new PowerGridStringInfo
            {
                Voltage = GetShortFloat(53),
                Frequency = GetShortFloat(61, 100),
                Power = GetShortFloat(63, 1)
            },
            Phase3 = new PowerGridStringInfo
            {
                Voltage = GetShortFloat(55),
                Frequency = GetShortFloat(65, 100),
                Power = GetShortFloat(67, 1)
            },
            EarthLeakageCircuitCurrentFault = GetShortFloat(91, 1000),
            FrequencyFault = GetShortFloat(83, 100),
            ImpedanceFault = GetShortFloat(85, 100),
            VoltageFault = GetShortFloat(81, 10),
        };


        /// <summary>
        /// Temperature fault value in In degrees celsius
        /// </summary>
        public float TempFaultValue => GetShortFloat(87, 10); 
        /// <summary>
        /// errorMsg binary index value 
        /// </summary>
        public float ErrorMessage => GetLongFloat(93);

        private UInt16 GetInt(int offset)
        {
            return RawData[offset];
        }

        private string GetString(int offset, int length)
        {
            byte[] stringBuffer = new byte[length];
            Buffer.BlockCopy(RawData, offset, stringBuffer, 0, length);
            return Encoding.Default.GetString(stringBuffer).Trim();
        }

        private float GetLongFloat(int offset, int divider = 10)
        {
            return ToUInt32(RawData, offset) / divider;
        }

        private float GetShortFloat(int offset, int divider = 10)
        {
            float result = 0;
            ushort value = ToUInt16(RawData, offset);
            if (value > 32767)
            {
                result = -1 * (Convert.ToSingle(65536) - value) / divider;
            }
            else
            {
                result = Convert.ToSingle(value) / divider;
            }

            return result;
        }

        private UInt32 ToUInt32(byte[] value, int startIndex)
        {
            return unchecked((uint)(FromBytesAsserted(value, startIndex, 4)));
        }

        private UInt16 ToUInt16(byte[] value, int startIndex)
        {
            return unchecked((ushort)(FromBytesAsserted(value, startIndex, 2)));
        }

        long FromBytesAsserted(byte[] value, int startIndex, int length)
        {
            AssertArguments(value, startIndex, length);
            return FromBytes(value, startIndex, length);
        }

        static void AssertArguments(byte[] value, int startIndex, int length)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (startIndex < 0 || startIndex > value.Length - length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
        }

        long FromBytes(byte[] buffer, int startIndex, int length)
        {
            long result = 0;
            for (int i = 0; i < length; i++)
            {
                result = unchecked((result << 8) | buffer[startIndex + i]);
            }
            return result;
        }
    }
}

