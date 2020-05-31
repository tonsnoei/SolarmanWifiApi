# SolarmanWifiApi
This C# library can be used to retrieve solar yield info from Omnik, Hosola, Goodwe, Solax, Ginlong, Samil, Sofar or Power-One Solar inverters.
It connects to most inverters with an Solarman Wifi device integrated.

The library is written for .NET Standard 2.0 and can be used in most cases. No dependencies.

## How it works?
This library connects to the inverter using local network, so it is important to have your inverter internal WiFi card connected to your network. What you need to use it, is:
* The IP address of the Solarman Wifi Card in your inverter
* The port it is running on. Normally: 8899
* The serial number of the Solarman Wifi Card. Normally placed on a label on the left or right side of your inverter. It is a 8 digits number starting with "6". (e.g. 645703039)

### Code
The code below shows how to use it.
```C#
ISolarmanWifiConnectorService service = new SolarmanWifiConnectorService();
service.WifiCardIP = "192.168.1.168";
service.WifiCardPort = 8899;
service.WifiCardSerial = 645703039;

SolarmanResponseFrame inverterData = service.GetInverterData();

Console.WriteLine($"Device ID: {inverterData.InverterId}");
Console.WriteLine($"Run State: {inverterData.RunState}");
Console.WriteLine($"Power Generated Today: {inverterData.EnergyGeneratedToday} kWh");
Console.WriteLine($"Power Generated Total: {inverterData.EnergyGeneratedTotal} kWh");
Console.WriteLine($"PV - String 1 Power: {inverterData.PVInfo.String1.Power} Watt");
Console.WriteLine($"PV - String 2 Power: {inverterData.PVInfo.String2.Power} Watt");
Console.WriteLine($"PV - String 3 Power: {inverterData.PVInfo.String3.Power} Watt");

See also the SolarmanWifiTestConsole project
