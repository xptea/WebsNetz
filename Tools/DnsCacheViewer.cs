using System;
using System.Diagnostics;

namespace WebsNetz.Tools
{
    class DnsCacheViewer
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            DisplayDnsCache();

            ReturnToMenu();
        }

        private static void DisplayDnsCache()
        {
            ProcessStartInfo psi = new ProcessStartInfo("ipconfig", "/displaydns");
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            Process p = Process.Start(psi);
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            Console.WriteLine(output);
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
