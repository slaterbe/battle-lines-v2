using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class PreWaveView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        Layout.Render(
            gameWorld,
            "Steel your nerve. Rally your warriors and meet the enemy head-on.",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex);
    }
}
