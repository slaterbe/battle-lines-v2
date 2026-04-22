using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class PostBattleSummaryComponent
{
    private readonly WaveOverviewComponent waveOverviewComponent = new();

    public void Render(GameWorld gameWorld, int startX, int startY, int contentWidth)
    {
        ConsoleTextComponent.SetCursorPosition(startX, startY);
        waveOverviewComponent.Render(gameWorld);

        if (gameWorld.LastBattleWon && PostBattleView.HasPostBattleVictoryContent(gameWorld.EnemyWaves))
        {
            PostBattleView.RenderVictoryMessages(gameWorld.EnemyWaves, Math.Max(1, contentWidth - startX - 1));
        }
    }
}
