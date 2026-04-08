using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class GameHeaderComponent
{
    private readonly GameTitleComponent gameTitleComponent = new();

    public void Render(string statusMessage, ConsoleColor statusColor)
    {
        ConsoleTextComponent.SetCursorPosition(0, 0);

        gameTitleComponent.Render();

        ConsoleTextComponent.WriteLine(statusMessage, statusColor);
    }
}
