using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class PlayerUnitsComponent
{
    private static readonly UnitType[] UnitDisplayOrder =
    [
        UnitType.Fighter,
        UnitType.SpearmenLvl1
    ];

    private static readonly IReadOnlyDictionary<string, UnitType> CommandUnitPreviews = new Dictionary<string, UnitType>
    {
        ["Add Spearmen"] = UnitType.SpearmenLvl1,
        ["Add Fighter"] = UnitType.Fighter
    };

    public void Render(GameWorld gameWorld, string selectedCommandLabel = "")
    {
        TryGetPreviewUnitModel(selectedCommandLabel, out var previewUnitModel);
        var previewUnitType = GetPreviewUnitType(selectedCommandLabel);

        ConsoleTextComponent.WriteLine(
            $"Army: {UnitDisplayComponent.RenderArmyCount(gameWorld)}",
            ConsoleColor.Blue);

        foreach (var unitType in UnitDisplayOrder)
        {
            WriteUnitCountLine(gameWorld, unitType, previewUnitType == unitType ? 1 : 0);
        }

        ConsoleTextComponent.WriteLine("---", ConsoleColor.Blue);

        WritePlayerStatLine("Health", BattleHistoryComponent.RenderPlayerHealth(gameWorld), previewUnitModel.Health);
        WritePlayerStatLine("Attack", BattleHistoryComponent.RenderPlayerAttack(gameWorld), previewUnitModel.Attack);
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

    private static void WriteUnitCountLine(GameWorld gameWorld, UnitType unitType, int increase)
    {
        gameWorld.PlayerUnits.TryGetValue(unitType, out var count);
        if (count <= 0 && increase <= 0)
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{UnitTypeDisplayNames.Get(unitType)}: {count}");

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

    private static UnitType? GetPreviewUnitType(string selectedCommandLabel)
    {
        return CommandUnitPreviews.TryGetValue(selectedCommandLabel, out var unitType)
            ? unitType
            : null;
    }
}
