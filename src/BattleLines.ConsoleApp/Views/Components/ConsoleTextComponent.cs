namespace BattleLines.ConsoleApp.Views.Components;

public static class ConsoleTextComponent
{
    public static void WriteLine(string text, ConsoleColor color = ConsoleColor.Gray)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = originalColor;
    }
}
