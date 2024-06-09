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
        private static readonly string NewVersionExecutableUrl = "https://github.com/xptea/WebsNetz/raw/main/WebsNetz.exe"; // Change this to your actual URL

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
                    Environment.Exit(0); // Exit without requiring user input
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(0); // Exit without requiring user input
            }
        }

        private static async Task<string> GetLatestVersionFromGitHub()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30); // Increase timeout to 30 seconds

                try
                {
                    // Add a cache-busting query parameter to the URL
                    string requestUrl = GitHubVersionUrl + "?t=" + DateTime.UtcNow.Ticks;
                    HttpResponseMessage response = await client.GetAsync(requestUrl);

                    response.EnsureSuccessStatusCode();

                    string version = await response.Content.ReadAsStringAsync();
                    version = version.Trim();

                    // Check if the version string seems valid (e.g., starts with "v" and contains digits)
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
                    return LocalVersion; // Return local version in case of error
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

                    // Move the old executable to a temporary location
                    string oldExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
                    string oldExecutableTempPath = Path.Combine(Path.GetTempPath(), "WebsNetz_Old.exe");

                    File.Move(oldExecutablePath, oldExecutableTempPath);

                    // Start the new version
                    Process newProcess = Process.Start(new ProcessStartInfo(tempFilePath)
                    {
                        UseShellExecute = true
                    });

                    if (newProcess != null)
                    {
                        // Wait for the new process to be ready for user input
                        newProcess.WaitForInputIdle();

                        // Ensure the new process has started properly
                        bool newProcessStarted = false;
                        for (int i = 0; i < 10; i++)
                        {
                            if (newProcess.HasExited)
                            {
                                newProcessStarted = false;
                                break;
                            }
                            newProcessStarted = true;
                            await Task.Delay(1000); // Wait for 1 second
                        }

                        if (newProcessStarted)
                        {
                            // Schedule the old executable for deletion
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    // Give the new process some more time to stabilize
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

                            Environment.Exit(0); // Exit the current application
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
                    Environment.Exit(0); // Exit the current application immediately on error
                }
            }
        }
    }
}
