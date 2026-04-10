using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class ResolveBattleTickCommand : IGameTickCommand
{
    private static readonly Random Random = new();
    private readonly GameWorldStatsService gameWorldStatsService = new();
    private readonly PlayerArmyBattleService playerArmyBattleService = new();
    private readonly VillageTransitionService villageTransitionService = new();

    public void Execute(GameWorld gameWorld)
    {
        var previousEnemyHealth = gameWorld.CurrentWaveTotalHealth;
        var previousPlayerHealth = gameWorld.PlayerTotalHealth;
        var previousPlayerAttack = gameWorld.PlayerTotalAttack;
        var previousPlayerMaxAttack = gameWorld.PlayerTotalMaxAttack;
        var previousEnemyAttack = gameWorld.CurrentWaveTotalAttack;
        var previousEnemyMaxAttack = gameWorld.CurrentWaveTotalMaxAttack;
        var playerDamage = RollDamage(gameWorld.PlayerTotalAttack, gameWorld.PlayerTotalMaxAttack);
        var enemyDamage = RollDamage(gameWorld.CurrentWaveTotalAttack, gameWorld.CurrentWaveTotalMaxAttack);
        gameWorld.CurrentWaveTotalHealth = Math.Max(0, gameWorld.CurrentWaveTotalHealth - playerDamage);
        gameWorld.PlayerTotalHealth = Math.Max(0, gameWorld.PlayerTotalHealth - enemyDamage);

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

        gameWorld.PlayerTotalMaxAttack = playerArmyBattleService.CalculateCurrentPlayerMaxAttack(gameWorld);
        if (gameWorld.PlayerTotalMaxAttack < previousPlayerMaxAttack)
        {
            gameWorld.PlayerMaxAttackHistory.Add(previousPlayerMaxAttack);
        }

        gameWorld.CurrentWaveTotalAttack = CalculateCurrentWaveAttack(gameWorld);
        if (gameWorld.CurrentWaveTotalAttack < previousEnemyAttack)
        {
            gameWorld.EnemyAttackHistory.Add(previousEnemyAttack);
        }

        gameWorld.CurrentWaveTotalMaxAttack = CalculateCurrentWaveMaxAttack(gameWorld);
        if (gameWorld.CurrentWaveTotalMaxAttack < previousEnemyMaxAttack)
        {
            gameWorld.EnemyMaxAttackHistory.Add(previousEnemyMaxAttack);
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

    private static int CalculateCurrentWaveMaxAttack(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return 0;
        }

        var currentWave = gameWorld.EnemyWaves.Waves[0];
        if (currentWave.Enemies.Count != 1)
        {
            return gameWorld.CurrentWaveTotalMaxAttack;
        }

        var enemy = currentWave.Enemies[0];
        if (!UnitCatalog.DefaultUnits.TryGetValue(enemy.EnemyType, out var enemyModel) || enemyModel.Health <= 0)
        {
            return gameWorld.CurrentWaveTotalMaxAttack;
        }

        var waveHealthAtStart = enemy.Count * enemyModel.Health;
        var healthLost = Math.Max(0, waveHealthAtStart - gameWorld.CurrentWaveTotalHealth);
        var enemiesLost = Math.Min(enemy.Count, healthLost / enemyModel.Health);
        var survivingEnemies = Math.Max(0, enemy.Count - enemiesLost);
        return survivingEnemies * (enemyModel.Attack + enemyModel.MaxAttack);
    }

    private static int RollDamage(int minAttack, int maxAttack)
    {
        var lowerBound = Math.Min(minAttack, maxAttack);
        var upperBound = Math.Max(minAttack, maxAttack);

        if (upperBound <= lowerBound)
        {
            return lowerBound;
        }

        return Random.Next(lowerBound, upperBound + 1);
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
