using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WebsNetz.Tools
{
    class WhoisLookupTool
    {
        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            Console.Write("Enter domain name: ");
            string domain = Console.ReadLine();

            DisplayWhoisInfo(domain);

            ReturnToMenu();
        }

        private static void DisplayWhoisInfo(string domain)
        {
            string whoisServer = "whois.verisign-grs.com";

            using (TcpClient whoisClient = new TcpClient(whoisServer, 43))
            {
                using (NetworkStream stream = whoisClient.GetStream())
                {
                    byte[] domainQuery = System.Text.Encoding.ASCII.GetBytes(domain + "\r\n");
                    stream.Write(domainQuery, 0, domainQuery.Length);

                    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.ASCII))
                    {
                        string response = reader.ReadToEnd();
                        Console.WriteLine(response);
                    }
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
