using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class VillageHeaderComponent
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
        ConsoleTextComponent.SetCursorPosition(startX, startY);

        if (!string.IsNullOrWhiteSpace(goalMessage))
        {
            ConsoleTextComponent.WriteLine(goalMessage, ConsoleColor.Yellow);
        }

        gameTitleComponent.Render(startX, ConsoleTextComponent.CursorTop);
        ConsoleTextComponent.WriteWrappedLines(statusMessage, Math.Max(1, maxWidth), statusColor);
    }
}
