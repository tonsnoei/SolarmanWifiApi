using SolarmanWifiApi.services.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace SolarmanWifiApi.services
{
    public class SolarmanWifiConnectorService : ISolarmanWifiConnectorService
    {
        public string WifiCardIP { get; set; }
        public int WifiCardPort { get; set; } = 8899;
        public long WifiCardSerial { get; set; }

        public SolarmanResponseFrame GetInverterData()
        {
            AssertProperties();

            using(Socket inverterSocket = CreateInverterSocket())
            {
                try
                {
                    this.Connect(inverterSocket);
                    int bytesSent = inverterSocket.Send(CreateInverterRequestMessage());
                    byte[] buffer = new byte[1024];
                    int bytesReceived = inverterSocket.Receive(buffer);
                    this.Disconnect(inverterSocket);
                    byte[] dataBytes = new byte[bytesReceived];
                    Buffer.BlockCopy(buffer, 0, dataBytes, 0, bytesReceived);
                    return new SolarmanResponseFrame(dataBytes);
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            return null;
        }

        private void AssertProperties()
        {
            AssertIp();
            AssertPort();
            AssertSerial();
        }

        private void AssertIp()
        {
            if (!IPAddress.TryParse(WifiCardIP, out _)) 
            {
                throw new ArgumentException($"{nameof(WifiCardIP)} needs to be set to a proper IP address");
            }
        }

        private void AssertPort()
        {
            if (WifiCardPort <= 0 || WifiCardPort > UInt16.MaxValue)
            {
                throw new ArgumentException($"{nameof(WifiCardPort)} should be between 1 and 65335. Default is: 8899");
            }
        }

        private void AssertSerial()
        {
            if (WifiCardSerial <= 10000)
            {
                throw new ArgumentException($"{nameof(WifiCardSerial)} should be something like: 645703099. Normally it will be available as a label in one of the sides of your solar inverter.");
            }
        }

        private void Connect(Socket inverterSocket)
        {
            inverterSocket.Connect(CreateInverterEndPoint());
        }

        private void Disconnect(Socket inverterSocket)
        {
            inverterSocket.Shutdown(SocketShutdown.Both);
            inverterSocket.Close();
        }

        private Socket CreateInverterSocket()
        {
            return new Socket(GetInverterIP().AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        private EndPoint CreateInverterEndPoint()
        {
            return new IPEndPoint(GetInverterIP(), WifiCardPort);
        }

        private IPAddress GetInverterIP()
        {
            return IPAddress.Parse(WifiCardIP);
        }

        private byte[] CreateInverterRequestMessage()
        {
            return new SolarmanRequestFrame(WifiCardSerial).Get();
        }
    }
}
