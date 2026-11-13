namespace AssetTrackingSystem;

public static class Utilities
{
    public static void WriteLineWithColor(string s, ConsoleColor color)
    {
        var backup = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(s);
        Console.ForegroundColor = backup;
        
    }
    
    public static void WriteWithColor(string s, ConsoleColor color)
    {
        var backup = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(s);
        Console.ForegroundColor = backup;
        
    }
    
    public static void WritelnRed(string s) => WriteLineWithColor(s, ConsoleColor.Red);
    public static void WritelnGreen(string s) => WriteLineWithColor(s, ConsoleColor.Green);
    public static void WritelnYellow(string s) => WriteLineWithColor(s, ConsoleColor.Yellow);
    public static void WritelnBlue(string s) => WriteLineWithColor(s, ConsoleColor.Blue);
    public static void WritelnMagenta(string s) => WriteLineWithColor(s, ConsoleColor.Magenta);
    
    public static void WriteRed(string s) => WriteWithColor(s, ConsoleColor.Red);
    public static void WriteGreen(string s) => WriteWithColor(s, ConsoleColor.Green);
    public static void WriteYellow(string s) => WriteWithColor(s, ConsoleColor.Yellow);
    public static void WriteBlue(string s) => WriteWithColor(s, ConsoleColor.Blue);
    public static void WriteMagenta(string s) => WriteWithColor(s, ConsoleColor.Magenta);
}