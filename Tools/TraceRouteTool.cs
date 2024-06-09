using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WebsNetz.Tools
{
    class TraceRouteTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            Console.Write("Enter IP Address or Hostname: ");
            string target = Console.ReadLine();

            if (!IsValidTarget(target))
            {
                Console.WriteLine("\nInvalid IP Address or Hostname. Please try again.");
                ReturnToMenu();
                return;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("tracert", target)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process
                {
                    StartInfo = startInfo
                };

                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Console.WriteLine(e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Console.WriteLine($"Error: {e.Data}");
                    }
                };

                Console.WriteLine("\nStarting traceroute...\n");

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                Console.WriteLine("\nTraceroute completed.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            ReturnToMenu();
        }

        static bool IsValidTarget(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return false;
            }

            string ipPattern = @"^(\d{1,3}\.){3}\d{1,3}$";
            string hostnamePattern = @"^([a-zA-Z0-9]+(-[a-zA-Z0-9]+)*\.)+[a-zA-Z]{2,}$";

            return Regex.IsMatch(target, ipPattern) || Regex.IsMatch(target, hostnamePattern);
        }

        static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
