using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class PlayerUnitsComponent
{
    private static readonly IReadOnlyDictionary<string, UnitType> CommandUnitPreviews = new Dictionary<string, UnitType>
    {
        ["Add Spearmen"] = UnitType.SpearmenLvl1,
        ["Add Fighter"] = UnitType.Fighter
    };

    public void Render(GameWorld gameWorld, string selectedCommandLabel = "")
    {
        if (gameWorld.PlayerUnits.Count == 0)
        {
            ConsoleTextComponent.WriteLine("No player units.", ConsoleColor.Blue);
            return;
        }

        foreach (var playerUnit in gameWorld.PlayerUnits)
        {
            if (playerUnit.Key == UnitType.Fighter)
            {
                continue;
            }

            ConsoleTextComponent.WriteLine(
                $"{playerUnit.Key}: {UnitDisplayComponent.RenderUnitCount(gameWorld, playerUnit.Key, playerUnit.Value)}",
                ConsoleColor.Blue);
        }

        var healthIncrease = 0;
        var attackIncrease = 0;
        if (TryGetPreviewUnitModel(selectedCommandLabel, out var previewUnitModel))
        {
            healthIncrease = previewUnitModel.Health;
            attackIncrease = previewUnitModel.Attack;
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

    private static bool TryGetPreviewUnitModel(string selectedCommandLabel, out UnitModel unitModel)
    {
        if (CommandUnitPreviews.TryGetValue(selectedCommandLabel, out var unitType) &&
            UnitCatalog.DefaultUnits.TryGetValue(unitType, out var previewUnitModel))
        {
            unitModel = previewUnitModel;
            return true;
        }

        unitModel = new UnitModel();
        return false;
    }
}
