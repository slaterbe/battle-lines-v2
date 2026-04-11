using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class BattleLineComponent
{
    public void Render(GameWorld gameWorld)
    {
        ConsoleTextComponent.WriteLine(RenderEnemyArmyLine(gameWorld), ConsoleColor.Red);
        ConsoleTextComponent.WriteLine(RenderPlayerArmyLine(gameWorld), ConsoleColor.Blue);
    }

    public static string RenderEnemyArmyLine(GameWorld gameWorld)
    {
        return $"Enemy Army: {UnitDisplayComponent.RenderUnitCount(gameWorld, UnitType.GiantRat, GetCurrentEnemyCount(gameWorld))}";
    }

    public static string RenderPlayerArmyLine(GameWorld gameWorld)
    {
        return $"Player Army: {UnitDisplayComponent.RenderArmyCount(gameWorld)}";
    }

    private static int GetCurrentEnemyCount(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return 0;
        }

        return gameWorld.EnemyWaves.Waves[0].Enemies.Sum(enemy => enemy.Count);
    }
}
