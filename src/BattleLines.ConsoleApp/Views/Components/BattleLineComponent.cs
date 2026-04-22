using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class BattleLineComponent
{
    public void Render(GameWorld gameWorld)
    {
        Render(gameWorld, ResourcePanelComponent.GetLeftColumnWidth());
    }

    public void Render(GameWorld gameWorld, int contentWidth)
    {
        if (ShouldShowEnemyArmy(gameWorld))
        {
            WriteCenteredLine(RenderEnemyArmyLine(gameWorld), ConsoleColor.Red, contentWidth);
        }

        WriteCenteredLine(RenderPlayerArmyLine(gameWorld), ConsoleColor.Blue, contentWidth);
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

    private static void WriteCenteredLine(string text, ConsoleColor color, int contentWidth)
    {
        var effectiveContentWidth = Math.Max(1, contentWidth);
        var left = Math.Max(0, (effectiveContentWidth - text.Length) / 2);
        var top = ConsoleTextComponent.CursorTop;

        ConsoleTextComponent.SetCursorPosition(left, top);
        ConsoleTextComponent.WriteLine(text, color);
    }
}
