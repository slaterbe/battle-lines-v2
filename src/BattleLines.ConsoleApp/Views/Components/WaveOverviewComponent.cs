using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class WaveOverviewComponent
{
    public void Render(GameWorld gameWorld)
    {
        RenderWaveOverview(gameWorld);
    }

    private static void RenderWaveOverview(GameWorld gameWorld)
    {
        var totalWaveCount = Math.Max(0, gameWorld.TotalWaveCount);
        var currentWavePosition = Math.Clamp(gameWorld.WavePosition, 0, totalWaveCount);
        var defeatedWaveCount = currentWavePosition == 0
            ? 0
            : Math.Max(0, currentWavePosition - 1);

        if (gameWorld.HasPendingPostBattleResolution &&
            (gameWorld.State == GameState.PostWave || gameWorld.State == GameState.PostBattle))
        {
            defeatedWaveCount = Math.Min(totalWaveCount, defeatedWaveCount + 1);
        }
        else if (gameWorld.State == GameState.PostBattle && gameWorld.LastBattleWon)
        {
            defeatedWaveCount = totalWaveCount;
        }

        const int progressBarWidth = 20;

        var filledSegments = totalWaveCount == 0
            ? 0
            : (int)Math.Round((double)defeatedWaveCount / totalWaveCount * progressBarWidth, MidpointRounding.AwayFromZero);
        filledSegments = Math.Clamp(filledSegments, 0, progressBarWidth);

        var progressBar = $"[{new string('#', filledSegments)}{new string('-', progressBarWidth - filledSegments)}]";
        ConsoleTextComponent.Write($"Progress: {progressBar} {defeatedWaveCount}/{totalWaveCount} defeated", ConsoleColor.Cyan);

        var rewardText = BuildRewardText(gameWorld);
        if (!string.IsNullOrWhiteSpace(rewardText))
        {
            ConsoleTextComponent.Write("  ", ConsoleColor.Cyan);
            ConsoleTextComponent.Write(rewardText, ConsoleColor.Yellow);
        }

        ConsoleTextComponent.NewLine();
    }

    private static string BuildRewardText(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return gameWorld.EnemyWaves.FinalRewardAmount > 0
                ? $"Final Reward: {gameWorld.EnemyWaves.FinalRewardAmount} {gameWorld.EnemyWaves.FinalRewardType}"
                : string.Empty;
        }

        var currentWave = gameWorld.EnemyWaves.Waves[0];
        var rewardText = $"Reward: {currentWave.RewardAmount} {currentWave.RewardType}";
        if (gameWorld.EnemyWaves.FinalRewardAmount > 0)
        {
            rewardText += $"  Final Reward: {gameWorld.EnemyWaves.FinalRewardAmount} {gameWorld.EnemyWaves.FinalRewardType}";
        }

        return rewardText;
    }
}
