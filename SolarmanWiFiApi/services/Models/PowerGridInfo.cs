using System;
using System.Collections.Generic;
using System.Text;

namespace SolarmanWifiApi.services.Models
{
    public class PowerGridInfo
    {
        public PowerGridStringInfo Phase1 { get; set; } = new PowerGridStringInfo();
        public PowerGridStringInfo Phase2 { get; set; } = new PowerGridStringInfo();
        public PowerGridStringInfo Phase3 { get; set; } = new PowerGridStringInfo();

        /// <summary>
        /// In Ohm ( )
        /// </summary>
        public float ImpedanceFault;
        /// <summary>
        /// In Hertz (Hz)
        /// </summary>
        public float FrequencyFault;
        /// <summary>
        /// In Volt (V)
        /// </summary>
        public float VoltageFault;
        /// <summary>
        /// In Ampere (A)
        /// </summary>
        public float EarthLeakageCircuitCurrentFault;

    }
}
