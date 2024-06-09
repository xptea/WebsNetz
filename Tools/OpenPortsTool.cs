using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace WebsNetz.Tools
{
    class OpenPortsTool
    {
        public static void Run()
        {
            while (true)
            {
                Console.Clear();
                Program.DisplayHeader();
                Console.WriteLine("1. Show Open Ports");
                Console.WriteLine("2. Show Closed Ports");
                Console.WriteLine("3. Return to Main Menu");

                var choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        ShowOpenPorts();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ShowClosedPorts();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Please choose a valid option.");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        private static void ShowOpenPorts()
        {
            Console.Clear();
            Program.DisplayHeader();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndpoints = properties.GetActiveTcpListeners();
            IPEndPoint[] udpEndpoints = properties.GetActiveUdpListeners();
            TcpConnectionInformation[] tcpConnections = properties.GetActiveTcpConnections();

            var openTcpPorts = tcpConnections.Select(c => c.LocalEndPoint.Port).Distinct().ToList();
            var openUdpPorts = udpEndpoints.Select(e => e.Port).Distinct().ToList();
            var openPorts = openTcpPorts.Union(openUdpPorts).Distinct().OrderBy(p => p).ToList();

            Console.WriteLine("Port\tStatus");
            Console.WriteLine(new string('-', 20));

            foreach (int port in openPorts)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{port}\tOpen");
            }

            Console.ResetColor();
            ReturnToMenu();
        }

        private static void ShowClosedPorts()
        {
            Console.Clear();
            Program.DisplayHeader();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndpoints = properties.GetActiveTcpListeners();
            IPEndPoint[] udpEndpoints = properties.GetActiveUdpListeners();
            TcpConnectionInformation[] tcpConnections = properties.GetActiveTcpConnections();

            var openTcpPorts = tcpConnections.Select(c => c.LocalEndPoint.Port).Distinct().ToList();
            var openUdpPorts = udpEndpoints.Select(e => e.Port).Distinct().ToList();
            var openPorts = openTcpPorts.Union(openUdpPorts).Distinct().ToList();

            int[] allPorts = Enumerable.Range(1, 65535).ToArray();
            List<int> closedPorts = allPorts.Except(openPorts).OrderBy(p => p).ToList();

            Console.WriteLine("Port\tStatus");
            Console.WriteLine(new string('-', 20));

            Console.ForegroundColor = ConsoleColor.Red;
            DisplayClosedPortRanges(closedPorts);

            Console.ResetColor();
            ReturnToMenu();
        }

        private static void DisplayClosedPortRanges(List<int> closedPorts)
        {
            if (closedPorts.Count == 0)
                return;

            closedPorts.Sort();
            int rangeStart = closedPorts[0];
            int rangeEnd = closedPorts[0];

            for (int i = 1; i < closedPorts.Count; i++)
            {
                if (closedPorts[i] == rangeEnd + 1)
                {
                    rangeEnd = closedPorts[i];
                }
                else
                {
                    if (rangeStart == rangeEnd)
                    {
                        Console.WriteLine($"{rangeStart}\tClosed");
                    }
                    else
                    {
                        Console.WriteLine($"{rangeStart}-{rangeEnd}\tClosed");
                    }
                    rangeStart = closedPorts[i];
                    rangeEnd = closedPorts[i];
                }
            }

            if (rangeStart == rangeEnd)
            {
                Console.WriteLine($"{rangeStart}\tClosed");
            }
            else
            {
                Console.WriteLine($"{rangeStart}-{rangeEnd}\tClosed");
            }
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
