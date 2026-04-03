using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class PostBattleService
{
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public void ExitBattleScreen(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.PostBattle || !gameWorld.HasPendingPostBattleResolution)
        {
            return;
        }

        if (gameWorld.LastBattleWon)
        {
            ApplyWaveReward(gameWorld);
        }

        ApplyPlayerBattleLosses(gameWorld);

        if (gameWorld.EnemyWaveList.Count > 0)
        {
            gameWorld.EnemyWaveList.RemoveAt(0);
        }

        gameWorld.PlayerHealthAtBattleStart = 0;
        gameWorld.SpearmenCountAtBattleStart = 0;
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
        gameWorld.State = GameState.Village;
        gameWorldStatsService.Refresh(gameWorld);
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
    }

    private static void ApplyPlayerBattleLosses(GameWorld gameWorld)
    {
        var healthLost = Math.Max(0, gameWorld.PlayerHealthAtBattleStart - gameWorld.PlayerTotalHealth);
        if (healthLost == 0)
        {
            return;
        }

        if (!UnitCatalog.DefaultUnits.TryGetValue(UnitType.SpearmenLvl1, out var spearmanModel) || spearmanModel.Health <= 0)
        {
            return;
        }

        gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var spearmenCount);
        var spearmenLost = healthLost / spearmanModel.Health;
        gameWorld.PlayerUnits[UnitType.SpearmenLvl1] = Math.Max(0, spearmenCount - spearmenLost);
    }

    private static void ApplyWaveReward(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaveList.Count == 0)
        {
            return;
        }

        var reward = gameWorld.EnemyWaveList[0];

        switch (reward.RewardType)
        {
            case EnemyWaveRewardType.Spears:
                gameWorld.Spears += reward.RewardAmount;
                break;
            case EnemyWaveRewardType.Commoners:
                gameWorld.Commoners += reward.RewardAmount;
                break;
            case EnemyWaveRewardType.Gold:
                gameWorld.Gold += reward.RewardAmount;
                break;
        }
    }
}
