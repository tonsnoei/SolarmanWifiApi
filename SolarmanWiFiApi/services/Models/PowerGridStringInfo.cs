using System;
using System.Collections.Generic;
using System.Text;

namespace SolarmanWifiApi.services.Models
{
    public class PowerGridStringInfo
    {
        /// <summary>
        /// In Volt (V)
        /// </summary>
        public float Voltage { get; set; }
        /// <summary>
        /// In Watt (W)
        /// </summary>
        public float Power { get; set; }
        /// <summary>
        /// In Ampere (A)
        /// </summary>
        public float Current => Power / Voltage;
        /// <summary>
        /// In Hertz (Hz)
        /// </summary>
        public float Frequency { get; set; }
    }
}
