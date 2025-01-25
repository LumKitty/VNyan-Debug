namespace Debug_UI
{
    internal static class Debug_UI
    {
        private static string Version = "0.3-alpha";
        private static string LineInput = "";
        public static string ProfilePath = "";
        public static TextWriter StdErr = Console.Error;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Title = "LumKitty's Debug console v" + Version;
                Console.WriteLine("LumKitty's Debug console v" + Version + " - https://github.com/LumKitty - https://twitch.tv/LumKitty");
                // SetConsoleCtrlHandler(Handler, true);
                while (true)
                {
                    LineInput = Console.ReadLine();
                    if (LineInput != null)
                    {
                        if (LineInput.Length >= 4)
                        {
                            switch (LineInput.Substring(0, 4))
                            {
                                case "LOG:":
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    break;
                                case "ERR:":
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    break;
                                case "TRG:":
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    break;
                                case "SYS:":
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    break;
                                /*case "UIW:":
                                    ProfilePath = LineInput.Substring(4);
                                    
                                    ApplicationConfiguration.Initialize();
                                    Application.Run(new Form1());
                                    break;*/

                            }
                        }
                        Console.WriteLine(LineInput);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            } else {
                foreach (string arg in args)
                {
                    ProfilePath += arg + " ";
                }
                ProfilePath = ProfilePath.Trim();
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
                Console.WriteLine("UIEXIT::");
                Thread.Sleep(200);
            }
       
        }
    }
}