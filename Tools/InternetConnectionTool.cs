using System;
using System.Net.NetworkInformation;

namespace WebsNetz.Tools
{
    class InternetConnectionTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8");

                    if (reply.Status == IPStatus.Success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Internet connection is available.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No internet connection available.");
                    }
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No internet connection available.");
            }
            finally
            {
                Console.ResetColor();
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
