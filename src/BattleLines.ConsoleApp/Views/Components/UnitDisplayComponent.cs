using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public static class UnitDisplayComponent
{
    public static string RenderUnitCount(GameWorld gameWorld, UnitType unitType, int count)
    {
        return unitType switch
        {
            UnitType.SpearmenLvl1 => RenderSpearmenPositions(gameWorld, count),
            UnitType.GiantRat => new string('|', Math.Max(0, count)),
            _ => count.ToString()
        };
    }

    private static string RenderSpearmenPositions(GameWorld gameWorld, int count)
    {
        var clampedMaxPositions = Math.Max(0, gameWorld.MaxArmySize);
        var displayedCount = Math.Clamp(count, 0, clampedMaxPositions);

        if (gameWorld.State != GameState.Battle &&
            !((gameWorld.State == GameState.PostWave || gameWorld.State == GameState.PostBattle) &&
              gameWorld.HasPendingPostBattleResolution))
        {
            return $"{new string('|', displayedCount)}{new string('O', clampedMaxPositions - displayedCount)}";
        }

        if (!UnitCatalog.DefaultUnits.TryGetValue(UnitType.SpearmenLvl1, out var spearmanModel) || spearmanModel.Health <= 0)
        {
            return $"{new string('|', displayedCount)}{new string('O', clampedMaxPositions - displayedCount)}";
        }

        gameWorld.PlayerUnitsAtBattleStart.TryGetValue(UnitType.SpearmenLvl1, out var spearmenAtBattleStart);
        var healthLost = Math.Max(0, gameWorld.PlayerHealthAtBattleStart - gameWorld.PlayerTotalHealth);
        var spearmenLost = Math.Min(spearmenAtBattleStart, healthLost / spearmanModel.Health);
        var survivingSpearmen = Math.Max(0, spearmenAtBattleStart - spearmenLost);
        var emptyPositions = Math.Max(0, clampedMaxPositions - spearmenAtBattleStart);

        return $"{new string('|', survivingSpearmen)}{new string('X', spearmenLost)}{new string('O', emptyPositions)}";
    }
}
