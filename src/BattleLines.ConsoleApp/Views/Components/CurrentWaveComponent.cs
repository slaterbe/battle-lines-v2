using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class CurrentWaveComponent
{
    public void Render(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            ConsoleTextComponent.WriteLine("No enemy waves queued.", ConsoleColor.Red);
            return;
        }

        var currentWave = gameWorld.EnemyWaves.Waves[0];
        var totalEnemyCount = currentWave.Enemies.Sum(enemy => enemy.Count);

        foreach (var enemy in currentWave.Enemies)
        {
            ConsoleTextComponent.WriteLine(
                $"{UnitTypeDisplayNames.Get(enemy.EnemyType)}: {RenderEnemyCountHistory(gameWorld, enemy.EnemyType, enemy.Count)}",
                ConsoleColor.Red);
        }

        var attackLabel = gameWorld.IsSpearControlsVisible ? "Min Attack" : "Attack";
        ConsoleTextComponent.WriteLine($"Health: {BattleHistoryComponent.RenderEnemyHealth(gameWorld)}", ConsoleColor.Red);
        ConsoleTextComponent.WriteLine($"{attackLabel}: {BattleHistoryComponent.RenderEnemyAttack(gameWorld)}", ConsoleColor.Red);
        if (gameWorld.IsSpearControlsVisible)
        {
            ConsoleTextComponent.WriteLine($"Max Attack: {BattleHistoryComponent.RenderEnemyMaxAttack(gameWorld)}", ConsoleColor.Red);
        }
    }

    private static string RenderEnemyCountHistory(GameWorld gameWorld, UnitType enemyType, int startingCount)
    {
        if (!UnitCatalog.DefaultUnits.TryGetValue(enemyType, out var unitModel) || unitModel.Health <= 0)
        {
            return startingCount.ToString();
        }

        if (gameWorld.EnemyHealthHistory.Count == 0)
        {
            return CalculateSurvivingEnemyCount(startingCount, unitModel.Health, gameWorld.CurrentWaveTotalHealth).ToString();
        }

        var countHistory = gameWorld.EnemyHealthHistory
            .Select(health => CalculateSurvivingEnemyCount(startingCount, unitModel.Health, health))
            .Append(CalculateSurvivingEnemyCount(startingCount, unitModel.Health, gameWorld.CurrentWaveTotalHealth));

        return string.Join(" -> ", countHistory);
    }

    private static int CalculateSurvivingEnemyCount(int startingCount, int enemyHealth, int currentWaveHealth)
    {
        var waveHealthAtStart = startingCount * enemyHealth;
        var healthLost = Math.Max(0, waveHealthAtStart - currentWaveHealth);
        var enemiesLost = Math.Min(startingCount, healthLost / enemyHealth);
        return Math.Max(0, startingCount - enemiesLost);
    }
}
