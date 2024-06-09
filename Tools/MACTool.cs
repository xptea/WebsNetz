using System;
using System.Net.NetworkInformation;

namespace WebsNetz.Tools
{
    class MACTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine($"Name: {nic.Name}");
                Console.WriteLine($"MAC Address: {nic.GetPhysicalAddress()}");
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
