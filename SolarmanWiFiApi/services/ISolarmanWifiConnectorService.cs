using SolarmanWifiApi.services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolarmanWifiApi.services
{
    public interface ISolarmanWifiConnectorService
    {
        string WifiCardIP { get; set; }
        int WifiCardPort { get; set; }
        long WifiCardSerial { get; set; }

        SolarmanResponseFrame GetInverterData();
    }
}
