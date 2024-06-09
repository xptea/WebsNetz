using System;
using System.Diagnostics;

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

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("tracert", target)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process
                {
                    StartInfo = startInfo
                };
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
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
