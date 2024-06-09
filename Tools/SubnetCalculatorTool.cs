using System;

namespace WebsNetz.Tools
{
    class SubnetCalculatorTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            Console.Write("Enter IP Address (e.g., 192.168.1.1): ");
            string ipAddress = Console.ReadLine();

            Console.Write("Enter Subnet Mask (e.g., 255.255.255.0): ");
            string subnetMask = Console.ReadLine();

            CalculateSubnet(ipAddress, subnetMask);

            ReturnToMenu();
        }

        private static void CalculateSubnet(string ipAddress, string subnetMask)
        {
            var ip = System.Net.IPAddress.Parse(ipAddress);
            var mask = System.Net.IPAddress.Parse(subnetMask);

            var ipBytes = ip.GetAddressBytes();
            var maskBytes = mask.GetAddressBytes();

            var networkBytes = new byte[ipBytes.Length];
            for (int i = 0; i < ipBytes.Length; i++)
            {
                networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            var networkAddress = new System.Net.IPAddress(networkBytes);
            Console.WriteLine($"Network Address: {networkAddress}");

            var broadcastBytes = new byte[ipBytes.Length];
            for (int i = 0; i < ipBytes.Length; i++)
            {
                broadcastBytes[i] = (byte)(networkBytes[i] | ~maskBytes[i]);
            }

            var broadcastAddress = new System.Net.IPAddress(broadcastBytes);
            Console.WriteLine($"Broadcast Address: {broadcastAddress}");
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
