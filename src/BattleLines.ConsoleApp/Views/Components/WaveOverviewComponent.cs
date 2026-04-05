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
        var remainingWaveCount = Math.Max(0, gameWorld.EnemyWaves.Waves.Count);
        var currentWavePosition = Math.Clamp(gameWorld.WavePosition, 0, totalWaveCount);
        var defeatedWaveCount = currentWavePosition == 0
            ? 0
            : Math.Max(0, currentWavePosition - 1);
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
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            if (gameWorld.EnemyWaves.FinalRewardAmount > 0)
            {
                ConsoleTextComponent.WriteLine(
                    $"Final Reward: {gameWorld.EnemyWaves.FinalRewardAmount} {gameWorld.EnemyWaves.FinalRewardType}",
                    ConsoleColor.Yellow);
            }

            return;
        }

        var currentWave = gameWorld.EnemyWaves.Waves[0];
        ConsoleTextComponent.WriteLine($"Reward: {currentWave.RewardAmount} {currentWave.RewardType}", ConsoleColor.Yellow);
        if (gameWorld.EnemyWaves.FinalRewardAmount > 0)
        {
            ConsoleTextComponent.WriteLine(
                $"Final Reward: {gameWorld.EnemyWaves.FinalRewardAmount} {gameWorld.EnemyWaves.FinalRewardType}",
                ConsoleColor.Yellow);
        }
    }
}
