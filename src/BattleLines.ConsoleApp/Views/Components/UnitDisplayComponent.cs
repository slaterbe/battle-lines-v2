using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Views.Components;

public static class UnitDisplayComponent
{
    private static readonly PlayerArmyBattleService PlayerArmyBattleService = new();

    public static string RenderUnitCount(GameWorld gameWorld, UnitType unitType, int count)
    {
        return unitType switch
        {
            UnitType.SpearmenLvl1 => RenderArmyPositions(gameWorld),
            UnitType.GiantRat => new string('|', Math.Max(0, count)),
            _ => count.ToString()
        };
    }

    private static string RenderArmyPositions(GameWorld gameWorld)
    {
        var clampedMaxPositions = Math.Max(0, gameWorld.MaxArmySize);
        var totalArmySize = gameWorld.PlayerUnits.Values.Sum();
        var displayedCount = Math.Clamp(totalArmySize, 0, clampedMaxPositions);

        if (gameWorld.State != GameState.Battle &&
            !((gameWorld.State == GameState.PostWave || gameWorld.State == GameState.PostBattle) &&
              gameWorld.HasPendingPostBattleResolution))
        {
            return $"{new string('|', displayedCount)}{new string('O', clampedMaxPositions - displayedCount)}";
        }

        var survivingArmySize = PlayerArmyBattleService.CalculateSurvivingUnits(gameWorld).Values.Sum();
        var armySizeAtBattleStart = gameWorld.PlayerUnitsAtBattleStart.Values.Sum();
        var survivingUnits = Math.Clamp(survivingArmySize, 0, clampedMaxPositions);
        var unitsLost = Math.Clamp(armySizeAtBattleStart - survivingArmySize, 0, clampedMaxPositions - survivingUnits);
        var emptyPositions = Math.Max(0, clampedMaxPositions - armySizeAtBattleStart);

        return $"{new string('|', survivingUnits)}{new string('X', unitsLost)}{new string('O', emptyPositions)}";
    }
}
