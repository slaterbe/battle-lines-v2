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
        var selectedCommandCost =
            selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
                ? commandOptions[selectedCommandIndex].Cost
                : null;

        Layout.Render(
            gameWorld,
            "The village waits for your command. Prepare the defenses.",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex,
            supplementalDetailsRenderer: () => RenderSupplementalDetails(gameWorld, selectedCommandLabel, selectedCommandCost),
            playerUnitsRenderer: () => PlayerUnits.Render(gameWorld, selectedCommandLabel),
            showResources: false,
            showWaveOverview: false,
            showCurrentWave: false);
    }

    private static void RenderSupplementalDetails(
        GameWorld gameWorld,
        string selectedCommandLabel,
        GameCommandCost? selectedCommandCost)
    {
        ConsoleTextComponent.WriteLine("-------------------------------------", ConsoleColor.DarkGray);
        ConsoleTextComponent.WriteLine("Resource   | Storage         Production", ConsoleColor.DarkYellow);
        ConsoleTextComponent.WriteLine("---------------------------------------", ConsoleColor.DarkGray);

        WriteVillageResourceRow(
            "Villagers",
            gameWorld.Villagers,
            selectedCommandCost?.Villagers ?? 0,
            gameWorld.VillagerProduction,
            selectedCommandLabel == "Boost Villagers");

        if (gameWorld.IsSpearControlsVisible)
        {
            WriteVillageResourceRow(
                "Spears",
                gameWorld.Spears,
                selectedCommandCost?.Spears ?? 0,
                gameWorld.SpearProduction,
                selectedCommandLabel == "Boost Spears");
        }

        if (gameWorld.IsUpgradesVisible)
        {
            WriteVillageResourceRow(
                "Gold",
                gameWorld.Gold,
                selectedCommandCost?.Gold ?? 0,
                null,
                false);
        }

        Console.WriteLine();
        WriteVillageStatLine("Max Army Size", gameWorld.MaxArmySize.ToString(), selectedCommandLabel == "Boost Army Size");
    }

    private static void WriteVillageResourceRow(
        string label,
        int storedAmount,
        int storageCost,
        int? productionAmount,
        bool showProductionIncrease)
    {
        ConsoleTextComponent.Write($"{label,-10} | ", ConsoleColor.Cyan);
        ConsoleTextComponent.Write($"{storedAmount,3}", ConsoleColor.Gray);

        if (storageCost > 0)
        {
            ConsoleTextComponent.Write($" [-{storageCost}]", ConsoleColor.Red);
        }
        else
        {
            ConsoleTextComponent.Write("      ");
        }

        ConsoleTextComponent.Write("     ");

        if (productionAmount.HasValue)
        {
            ConsoleTextComponent.Write($"{("+" + productionAmount.Value),5}", ConsoleColor.Green);
        }
        else
        {
            ConsoleTextComponent.Write($"{ "--",5}", ConsoleColor.DarkGray);
        }

        if (showProductionIncrease)
        {
            ConsoleTextComponent.Write(" [+1]", ConsoleColor.Green);
        }

        Console.WriteLine();
    }

    private static void WriteVillageStatLine(string label, string value, bool showIncrease)
    {
        ConsoleTextComponent.Write($"{label}: {value}", ConsoleColor.Cyan);

        if (showIncrease)
        {
            ConsoleTextComponent.Write(" [+1]", ConsoleColor.Green);
        }

        Console.WriteLine();
    }
}
