using System;
using System.Collections.Generic;
using System.Text;

namespace SolarmanWifiApi.services.Models
{
    public class PVInfo
    {
        public PVStringInfo String1 { get; set; } = new PVStringInfo();
        public PVStringInfo String2 { get; set; } = new PVStringInfo();
        public PVStringInfo String3 { get; set; } = new PVStringInfo();
        /// <summary>
        /// In Volt (V)
        /// </summary>
        public float VoltageFault { get; set; }
    }
}
