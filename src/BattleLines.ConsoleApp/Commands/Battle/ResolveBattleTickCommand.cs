using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class ResolveBattleTickCommand : IGameTickCommand
{
    private readonly GameWorldStatsService gameWorldStatsService = new();
    private readonly PlayerArmyBattleService playerArmyBattleService = new();
    private readonly VillageTransitionService villageTransitionService = new();

    public void Execute(GameWorld gameWorld)
    {
        var previousEnemyHealth = gameWorld.CurrentWaveTotalHealth;
        var previousPlayerHealth = gameWorld.PlayerTotalHealth;
        var previousPlayerAttack = gameWorld.PlayerTotalAttack;
        var previousEnemyAttack = gameWorld.CurrentWaveTotalAttack;
        gameWorld.CurrentWaveTotalHealth = Math.Max(0, gameWorld.CurrentWaveTotalHealth - gameWorld.PlayerTotalAttack);
        gameWorld.PlayerTotalHealth = Math.Max(0, gameWorld.PlayerTotalHealth - gameWorld.CurrentWaveTotalAttack);

        if (gameWorld.CurrentWaveTotalHealth < previousEnemyHealth)
        {
            gameWorld.EnemyHealthHistory.Add(previousEnemyHealth);
        }

        if (gameWorld.PlayerTotalHealth < previousPlayerHealth)
        {
            gameWorld.PlayerHealthHistory.Add(previousPlayerHealth);
            RecordPlayerUnitSnapshot(gameWorld);
        }

        gameWorld.PlayerTotalAttack = playerArmyBattleService.CalculateCurrentPlayerAttack(gameWorld);
        if (gameWorld.PlayerTotalAttack < previousPlayerAttack)
        {
            gameWorld.PlayerAttackHistory.Add(previousPlayerAttack);
        }

        gameWorld.CurrentWaveTotalAttack = CalculateCurrentWaveAttack(gameWorld);
        if (gameWorld.CurrentWaveTotalAttack < previousEnemyAttack)
        {
            gameWorld.EnemyAttackHistory.Add(previousEnemyAttack);
        }

        if (gameWorld.CurrentWaveTotalHealth > 0 && gameWorld.PlayerTotalHealth > 0)
        {
            return;
        }

        gameWorld.IsUpgradesVisible = true;
        gameWorld.LastBattleWon = gameWorld.CurrentWaveTotalHealth == 0;
        if (!gameWorld.LastBattleWon)
        {
            playerArmyBattleService.ApplyPlayerBattleLosses(gameWorld);
            villageTransitionService.MoveToVillage(gameWorld, applyProduction: true);
            return;
        }

        gameWorld.HasPendingPostBattleResolution = true;
        gameWorld.State = GameState.PostWave;
    }

    private static int CalculateCurrentWaveAttack(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return 0;
        }

        var currentWave = gameWorld.EnemyWaves.Waves[0];
        if (currentWave.Enemies.Count != 1)
        {
            return gameWorld.CurrentWaveTotalAttack;
        }

        var enemy = currentWave.Enemies[0];
        if (!UnitCatalog.DefaultUnits.TryGetValue(enemy.EnemyType, out var enemyModel) || enemyModel.Health <= 0)
        {
            return gameWorld.CurrentWaveTotalAttack;
        }

        var waveHealthAtStart = enemy.Count * enemyModel.Health;
        var healthLost = Math.Max(0, waveHealthAtStart - gameWorld.CurrentWaveTotalHealth);
        var enemiesLost = Math.Min(enemy.Count, healthLost / enemyModel.Health);
        var survivingEnemies = Math.Max(0, enemy.Count - enemiesLost);
        return survivingEnemies * enemyModel.Attack;
    }

    private void RecordPlayerUnitSnapshot(GameWorld gameWorld)
    {
        var survivingUnits = playerArmyBattleService.CalculateSurvivingUnits(gameWorld);

        if (gameWorld.PlayerUnitHistory.Count == 0)
        {
            gameWorld.PlayerUnitHistory.Add(survivingUnits.ToDictionary(entry => entry.Key, entry => entry.Value));
            return;
        }

        var latestSnapshot = gameWorld.PlayerUnitHistory[^1];
        var unitTypes = survivingUnits.Keys.Concat(latestSnapshot.Keys).Distinct();
        var hasChanged = unitTypes.Any(unitType =>
        {
            survivingUnits.TryGetValue(unitType, out var survivingCount);
            latestSnapshot.TryGetValue(unitType, out var latestCount);
            return survivingCount != latestCount;
        });

        if (!hasChanged)
        {
            return;
        }

        gameWorld.PlayerUnitHistory.Add(survivingUnits.ToDictionary(entry => entry.Key, entry => entry.Value));
    }

}
