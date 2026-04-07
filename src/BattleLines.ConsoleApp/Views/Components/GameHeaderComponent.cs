using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Views.Components;

public class GameHeaderComponent
{
    public void Render(
        GameWorld gameWorld,
        string statusMessage,
        ConsoleColor statusColor,
        GameCommandCost? selectedCommandCost,
        string selectedCommandLabel,
        bool showResources = true)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        ConsoleTextComponent.WriteLine("Battle Lines", ConsoleColor.White);

        if (showResources)
        {
            RenderResourceLine(gameWorld, selectedCommandCost, selectedCommandLabel);
        }

        ConsoleTextComponent.WriteLine(new string('=', 80));
        ConsoleTextComponent.WriteLine(statusMessage, statusColor);
    }

    private static void RenderResourceLine(GameWorld gameWorld, GameCommandCost? selectedCommandCost, string selectedCommandLabel)
    {
        WriteResource(
            "Villagers",
            gameWorld.Villagers,
            selectedCommandCost?.Villagers ?? 0,
            selectedCommandLabel == "Boost Villagers" ? 1 : 0);

        if (gameWorld.IsSpearControlsVisible)
        {
            Console.Write("    ");
            WriteResource(
                "Spears",
                gameWorld.Spears,
                selectedCommandCost?.Spears ?? 0,
                selectedCommandLabel == "Boost Spears" ? 1 : 0);
        }

        if (gameWorld.IsUpgradesVisible)
        {
            Console.Write("    ");
            WriteResource("Gold", gameWorld.Gold, selectedCommandCost?.Gold ?? 0, 0);
        }

        Console.WriteLine();
    }

    private static void WriteResource(string label, int amount, int cost, int increase)
    {
        Console.Write($"{label}: {amount}");
        if (cost > 0)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" [-{cost}]");
            Console.ForegroundColor = originalColor;
        }

        if (increase > 0)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" [+{increase}]");
            Console.ForegroundColor = originalColor;
        }
    }
}
