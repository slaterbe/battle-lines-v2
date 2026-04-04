using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Views.Components;

public class GameHeaderComponent
{
    public void Render(GameWorld gameWorld, string statusMessage, ConsoleColor statusColor, GameCommandCost? selectedCommandCost)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        ConsoleTextComponent.WriteLine("Battle Lines", ConsoleColor.White);
        RenderResourceLine(gameWorld, selectedCommandCost);
        ConsoleTextComponent.WriteLine(new string('=', 80));
        ConsoleTextComponent.WriteLine(statusMessage, statusColor);
    }

    private static void RenderResourceLine(GameWorld gameWorld, GameCommandCost? selectedCommandCost)
    {
        WriteResource("Villagers", gameWorld.Villagers, selectedCommandCost?.Villagers ?? 0);
        Console.Write("    ");
        WriteResource("Spears", gameWorld.Spears, selectedCommandCost?.Spears ?? 0);
        Console.Write("    ");
        WriteResource("Gold", gameWorld.Gold, selectedCommandCost?.Gold ?? 0);
        Console.Write($"    State: {gameWorld.State}        ");
        Console.WriteLine();
    }

    private static void WriteResource(string label, int amount, int cost)
    {
        Console.Write($"{label}: {amount}");
        if (cost > 0)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" [-{cost}]");
            Console.ForegroundColor = originalColor;
        }
    }
}
