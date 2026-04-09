using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class GameHeaderComponent
{
    private readonly GameTitleComponent gameTitleComponent = new();

    public void Render(string statusMessage, ConsoleColor statusColor, string? goalMessage = null)
    {
        ConsoleTextComponent.SetCursorPosition(0, 0);

        gameTitleComponent.Render();

        ConsoleTextComponent.WriteLine(statusMessage, statusColor);

        if (!string.IsNullOrWhiteSpace(goalMessage))
        {
            ConsoleTextComponent.WriteLine(goalMessage, ConsoleColor.Yellow);
        }
    }
}
