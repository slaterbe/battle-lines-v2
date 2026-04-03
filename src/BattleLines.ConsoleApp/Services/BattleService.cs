using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class BattleService
{
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public void StartBattle(GameWorld gameWorld)
    {
        if (gameWorld.State == GameState.Battle || gameWorld.State == GameState.PostBattle || gameWorld.EnemyWaveList.Count == 0)
        {
            return;
        }

        gameWorld.State = GameState.PreBattle;
    }

    public void BeginBattle(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.PreBattle)
        {
            return;
        }

        gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var spearmenCount);
        gameWorld.PlayerHealthAtBattleStart = gameWorld.PlayerTotalHealth;
        gameWorld.SpearmenCountAtBattleStart = spearmenCount;
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
        gameWorld.State = GameState.Battle;
    }

    public void ResolveBattleTick(GameWorld gameWorld)
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
        }

        gameWorld.PlayerTotalAttack = CalculateCurrentPlayerAttack(gameWorld);
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

        gameWorld.LastBattleWon = gameWorld.CurrentWaveTotalHealth == 0;
        gameWorld.HasPendingPostBattleResolution = true;
        gameWorld.State = GameState.PostBattle;
    }

    public void ResetCurrentWave(GameWorld gameWorld)
    {
        gameWorld.PlayerHealthAtBattleStart = 0;
        gameWorld.SpearmenCountAtBattleStart = 0;
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
        gameWorld.State = GameState.Village;
        gameWorldStatsService.Refresh(gameWorld);
    }

    private static int CalculateCurrentPlayerAttack(GameWorld gameWorld)
    {
        if (!UnitCatalog.DefaultUnits.TryGetValue(UnitType.SpearmenLvl1, out var spearmanModel) || spearmanModel.Health <= 0)
        {
            return gameWorld.PlayerTotalAttack;
        }

        var healthLost = Math.Max(0, gameWorld.PlayerHealthAtBattleStart - gameWorld.PlayerTotalHealth);
        var spearmenLost = Math.Min(gameWorld.SpearmenCountAtBattleStart, healthLost / spearmanModel.Health);
        var survivingSpearmen = Math.Max(0, gameWorld.SpearmenCountAtBattleStart - spearmenLost);
        return survivingSpearmen * spearmanModel.Attack;
    }

    private static int CalculateCurrentWaveAttack(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaveList.Count == 0)
        {
            return 0;
        }

        var currentWave = gameWorld.EnemyWaveList[0];
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
}
