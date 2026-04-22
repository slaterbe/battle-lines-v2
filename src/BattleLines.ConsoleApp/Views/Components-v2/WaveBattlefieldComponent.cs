using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class WaveBattlefieldComponent
{
    private readonly WaveOverviewComponent waveOverviewComponent = new();
    private readonly CurrentWaveComponent currentWaveComponent = new();
    private readonly BattleLineComponent battleLineComponent = new();
    private readonly PlayerUnitsComponent playerUnitsComponent = new();

    public void Render(GameWorld gameWorld, string selectedCommandLabel, int startX, int startY, int contentWidth)
    {
        ConsoleTextComponent.SetCursorPosition(startX, startY);
        waveOverviewComponent.Render(gameWorld);
        ConsoleTextComponent.NewLine();

        currentWaveComponent.Render(gameWorld);
        battleLineComponent.Render(gameWorld, contentWidth);
        playerUnitsComponent.Render(gameWorld, selectedCommandLabel);
    }
}
