using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class GameTitleComponent
{
    private readonly Components.GameTitleComponent innerGameTitleComponent = new();

    public void Render(int startX, int startY)
    {
        ConsoleTextComponent.SetCursorPosition(startX, startY);
        innerGameTitleComponent.Render();
    }
}
