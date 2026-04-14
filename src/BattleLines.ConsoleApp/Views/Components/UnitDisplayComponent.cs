using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Views.Components;

public static class UnitDisplayComponent
{
    private static readonly PlayerArmyBattleService PlayerArmyBattleService = new();
    private static readonly UnitType[] PlayerBattleLineOrder =
    [
        UnitType.Fighter,
        UnitType.SpearmenLvl1
    ];

    public static string RenderArmyCount(GameWorld gameWorld)
    {
        return RenderArmyPositions(gameWorld);
    }

    public static string RenderUnitCount(GameWorld gameWorld, UnitType unitType, int count)
    {
        return unitType switch
        {
            UnitType.SpearmenLvl1 => RenderArmyPositions(gameWorld),
            UnitType.GiantRat => RenderEnemyPositions(gameWorld, unitType, count),
            _ => count.ToString()
        };
    }

    private static string RenderArmyPositions(GameWorld gameWorld)
    {
        var clampedMaxPositions = Math.Max(0, gameWorld.FrontLineCapacity);
        var totalArmySize = gameWorld.PlayerUnits.Values.Sum();
        var displayedCount = Math.Clamp(totalArmySize, 0, clampedMaxPositions);

        if (gameWorld.State != GameState.Battle &&
            !((gameWorld.State == GameState.PostWave || gameWorld.State == GameState.PostBattle) &&
              gameWorld.HasPendingPostBattleResolution))
        {
            return $"{RenderUnits(gameWorld.PlayerUnits, PlayerBattleLineOrder)}{new string('O', clampedMaxPositions - displayedCount)}";
        }

        var survivingUnitsByType = PlayerArmyBattleService.CalculateSurvivingUnits(gameWorld);
        var survivingArmySize = survivingUnitsByType.Values.Sum();
        var armySizeAtBattleStart = gameWorld.PlayerUnitsAtBattleStart.Values.Sum();
        var survivingUnits = Math.Clamp(survivingArmySize, 0, clampedMaxPositions);
        var unitsLost = Math.Clamp(armySizeAtBattleStart - survivingArmySize, 0, clampedMaxPositions - survivingUnits);
        var emptyPositions = Math.Max(0, clampedMaxPositions - armySizeAtBattleStart);

        return $"{RenderUnits(survivingUnitsByType, PlayerBattleLineOrder)}{new string('X', unitsLost)}{new string('O', emptyPositions)}";
    }

    private static string RenderEnemyPositions(GameWorld gameWorld, UnitType unitType, int count)
    {
        var displayedCount = Math.Max(0, count);

        if (gameWorld.State != GameState.Battle &&
            !((gameWorld.State == GameState.PostWave || gameWorld.State == GameState.PostBattle) &&
              gameWorld.HasPendingPostBattleResolution))
        {
            return RenderUnit(unitType, displayedCount);
        }

        if (!UnitCatalog.DefaultUnits.TryGetValue(unitType, out var unitModel) || unitModel.Health <= 0)
        {
            return RenderUnit(unitType, displayedCount);
        }

        var healthLost = Math.Max(0, displayedCount * unitModel.Health - gameWorld.CurrentWaveTotalHealth);
        var unitsLost = Math.Min(displayedCount, healthLost / unitModel.Health);
        var survivingUnits = Math.Max(0, displayedCount - unitsLost);

        return $"{RenderUnit(unitType, survivingUnits)}{new string('X', unitsLost)}";
    }

    private static string RenderUnits(IReadOnlyDictionary<UnitType, int> unitCounts, IReadOnlyList<UnitType> unitOrder)
    {
        return string.Concat(unitOrder.Select(unitType =>
            unitCounts.TryGetValue(unitType, out var count)
                ? RenderUnit(unitType, count)
                : string.Empty));
    }

    private static string RenderUnit(UnitType unitType, int count)
    {
        if (count <= 0)
        {
            return string.Empty;
        }

        var unitAcronym = UnitCatalog.DefaultUnits.TryGetValue(unitType, out var unitModel) && unitModel.UnitAcronym != default
            ? unitModel.UnitAcronym
            : '|';

        return new string(unitAcronym, count);
    }
}
