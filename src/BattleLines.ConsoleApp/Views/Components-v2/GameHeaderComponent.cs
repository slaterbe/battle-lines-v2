using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class GameHeaderComponent
{
    private readonly GameTitleComponent gameTitleComponent = new();

    public void Render(
        string statusMessage,
        ConsoleColor statusColor,
        string goalMessage,
        int startX,
        int startY,
        int maxWidth)
    {
        var goalRowCount = string.IsNullOrWhiteSpace(goalMessage) ? 0 : 1;
        var titleStartY = startY + goalRowCount;
        var statusStartY = titleStartY + 1;

        if (!string.IsNullOrWhiteSpace(goalMessage))
        {
            ConsoleTextComponent.SetCursorPosition(startX, startY);
            ConsoleTextComponent.WriteLine(goalMessage, ConsoleColor.Yellow);
        }

        gameTitleComponent.Render(startX, titleStartY);
        ConsoleTextComponent.SetCursorPosition(startX, statusStartY);
        ConsoleTextComponent.WriteWrappedLines(statusMessage, Math.Max(1, maxWidth), statusColor);
    }
}
