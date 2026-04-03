using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class ExitPostBattleCommand : IGameCommand
{
    private readonly GameWorldStatsService gameWorldStatsService = new();
    private readonly VillageTransitionService villageTransitionService = new();

    public string Label => "Continue";
    public string HelpText => "Apply battle results, collect rewards, and move on.";

    public bool Execute(GameWorld gameWorld)
    {
        var nextState = gameWorld.State switch
        {
            GameState.PostWave => GameState.PreBattle,
            GameState.PostBattle => GameState.Village,
            _ => gameWorld.State
        };

        if ((gameWorld.State != GameState.PostWave && gameWorld.State != GameState.PostBattle) ||
            !gameWorld.HasPendingPostBattleResolution)
        {
            return false;
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
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();

        if (nextState == GameState.Village)
        {
            villageTransitionService.MoveToVillage(gameWorld, applyProduction: true);
            return false;
        }

        gameWorld.State = nextState;
        gameWorldStatsService.Refresh(gameWorld);
        return false;
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
            case EnemyWaveRewardType.Villagers:
                gameWorld.Villagers += reward.RewardAmount;
                break;
            case EnemyWaveRewardType.Gold:
                gameWorld.Gold += reward.RewardAmount;
                break;
        }
    }
}
