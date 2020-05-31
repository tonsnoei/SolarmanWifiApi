using SolarmanWifiApi.services;
using SolarmanWifiApi.services.Models;
using SolarmanWifiApi.services.Models;
using System;

namespace SolarmanWifiTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ISolarmanWifiConnectorService service = new SolarmanWifiConnectorService();
            service.WifiCardIP = "192.168.1.168";
            service.WifiCardPort = 8899;
            service.WifiCardSerial = 645703039;

            // 2 rows below are purely for demo purposes
            SolarmanRequestFrame frame = new SolarmanRequestFrame(service.WifiCardSerial);
            //Console.WriteLine($"Network request frame: {frame.GetAsHex()}");

            SolarmanResponseFrame inverterData = service.GetInverterData();

            //Console.WriteLine($"InverterData: {inverterData.RawDataAsHex}");
            //Console.WriteLine($"Length: {inverterData.MessageLength}");
            //Console.WriteLine($"Message: {inverterData.Message}");

            PrintHeader("Device Info", '=');
            PrintNameValue("Device ID", $"{inverterData.InverterId}");
            PrintNameValue("Run State", $"{inverterData.RunState}");
            PrintNameValue("Power Generated Today", inverterData.EnergyGeneratedToday, "kWh");
            PrintNameValue("Power Generated Total", inverterData.EnergyGeneratedTotal, "kWh");
            PrintNameValue("Temperature", inverterData.Temperature, "°C");
            PrintNameValue("Temperature Fault", inverterData.TempFaultValue, "°C");
            PrintNameValue("Error Message", inverterData.ErrorMessage.ToString());
            Console.WriteLine("");

            PrintHeader("PV Info", '=');
            PrintPVStringInfo("PV - String 1", inverterData.PVInfo.String1);
            PrintPVStringInfo("PV - String 2", inverterData.PVInfo.String2);
            PrintPVStringInfo("PV - String 3", inverterData.PVInfo.String3);
            PrintNameValue("Voltage Fault", inverterData.PVInfo.VoltageFault, "V");
            Console.WriteLine("");

            PrintHeader("Grid Info", '=');
            PrintHeader("General", '-');
            PrintNameValue("Voltage Fault", inverterData.PowerGridInfo.VoltageFault, "V");
            PrintNameValue("Earth Circuit Current Fault", inverterData.PowerGridInfo.EarthLeakageCircuitCurrentFault, "A");
            PrintNameValue("Frequency Fault", inverterData.PowerGridInfo.FrequencyFault, "Hz");
            PrintNameValue("Impedance Fault", inverterData.PowerGridInfo.ImpedanceFault, "Ohm");
            PrintGridStringInfo("Grid - Phase 1", inverterData.PowerGridInfo.Phase1);
            PrintGridStringInfo("Grid - Phase 2", inverterData.PowerGridInfo.Phase2);
            PrintGridStringInfo("Grid - Phase 3", inverterData.PowerGridInfo.Phase3);
        }

        private static void PrintPVStringInfo(string title, PVStringInfo stringInfo)
        {
            PrintHeader(title, '-');
            PrintNameValue("Voltage", stringInfo.Voltage, "V");
            PrintNameValue("Current", stringInfo.Current, "A");
            PrintNameValue("Power", stringInfo.Power, "Watt");
        }

        private static void PrintGridStringInfo(string title, PowerGridStringInfo stringInfo)
        {
            PrintHeader(title, '-');
            PrintNameValue("Voltage", stringInfo.Voltage, "V");
            PrintNameValue("Current", stringInfo.Current, "A");
            PrintNameValue("Frequency", stringInfo.Frequency, "Hz");
            PrintNameValue("Power", stringInfo.Power, "Watt");
        }

        private static void PrintNameValue(string name, string value)
        {
            Console.WriteLine($"{name.PadRight(40, ' ')} : {value.PadLeft(35, ' ')}");
        }

        private static void PrintNameValue(string name, float value, string units)
        {
            Console.WriteLine($"{name.PadRight(40, ' ')} : {Math.Round(value, 2).ToString("0.00").PadLeft(32, ' ')} {units}");
        }

        private static void PrintHeader(string title, char padChar)
        {
            title = $" [{title}] ";
            int padSize = 40 + title.Length / 2;
            Console.WriteLine($"{title.PadLeft(padSize, padChar).PadRight(80, padChar)}");
        }
    }
}
