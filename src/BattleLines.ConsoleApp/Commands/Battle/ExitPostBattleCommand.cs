using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class ExitPostBattleCommand : IGameCommand
{
    private readonly GameWorldStatsService gameWorldStatsService = new();
    private readonly PlayerArmyBattleService playerArmyBattleService = new();
    private readonly VillageTransitionService villageTransitionService = new();
    private readonly string label;
    private readonly string helpText;

    public ExitPostBattleCommand(
        string label = "Continue",
        string helpText = "Apply battle results, collect rewards, and move on.")
    {
        this.label = label;
        this.helpText = helpText;
    }

    public GameCommandCategory Category => GameCommandCategory.Battle;
    public string Label => label;
    public string HelpText => helpText;

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State == GameState.PostBattle && !gameWorld.HasPendingPostBattleResolution)
        {
            villageTransitionService.MoveToVillage(gameWorld, applyProduction: true, advanceBattle: gameWorld.LastBattleWon);
            return false;
        }

        var battleWon = gameWorld.LastBattleWon;
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

        if (battleWon)
        {
            ApplyReward(gameWorld.EnemyWaves.Waves[0].RewardType, gameWorld.EnemyWaves.Waves[0].RewardAmount, gameWorld);
        }

        playerArmyBattleService.ApplyPlayerBattleLosses(gameWorld);

        if (gameWorld.EnemyWaves.Waves.Count > 0)
        {
            gameWorld.EnemyWaves.Waves.RemoveAt(0);
        }

        if (battleWon && gameWorld.EnemyWaves.Waves.Count == 0)
        {
            ApplyReward(gameWorld.EnemyWaves.FinalRewardType, gameWorld.EnemyWaves.FinalRewardAmount, gameWorld);
        }

        gameWorld.WavePosition = GetCurrentWavePosition(gameWorld);

        gameWorld.PlayerHealthAtBattleStart = 0;
        gameWorld.PlayerUnitsAtBattleStart.Clear();
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.PlayerMaxAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
        gameWorld.EnemyMaxAttackHistory.Clear();

        if (gameWorld.State == GameState.PostWave && battleWon && gameWorld.EnemyWaves.Waves.Count == 0)
        {
            gameWorld.HasPendingPostBattleResolution = false;
            gameWorld.State = GameState.PostBattle;
            gameWorldStatsService.Refresh(gameWorld);
            return false;
        }

        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;

        if (nextState == GameState.Village)
        {
            villageTransitionService.MoveToVillage(gameWorld, applyProduction: true, advanceBattle: battleWon);
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

    private static int GetCurrentWavePosition(GameWorld gameWorld)
    {
        var remainingWaveCount = gameWorld.EnemyWaves.Waves.Count;
        if (remainingWaveCount <= 0)
        {
            return 0;
        }

        var defeatedWaveCount = Math.Max(0, gameWorld.TotalWaveCount - remainingWaveCount);
        return defeatedWaveCount + 1;
    }
}
