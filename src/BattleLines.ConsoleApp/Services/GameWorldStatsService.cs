using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class GameWorldStatsService
{
    public void Refresh(GameWorld gameWorld)
    {
        gameWorld.PlayerTotalHealth = CalculatePlayerTotalHealth(gameWorld);
        gameWorld.PlayerTotalAttack = CalculatePlayerTotalAttack(gameWorld);
        gameWorld.PlayerTotalMaxAttack = CalculatePlayerTotalMaxAttack(gameWorld);
        gameWorld.CurrentWaveTotalHealth = CalculateCurrentWaveTotalHealth(gameWorld);
        gameWorld.CurrentWaveTotalAttack = CalculateCurrentWaveTotalAttack(gameWorld);
        gameWorld.CurrentWaveTotalMaxAttack = CalculateCurrentWaveTotalMaxAttack(gameWorld);
    }

    private static int CalculatePlayerTotalHealth(GameWorld gameWorld)
    {
        var totalHealth = 0;

        foreach (var playerUnit in gameWorld.PlayerUnits)
        {
            if (!UnitCatalog.DefaultUnits.TryGetValue(playerUnit.Key, out var unitModel))
            {
                continue;
            }

            totalHealth += unitModel.Health * playerUnit.Value;
        }

        return totalHealth;
    }

    private static int CalculatePlayerTotalAttack(GameWorld gameWorld)
    {
        var totalAttack = 0;

        foreach (var playerUnit in gameWorld.PlayerUnits)
        {
            if (!UnitCatalog.DefaultUnits.TryGetValue(playerUnit.Key, out var unitModel))
            {
                continue;
            }

            totalAttack += unitModel.Attack * playerUnit.Value;
        }

        return totalAttack;
    }

    private static int CalculateCurrentWaveTotalHealth(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return 0;
        }

        var totalHealth = 0;

        foreach (var enemy in gameWorld.EnemyWaves.Waves[0].Enemies)
        {
            if (!UnitCatalog.DefaultUnits.TryGetValue(enemy.EnemyType, out var unitModel))
            {
                continue;
            }

            totalHealth += unitModel.Health * enemy.Count;
        }

        return totalHealth;
    }

    private static int CalculateCurrentWaveTotalAttack(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return 0;
        }

        var totalAttack = 0;

        foreach (var enemy in gameWorld.EnemyWaves.Waves[0].Enemies)
        {
            if (!UnitCatalog.DefaultUnits.TryGetValue(enemy.EnemyType, out var unitModel))
            {
                continue;
            }

            totalAttack += unitModel.Attack * enemy.Count;
        }

        return totalAttack;
    }

    private static int CalculatePlayerTotalMaxAttack(GameWorld gameWorld)
    {
        var totalMaxAttack = 0;

        foreach (var playerUnit in gameWorld.PlayerUnits)
        {
            if (!UnitCatalog.DefaultUnits.TryGetValue(playerUnit.Key, out var unitModel))
            {
                continue;
            }

            totalMaxAttack += (unitModel.Attack + unitModel.MaxAttack) * playerUnit.Value;
        }

        return totalMaxAttack;
    }

    private static int CalculateCurrentWaveTotalMaxAttack(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return 0;
        }

        var totalMaxAttack = 0;

        foreach (var enemy in gameWorld.EnemyWaves.Waves[0].Enemies)
        {
            if (!UnitCatalog.DefaultUnits.TryGetValue(enemy.EnemyType, out var unitModel))
            {
                continue;
            }

            totalMaxAttack += (unitModel.Attack + unitModel.MaxAttack) * enemy.Count;
        }

        return totalMaxAttack;
    }
}
