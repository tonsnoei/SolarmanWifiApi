using System;
using System.Collections.Generic;
using System.Text;

namespace SolarmanWifiApi.services.Models
{
    public class PVStringInfo
    {
        /// <summary>
        /// In Volt (V)
        /// </summary>
        public float Voltage { get; set; }
        /// <summary>
        /// In Ampere (A)
        /// </summary>
        public float Current { get; set; }
        /// <summary>
        /// In Watts (W)
        /// </summary>
        public float Power => Voltage * Current;
    }
}
