using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace WebsNetz.Tools
{
    class PingSweepTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            Console.Write("Enter the base IP (e.g., 192.168.1.): ");
            string baseIp = Console.ReadLine();

            Console.Write("Enter starting IP (last octet): ");
            int startIp = int.Parse(Console.ReadLine());

            Console.Write("Enter ending IP (last octet): ");
            int endIp = int.Parse(Console.ReadLine());

            PingSweep(baseIp, startIp, endIp).Wait();

            ReturnToMenu();
        }

        private static async Task PingSweep(string baseIp, int startIp, int endIp)
        {
            for (int i = startIp; i <= endIp; i++)
            {
                string ip = baseIp + i;
                if (await IsPingSuccessful(ip))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{ip} is up");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ip} is down");
                }
            }

            Console.ResetColor();
        }

        private static async Task<bool> IsPingSuccessful(string ip)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = await ping.SendPingAsync(ip, 1000);
                    return reply.Status == IPStatus.Success;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
