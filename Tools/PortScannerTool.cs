using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebsNetz.Tools
{
    class PortScannerTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            Console.Write("Enter IP Address or Hostname: ");
            string target = Console.ReadLine();

            Console.Write("Enter starting port: ");
            int startPort = int.Parse(Console.ReadLine());

            Console.Write("Enter ending port: ");
            int endPort = int.Parse(Console.ReadLine());

            Console.Clear();
            Program.DisplayHeader();
            ScanPorts(target, startPort, endPort).Wait();

            ReturnToMenu();
        }

        private static async Task ScanPorts(string target, int startPort, int endPort)
        {
            for (int port = startPort; port <= endPort; port++)
            {
                if (await IsPortOpen(target, port))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Port {port}: Open");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Port {port}: Closed");
                }
            }

            Console.ResetColor();
        }

        private static async Task<bool> IsPortOpen(string target, int port)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(target, port);
                    return true;
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
