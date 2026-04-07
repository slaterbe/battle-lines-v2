using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class PlayerArmyBattleService
{
    public void CaptureBattleStart(GameWorld gameWorld)
    {
        gameWorld.PlayerHealthAtBattleStart = gameWorld.PlayerTotalHealth;
        gameWorld.PlayerUnitsAtBattleStart = gameWorld.PlayerUnits.ToDictionary(entry => entry.Key, entry => entry.Value);
    }

    public int CalculateCurrentPlayerAttack(GameWorld gameWorld)
    {
        var survivingUnits = CalculateSurvivingUnits(gameWorld);
        var totalAttack = 0;

        foreach (var unit in survivingUnits)
        {
            if (!UnitCatalog.DefaultUnits.TryGetValue(unit.Key, out var unitModel))
            {
                continue;
            }

            totalAttack += unit.Value * unitModel.Attack;
        }

        return totalAttack;
    }

    public void ApplyPlayerBattleLosses(GameWorld gameWorld)
    {
        var survivingUnits = CalculateSurvivingUnits(gameWorld);
        var unitTypes = gameWorld.PlayerUnits.Keys
            .Concat(gameWorld.PlayerUnitsAtBattleStart.Keys)
            .Distinct()
            .ToArray();

        foreach (var unitType in unitTypes)
        {
            survivingUnits.TryGetValue(unitType, out var survivingCount);
            gameWorld.PlayerUnits[unitType] = survivingCount;
        }
    }

    public IReadOnlyDictionary<UnitType, int> CalculateSurvivingUnits(GameWorld gameWorld)
    {
        return CalculateSurvivingUnits(gameWorld, gameWorld.PlayerTotalHealth);
    }

    public IReadOnlyDictionary<UnitType, int> CalculateSurvivingUnits(GameWorld gameWorld, int playerTotalHealth)
    {
        if (gameWorld.PlayerUnitsAtBattleStart.Count == 0 || gameWorld.PlayerHealthAtBattleStart <= 0)
        {
            return gameWorld.PlayerUnits.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        var survivingUnits = gameWorld.PlayerUnitsAtBattleStart.ToDictionary(entry => entry.Key, entry => entry.Value);
        var remainingHealthLoss = Math.Max(0, gameWorld.PlayerHealthAtBattleStart - playerTotalHealth);

        foreach (var unitType in gameWorld.PlayerUnitsAtBattleStart.Keys
                     .OrderBy(GetUnitHealth)
                     .ThenBy(unitType => unitType))
        {
            if (!UnitCatalog.DefaultUnits.TryGetValue(unitType, out var unitModel) || unitModel.Health <= 0)
            {
                continue;
            }

            var startingCount = gameWorld.PlayerUnitsAtBattleStart[unitType];
            var unitsLost = Math.Min(startingCount, remainingHealthLoss / unitModel.Health);
            survivingUnits[unitType] = Math.Max(0, startingCount - unitsLost);
            remainingHealthLoss -= unitsLost * unitModel.Health;

            if (remainingHealthLoss <= 0)
            {
                break;
            }
        }

        return survivingUnits;
    }

    private static int GetUnitHealth(UnitType unitType)
    {
        return UnitCatalog.DefaultUnits.TryGetValue(unitType, out var unitModel)
            ? unitModel.Health
            : int.MaxValue;
    }
}
