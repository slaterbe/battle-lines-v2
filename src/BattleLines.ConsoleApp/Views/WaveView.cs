using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class WaveView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<string> commandOptions, int selectedCommandIndex)
    {
        Layout.Render(
            gameWorld,
            "Battle is active. Both sides deal damage each tick.     ",
            ConsoleColor.Yellow,
            commandOptions,
            selectedCommandIndex);
    }
}
