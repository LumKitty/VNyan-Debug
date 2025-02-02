using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace Debug_UI
{
    internal static class Debug_UI
    {
        private static string Version = "0.4-alpha";
        private static string LineInput = "";
        private const string DebugPipeName = "uk.lum.vnyan-debug.console";
        private const string LogFile = "D:\\Dev\\VNyan-Debug-Console.log"; // TODO: DONT FUCKING RELEASE LIKE THIS

        public const string DefaultSettingsFile = "\\Lum-Debug-Settings.json";
        public static string ProfilePath = "";
        public static TextWriter StdErr = Console.Error;

        // For setting always on top
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        int uFlags);

        private const int HWND_TOPMOST = -1;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;
        //


        static void Log(string message)
        {
            if (LogFile.ToString().Length > 0)
            {
                System.IO.File.AppendAllText(LogFile, message + "\r\n");
            }
            Console.WriteLine(message);
        }


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {

            }
            else
            {
                if (args[0] == "/console")
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Title = "LumKitty's Debug console v" + Version;
                        Log("LumKitty's Debug console v" + Version + " - https://github.com/LumKitty - https://twitch.tv/LumKitty");
                        // SetConsoleCtrlHandler(Handler, true);

                        IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
                        SetWindowPos(hWnd, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);


                        Log("Creating pipe");
                        NamedPipeClientStream DebugPipe = new NamedPipeClientStream(".", DebugPipeName, PipeDirection.In);
                        Log("Connecting to pipe");
                        DebugPipe.Connect();
                        Log("Connected, creating stream");
                        StreamReader DebugStreamReader = new StreamReader(DebugPipe);
                        Log("Stream connected, waiting for input");

                        while (true)
                        {
                            if (DebugStreamReader.Peek() > 0)
                            {
                                LineInput = DebugStreamReader.ReadLine();
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
                                        case "QUIT":
                                            Application.Exit();
                                            break;
                                    }
                                }
                                Log(LineInput);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(ex.ToString());
                        throw;
                    }
                }
                else
                {
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
}