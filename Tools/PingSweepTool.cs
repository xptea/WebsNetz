using System;
using System.Net.NetworkInformation;
using System.Threading;
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
            if (!int.TryParse(Console.ReadLine(), out int startIp))
            {
                Console.WriteLine("Invalid starting IP.");
                ReturnToMenu();
                return;
            }

            Console.Write("Enter ending IP (last octet): ");
            if (!int.TryParse(Console.ReadLine(), out int endIp))
            {
                Console.WriteLine("Invalid ending IP.");
                ReturnToMenu();
                return;
            }

            var cts = new CancellationTokenSource();
            var loadingThread = new Thread(() => ShowLoadingScreen(cts.Token));
            loadingThread.Start();

            Task.Run(async () =>
            {
                await PingSweep(baseIp, startIp, endIp);
                cts.Cancel(); // Stop the loading screen
                loadingThread.Join(); // Wait for the loading thread to finish
                ReturnToMenu();
            }).Wait();
        }

        private static void ShowLoadingScreen(CancellationToken token)
        {
            string[] loadingAnimation = { "/", "-", "\\", "|" };
            int animationIndex = 0;

            Console.WriteLine("\nThis might take some time, please wait...");

            while (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Pinging IPs {loadingAnimation[animationIndex++ % loadingAnimation.Length]}");
                Thread.Sleep(100);
            }

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); // Clear the loading line
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
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
