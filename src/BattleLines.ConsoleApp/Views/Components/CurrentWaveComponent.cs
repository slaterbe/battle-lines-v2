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

        foreach (var enemy in currentWave.Enemies)
        {
            ConsoleTextComponent.WriteLine(
                $"{UnitTypeDisplayNames.Get(enemy.EnemyType)}: {UnitDisplayComponent.RenderUnitCount(gameWorld, enemy.EnemyType, enemy.Count)}",
                ConsoleColor.Red);
        }

        ConsoleTextComponent.WriteLine($"Health: {BattleHistoryComponent.RenderEnemyHealth(gameWorld)}", ConsoleColor.Red);
        ConsoleTextComponent.WriteLine($"Attack: {BattleHistoryComponent.RenderEnemyAttack(gameWorld)}", ConsoleColor.Red);
    }
}
