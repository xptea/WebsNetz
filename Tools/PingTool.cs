using System;
using System.Net.NetworkInformation;

namespace WebsNetz.Tools
{
    class PingTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            Console.Write("Enter IP Address to ping: ");
            string ipAddress = Console.ReadLine();

            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = ping.Send(ipAddress);

                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine($"\nPing to {ipAddress} successful. Time: {reply.RoundtripTime}ms");
                    }
                    else
                    {
                        Console.WriteLine($"\nPing to {ipAddress} failed. Status: {reply.Status}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
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
