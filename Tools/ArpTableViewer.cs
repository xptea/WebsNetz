using System;
using System.Diagnostics;

namespace WebsNetz.Tools
{
    class ArpTableViewer
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            DisplayArpTable();

            ReturnToMenu();
        }

        private static void DisplayArpTable()
        {
            ProcessStartInfo psi = new ProcessStartInfo("arp", "-a");
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
