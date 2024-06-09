using System;
using System.Net;

namespace WebsNetz.Tools
{
    class LocalIPTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
            foreach (IPAddress ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Console.WriteLine($"Local IP Address: {ip}");
                }
            }

            ReturnToMenu();
        }

        static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
