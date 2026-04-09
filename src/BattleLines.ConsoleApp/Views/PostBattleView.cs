using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class PostBattleView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var message = gameWorld.LastBattleWon
            ? "The last of the enemy scatters. The village stands, and your warriors breathe again."
            : "Battle Lost: Review the outcome and return to the village";
        var messageColor = gameWorld.LastBattleWon
            ? ConsoleColor.Green
            : ConsoleColor.Yellow;

        Layout.Render(
            gameWorld,
            message,
            messageColor,
            commandOptions,
            selectedCommandIndex,
            showCurrentWave: false,
            showPlayerUnits: false);
    }
}
