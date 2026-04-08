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
        ["Recruit Spearmen"] = UnitType.SpearmenLvl1,
        ["Recruit Fighter"] = UnitType.Fighter
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
        ConsoleTextComponent.Write($"{label}: {value}", ConsoleColor.Blue);

        if (increase > 0)
        {
            ConsoleTextComponent.Write($" [+{increase}]", ConsoleColor.Green);
        }

        ConsoleTextComponent.NewLine();
    }

    private static void WriteUnitCountLine(GameWorld gameWorld, UnitType unitType, int increase)
    {
        gameWorld.PlayerUnits.TryGetValue(unitType, out var count);
        if (count <= 0 && increase <= 0)
        {
            return;
        }

        var countDisplay = RenderUnitCountHistory(gameWorld, unitType, count);

        ConsoleTextComponent.Write($"{UnitTypeDisplayNames.Get(unitType)}: {countDisplay}", ConsoleColor.Blue);

        if (increase > 0)
        {
            ConsoleTextComponent.Write($" [+{increase}]", ConsoleColor.Green);
        }

        ConsoleTextComponent.NewLine();
    }

    private static string RenderUnitCountHistory(GameWorld gameWorld, UnitType unitType, int currentCount)
    {
        if (gameWorld.PlayerUnitHistory.Count == 0)
        {
            return currentCount.ToString();
        }

        var unitCounts = gameWorld.PlayerUnitHistory
            .Select(snapshot => snapshot.TryGetValue(unitType, out var count) ? count : 0)
            .ToArray();

        return string.Join(" -> ", unitCounts);
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
