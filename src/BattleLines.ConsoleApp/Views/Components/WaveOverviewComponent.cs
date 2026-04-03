using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class WaveOverviewComponent
{
    public void Render(GameWorld gameWorld)
    {
        RenderWaveProgress(gameWorld);
        RenderWaveReward(gameWorld);
    }

    private static void RenderWaveProgress(GameWorld gameWorld)
    {
        var totalWaveCount = Math.Max(0, gameWorld.TotalWaveCount);
        var remainingWaveCount = Math.Max(0, gameWorld.EnemyWaveList.Count);
        var defeatedWaveCount = Math.Max(0, totalWaveCount - remainingWaveCount);
        const int progressBarWidth = 20;

        var filledSegments = totalWaveCount == 0
            ? 0
            : (int)Math.Round((double)defeatedWaveCount / totalWaveCount * progressBarWidth, MidpointRounding.AwayFromZero);
        filledSegments = Math.Clamp(filledSegments, 0, progressBarWidth);

        var progressBar = $"[{new string('#', filledSegments)}{new string('-', progressBarWidth - filledSegments)}]";
        ConsoleTextComponent.WriteLine(
            $"Progress: {progressBar} {defeatedWaveCount}/{totalWaveCount} defeated, {remainingWaveCount} remaining",
            ConsoleColor.Cyan);
    }

    private static void RenderWaveReward(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaveList.Count == 0)
        {
            return;
        }

        var currentWave = gameWorld.EnemyWaveList[0];
        ConsoleTextComponent.WriteLine($"Reward: {currentWave.RewardAmount} {currentWave.RewardType}", ConsoleColor.Yellow);
    }
}
