using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

            if (string.IsNullOrEmpty(domain))
            {
                Console.WriteLine("\nInvalid domain name. Please try again.");
                ReturnToMenu();
                return;
            }

            Thread loadingThread = new Thread(ShowLoadingScreen);
            loadingThread.Start();

            DisplayWhoisInfo(domain);

            loadingThread.Abort();
            Console.WriteLine("\n\nWhois lookup completed.");
            ReturnToMenu();
        }

        private static void ShowLoadingScreen()
        {
            string[] loadingAnimation = { "/", "-", "\\", "|" };
            int animationIndex = 0;

            Console.WriteLine("\nThis might take some time, please wait...");

            while (true)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Performing Whois lookup {loadingAnimation[animationIndex++ % loadingAnimation.Length]}");
                Thread.Sleep(100);
            }
        }

        private static void DisplayWhoisInfo(string domain)
        {
            try
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
                            Console.SetCursorPosition(0, Console.CursorTop);
                            Console.WriteLine("\n\nWhois Lookup Result:\n");
                            string response;
                            while ((response = reader.ReadLine()) != null)
                            {
                                Console.WriteLine(response);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
