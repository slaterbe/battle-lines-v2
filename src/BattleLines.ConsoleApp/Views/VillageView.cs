using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class VillageView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var selectedCommandLabel =
            selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
                ? commandOptions[selectedCommandIndex].Label
                : string.Empty;

        Layout.Render(
            gameWorld,
            "Village: Choose upgrades or start a battle",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex,
            supplementalDetailsRenderer: () => RenderSupplementalDetails(gameWorld, selectedCommandLabel),
            playerUnitsRenderer: () => RenderPlayerUnits(gameWorld, selectedCommandLabel),
            showWaveOverview: false,
            showCurrentWave: false);
    }

    private static void RenderSupplementalDetails(GameWorld gameWorld, string selectedCommandLabel)
    {
        WriteVillageDetailLine("Villager Production", $"+{gameWorld.VillagerProduction}", selectedCommandLabel == "Boost Villagers");
        WriteVillageDetailLine("Spear Production", $"+{gameWorld.SpearProduction}", selectedCommandLabel == "Boost Spears");
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

    private static void RenderPlayerUnits(GameWorld gameWorld, string selectedCommandLabel)
    {
        if (gameWorld.PlayerUnits.Count == 0)
        {
            ConsoleTextComponent.WriteLine("No player units.", ConsoleColor.Blue);
            return;
        }

        foreach (var playerUnit in gameWorld.PlayerUnits)
        {
            ConsoleTextComponent.WriteLine(
                $"{playerUnit.Key}: {UnitDisplayComponent.RenderUnitCount(gameWorld, playerUnit.Key, playerUnit.Value)}",
                ConsoleColor.Blue);
        }

        var healthIncrease = 0;
        var attackIncrease = 0;
        if (selectedCommandLabel == "Add Spearmen" &&
            UnitCatalog.DefaultUnits.TryGetValue(UnitType.SpearmenLvl1, out var spearmanModel))
        {
            healthIncrease = spearmanModel.Health;
            attackIncrease = spearmanModel.Attack;
        }

        WritePlayerStatLine("Health", BattleHistoryComponent.RenderPlayerHealth(gameWorld), healthIncrease);
        WritePlayerStatLine("Attack", BattleHistoryComponent.RenderPlayerAttack(gameWorld), attackIncrease);
    }

    private static void WritePlayerStatLine(string label, string value, int increase)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{label}: {value}");

        if (increase > 0)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" [+{increase}]");
            Console.ForegroundColor = originalColor;
        }

        Console.WriteLine();
        Console.ResetColor();
    }
}
