using System;
using System.Net;

namespace WebsNetz.Tools
{
    class ExternalIPTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            try
            {
                using (WebClient client = new WebClient())
                {
                    string externalIP = client.DownloadString("https://api.ipify.org").Trim();
                    Console.WriteLine($"External IP Address: {externalIP}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
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
