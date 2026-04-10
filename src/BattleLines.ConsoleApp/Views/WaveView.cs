using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class WaveView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        Layout.Render(
            gameWorld,
            "The clash is joined. Hold the line as steel and fury collide.",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex,
            goalMessage: gameWorld.GoalMessage);
    }
}
