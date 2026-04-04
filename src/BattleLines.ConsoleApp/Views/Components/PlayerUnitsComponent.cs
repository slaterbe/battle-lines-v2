using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class PlayerUnitsComponent
{
    public void Render(GameWorld gameWorld, string selectedCommandLabel = "")
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
