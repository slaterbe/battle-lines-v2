namespace BattleLines.ConsoleApp.Views.Components;

public static class ConsoleTextComponent
{
    public static void Write(string text, ConsoleColor color = ConsoleColor.Gray)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }

    public static void WriteLine(string text, ConsoleColor color = ConsoleColor.Gray)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = originalColor;
    }

    public static void WriteHighlighted(
        string text,
        ConsoleColor foregroundColor = ConsoleColor.Black,
        ConsoleColor backgroundColor = ConsoleColor.Yellow)
    {
        var originalForegroundColor = Console.ForegroundColor;
        var originalBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        Console.Write(text);
        Console.ForegroundColor = originalForegroundColor;
        Console.BackgroundColor = originalBackgroundColor;
    }

    public static void WriteLineSlow(string text, ConsoleColor color = ConsoleColor.Gray, int characterDelayMs = 20)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        foreach (var character in text)
        {
            Console.Write(character);
            Thread.Sleep(characterDelayMs);
        }

        Console.WriteLine();
        Console.ForegroundColor = originalColor;
    }
}
