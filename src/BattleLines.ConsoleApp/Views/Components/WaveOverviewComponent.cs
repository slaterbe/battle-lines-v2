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
        ConsoleTextComponent.WriteLine($"Progress: {progressBar} {defeatedWaveCount}/{totalWaveCount} defeated", ConsoleColor.Cyan);

        var rewardText = BuildRewardText(gameWorld);
        if (!string.IsNullOrWhiteSpace(rewardText))
        {
            ConsoleTextComponent.WriteLine(rewardText, ConsoleColor.Yellow);
        }
    }

    private static string BuildRewardText(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return gameWorld.EnemyWaves.FinalRewards.Count > 0
                ? $"Final Reward: {FormatRewards(gameWorld.EnemyWaves.FinalRewards)}"
                : string.Empty;
        }

        var currentWave = gameWorld.EnemyWaves.Waves[0];
        var rewardText = $"Reward: {FormatRewards(currentWave.Rewards)}";
        if (gameWorld.EnemyWaves.FinalRewards.Count > 0)
        {
            rewardText += $"  Final Reward: {FormatRewards(gameWorld.EnemyWaves.FinalRewards)}";
        }

        return rewardText;
    }

    private static string FormatRewards(IReadOnlyList<EnemyWaveRewardModel> rewards)
    {
        return string.Join(", ", rewards
            .Where(reward => reward.Amount > 0)
            .Select(reward => $"{reward.Amount} {reward.Type}"));
    }
}
