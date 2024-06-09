using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebsNetz.Tools
{
    class SpeedTestTool
    {
        private const string TestFileUrl = "http://speedtest.tele2.net/10MB.zip";
        private const string UploadTestUrl = "http://speedtest.tele2.net/upload.php";

        public static void Run()
        {
            Console.Clear();
            Program.DisplayHeader();
            Task.Run(async () =>
            {
                double downloadSpeed = await TestDownloadSpeed();
                double uploadSpeed = await TestUploadSpeed();
                DisplayResults(downloadSpeed, uploadSpeed);
                ReturnToMenu();
            }).Wait();
        }

        private static async Task<double> TestDownloadSpeed()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Stopwatch sw = new Stopwatch();
                    Console.Write("Testing download speed: ");
                    sw.Start();
                    var data = await client.GetByteArrayAsync(TestFileUrl);
                    sw.Stop();

                    double speed = (data.Length * 8) / (sw.Elapsed.TotalSeconds * 1024 * 1024); // Convert to Mbps
                    Console.WriteLine($"{Math.Round(speed, 2)} Mbps");
                    return speed;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                return 0;
            }
        }

        private static async Task<double> TestUploadSpeed()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] data = new byte[10485760]; // 10MB
                    Stopwatch sw = new Stopwatch();
                    Console.Write("Testing upload speed: ");
                    sw.Start();
                    var response = await client.PostAsync(UploadTestUrl, new ByteArrayContent(data));
                    sw.Stop();

                    double speed = (data.Length * 8) / (sw.Elapsed.TotalSeconds * 1024 * 1024); // Convert to Mbps
                    Console.WriteLine($"{Math.Round(speed, 2)} Mbps");
                    return speed;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                return 0;
            }
        }

        private static void DisplayResults(double downloadSpeed, double uploadSpeed)
        {
            Console.Clear();
            Program.DisplayHeader();
            if (downloadSpeed > 0)
            {
                Console.WriteLine($"Download Speed: {Math.Round(downloadSpeed, 2)} Mbps");
            }
            else
            {
                Console.WriteLine("Failed to measure download speed.");
            }

            if (uploadSpeed > 0)
            {
                Console.WriteLine($"Upload Speed: {Math.Round(uploadSpeed, 2)} Mbps");
            }
            else
            {
                Console.WriteLine("Failed to measure upload speed.");
            }
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}
