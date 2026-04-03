using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class PostWaveView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var message = gameWorld.LastBattleWon
            ? "Wave Defeated: Claim your reward and prepare for the next wave"
            : "Battle ended. Continue to the next wave.               ";

        Layout.Render(
            gameWorld,
            message,
            ConsoleColor.Yellow,
            commandOptions,
            selectedCommandIndex);
    }
}
