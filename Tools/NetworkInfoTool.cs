using System;
using System.Net.NetworkInformation;

namespace WebsNetz.Tools
{
    class NetworkInfoTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                Console.WriteLine($"Name: {ni.Name}");
                Console.WriteLine($"Description: {ni.Description}");
                Console.WriteLine($"Status: {ni.OperationalStatus}");
                Console.WriteLine($"MAC Address: {ni.GetPhysicalAddress()}");
                Console.WriteLine(new string('-', 30));
            }

            ReturnToMenu();
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
