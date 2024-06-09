using System;
using System.Net;

namespace WebsNetz.Tools
{
    class DNSTool
    {
        public static void Run()
        {
            Console.Clear();
                Program.DisplayHeader(); 
            Console.Write("Enter domain name: ");
            string domainName = Console.ReadLine();

            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(domainName);
                Console.WriteLine($"\nIP Addresses for {domainName}:");

                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    Console.WriteLine(ip);
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
