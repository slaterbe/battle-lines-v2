using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class ExitPostBattleCommand : IGameCommand
{
    private readonly GameWorldStatsService gameWorldStatsService = new();
    private readonly PlayerArmyBattleService playerArmyBattleService = new();
    private readonly VillageTransitionService villageTransitionService = new();

    public GameCommandCategory Category => GameCommandCategory.Battle;
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
            ApplyReward(gameWorld.EnemyWaves.Waves[0].RewardType, gameWorld.EnemyWaves.Waves[0].RewardAmount, gameWorld);
        }

        playerArmyBattleService.ApplyPlayerBattleLosses(gameWorld);

        if (gameWorld.EnemyWaves.Waves.Count > 0)
        {
            gameWorld.EnemyWaves.Waves.RemoveAt(0);
        }

        if (gameWorld.LastBattleWon && gameWorld.EnemyWaves.Waves.Count == 0)
        {
            ApplyReward(gameWorld.EnemyWaves.FinalRewardType, gameWorld.EnemyWaves.FinalRewardAmount, gameWorld);
        }

        gameWorld.PlayerHealthAtBattleStart = 0;
        gameWorld.PlayerUnitsAtBattleStart.Clear();
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
    private static void ApplyReward(EnemyWaveRewardType rewardType, int rewardAmount, GameWorld gameWorld)
    {
        if (rewardAmount <= 0)
        {
            return;
        }

        switch (rewardType)
        {
            case EnemyWaveRewardType.Spears:
                gameWorld.Spears += rewardAmount;
                break;
            case EnemyWaveRewardType.Villagers:
                gameWorld.Villagers += rewardAmount;
                break;
            case EnemyWaveRewardType.Gold:
                gameWorld.Gold += rewardAmount;
                break;
        }
    }
}
