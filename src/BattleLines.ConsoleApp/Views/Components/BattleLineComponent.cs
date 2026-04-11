using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class BattleLineComponent
{
    public void Render(GameWorld gameWorld)
    {
        if (ShouldShowEnemyArmy(gameWorld))
        {
            WriteCenteredLine(RenderEnemyArmyLine(gameWorld), ConsoleColor.Red);
        }

        WriteCenteredLine(RenderPlayerArmyLine(gameWorld), ConsoleColor.Blue);
    }

    public static string RenderEnemyArmyLine(GameWorld gameWorld)
    {
        return UnitDisplayComponent.RenderUnitCount(gameWorld, UnitType.GiantRat, GetCurrentEnemyCount(gameWorld));
    }

    public static string RenderPlayerArmyLine(GameWorld gameWorld)
    {
        return UnitDisplayComponent.RenderArmyCount(gameWorld);
    }

    private static int GetCurrentEnemyCount(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return 0;
        }

        return gameWorld.EnemyWaves.Waves[0].Enemies.Sum(enemy => enemy.Count);
    }

    private static bool ShouldShowEnemyArmy(GameWorld gameWorld)
    {
        return gameWorld.State != GameState.Village;
    }

    private static void WriteCenteredLine(string text, ConsoleColor color)
    {
        var contentWidth = Math.Max(1, ResourcePanelComponent.GetLeftColumnWidth());
        var left = Math.Max(0, (contentWidth - text.Length) / 2);
        var top = ConsoleTextComponent.CursorTop;

        ConsoleTextComponent.SetCursorPosition(left, top);
        ConsoleTextComponent.WriteLine(text, color);
    }
}
