using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebsNetz.Tools
{
    class Update
    {
        private static readonly string LocalVersion = "v1.0.1";
        private static readonly string GitHubVersionUrl = "https://raw.githubusercontent.com/xptea/WebsNetz/main/version.txt";
        private static readonly string GitHubReleaseUrl = "https://github.com/xptea/WebsNetz/releases";
        private static readonly string NewVersionExecutableUrl = "https://github.com/xptea/WebsNetz/raw/main/WebsNetz.exe"; 

        public static async Task Run()
        {
            Console.Clear();
            Program.DisplayHeader();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Local Version: {LocalVersion}");

            try
            {
                Console.WriteLine("Checking for updates...");
                string latestVersion = await GetLatestVersionFromGitHub();

                if (LocalVersion != latestVersion)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nAn update is available! Downloading the latest version...");
                    await DownloadAndRestart(NewVersionExecutableUrl);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nYou are using the latest version.");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(0);
            }
        }

        private static async Task<string> GetLatestVersionFromGitHub()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);

                try
                {
                    string requestUrl = GitHubVersionUrl + "?t=" + DateTime.UtcNow.Ticks;
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    response.EnsureSuccessStatusCode();

                    string version = await response.Content.ReadAsStringAsync();
                    version = version.Trim();

                    if (version.StartsWith("v") && version.Length > 1 && char.IsDigit(version[1]))
                    {
                        return version;
                    }
                    else
                    {
                        throw new Exception("Invalid version format received.");
                    }
                }
                catch
                {
                    return LocalVersion;
                }
            }
        }

        private static async Task DownloadAndRestart(string downloadUrl)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "WebsNetz_Update.exe");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(downloadUrl);
                    response.EnsureSuccessStatusCode();

                    using (FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }

                    string oldExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
                    string oldExecutableTempPath = Path.Combine(Path.GetTempPath(), "WebsNetz_Old.exe");

                    File.Move(oldExecutablePath, oldExecutableTempPath);

                    Process newProcess = Process.Start(new ProcessStartInfo(tempFilePath)
                    {
                        UseShellExecute = true
                    });

                    if (newProcess != null)
                    {
                        newProcess.WaitForInputIdle();

                        bool newProcessStarted = false;
                        for (int i = 0; i < 10; i++)
                        {
                            if (newProcess.HasExited)
                            {
                                newProcessStarted = false;
                                break;
                            }
                            newProcessStarted = true;
                            await Task.Delay(1000); 
                        }

                        if (newProcessStarted)
                        {
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    await Task.Delay(10000);
                                    File.Delete(oldExecutableTempPath);
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Failed to delete old executable: {ex.Message}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            });

                            Environment.Exit(0);
                        }
                        else
                        {
                            throw new Exception("The new process failed to start properly.");
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to start the new process.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to download and restart: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Environment.Exit(0);
                }
            }
        }
    }
}
