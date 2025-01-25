using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Windows.Forms;

class Debug_Console
{
    private static string Version = "0.2-alpha";
    private static string LineInput = "";

    /* [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

    // https://learn.microsoft.com/en-us/windows/console/handlerroutine?WT.mc_id=DT-MVP-5003978
    private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);

    private enum CtrlType
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6
    }
    private static bool Handler(CtrlType signal)
    {
        switch (signal)
        {
            case CtrlType.CTRL_BREAK_EVENT:
            case CtrlType.CTRL_C_EVENT:
            case CtrlType.CTRL_LOGOFF_EVENT:
            case CtrlType.CTRL_SHUTDOWN_EVENT:
            case CtrlType.CTRL_CLOSE_EVENT:
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine("EXIT");
                Environment.Exit(0);
                return false;

            default:
                return false;
        }
    } */

    static void Main()
    {
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
                    }
                }
                Console.WriteLine(LineInput);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
