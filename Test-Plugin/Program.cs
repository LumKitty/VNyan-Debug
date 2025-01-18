// See https://aka.ms/new-console-template for more information
using System.Diagnostics;


StreamWriter DebugStreamWriter = null;
string ConsolePath = "D:\\Twitch\\Software\\VNyan\\Items\\Assemblies\\Debug-Console.exe";
using (Process DebugProcess = new Process())
{
    DebugProcess.StartInfo.FileName = ConsolePath;
    DebugProcess.StartInfo.UseShellExecute = false;
    DebugProcess.StartInfo.RedirectStandardInput = true;
    DebugProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
    DebugProcess.StartInfo.CreateNoWindow = false;
    Console.WriteLine("Starting Process");
    DebugProcess.Start();

    DebugStreamWriter = DebugProcess.StandardInput;

    string text = "lemon";
    DebugStreamWriter.WriteLine(text);
    text = Console.ReadLine();
}