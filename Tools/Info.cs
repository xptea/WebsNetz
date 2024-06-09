using System;

namespace WebsNetz.Tools
{
    public static class Info
    {
        public static void Run()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("WebsNetz Software Information");
            Console.WriteLine("=============================");
            Console.ResetColor();

            // Display the information about the software
            Console.WriteLine("Name: WebsNetz");
            Console.WriteLine("Version: 1.0.1");
            Console.WriteLine("Description: WebsNetz is a network diagnostics and information tool.");
            Console.WriteLine();

            Console.WriteLine("Developer: Xptea");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("https://github.com/xptea");


            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("For more information, visit our GitHub page:");
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("https://github.com/xptea/WebsNetz");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}
