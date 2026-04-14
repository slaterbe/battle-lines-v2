using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class GameHeaderComponent
{
    private readonly GameTitleComponent gameTitleComponent = new();

    public void Render(string statusMessage, ConsoleColor statusColor, string? goalMessage = null)
    {
        ConsoleTextComponent.SetCursorPosition(0, 1);

        if (!string.IsNullOrWhiteSpace(goalMessage))
        {
            ConsoleTextComponent.WriteLine(goalMessage, ConsoleColor.Yellow);
        }

        gameTitleComponent.Render();

        var maxWidth = Math.Max(1, ResourcePanelComponent.GetLeftColumnWidth() - 1);
        ConsoleTextComponent.WriteWrappedLines(statusMessage, maxWidth, statusColor);
    }
}
