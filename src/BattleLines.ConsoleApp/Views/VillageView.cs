using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class VillageView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();
    private static readonly PlayerUnitsComponent PlayerUnits = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var selectedCommandLabel =
            selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
                ? commandOptions[selectedCommandIndex].Label
                : string.Empty;

        Layout.Render(
            gameWorld,
            "The village waits for your command. Prepare the defenses.",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex,
            supplementalDetailsRenderer: () => RenderSupplementalDetails(gameWorld, selectedCommandLabel),
            playerUnitsRenderer: () => PlayerUnits.Render(gameWorld, selectedCommandLabel),
            showWaveOverview: false,
            showCurrentWave: false);
    }

    private static void RenderSupplementalDetails(GameWorld gameWorld, string selectedCommandLabel)
    {
        WriteVillageDetailLine("Villager Production", $"+{gameWorld.VillagerProduction}", selectedCommandLabel == "Boost Villagers");

        if (gameWorld.IsSpearControlsVisible)
        {
            WriteVillageDetailLine("Spear Production", $"+{gameWorld.SpearProduction}", selectedCommandLabel == "Boost Spears");
        }

        WriteVillageDetailLine("Max Army Size", gameWorld.MaxArmySize.ToString(), selectedCommandLabel == "Boost Army Size");
    }

    private static void WriteVillageDetailLine(string label, string value, bool showIncrease)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{label}: {value}");

        if (showIncrease)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" [+1]");
            Console.ForegroundColor = originalColor;
        }

        Console.WriteLine();
        Console.ResetColor();
    }
}
