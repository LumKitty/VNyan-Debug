string Version = "0.1-alpha";
Console.Title = "LumKitty's Debug console v" + Version;
Console.WriteLine("LumKitty's Debug console v"+Version+" - https://github.com/LumKitty - https://twitch.tv/LumKitty");
String LineInput;
while (true)
{
    LineInput = Console.ReadLine();
    if (LineInput != null)
    {
        if (LineInput.Length >= 4)
        {
            switch (LineInput.Substring(0,4))
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
