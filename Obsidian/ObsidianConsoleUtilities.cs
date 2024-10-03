namespace Obsidian;
public static class ObsidianConsoleUtilities
{
    /// <summary>
    /// Prints error messages in red.
    /// </summary>
    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Prints general messages in yellow.
    /// </summary>
    public static void PrintMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public static void PrintDelimiter() => Console.WriteLine("========================================================================");
}