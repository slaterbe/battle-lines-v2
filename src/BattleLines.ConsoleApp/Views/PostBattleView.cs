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
            goalMessage: gameWorld.GoalMessage,
            postWaveOverviewRenderer: gameWorld.LastBattleWon && !string.IsNullOrWhiteSpace(gameWorld.EnemyWaves.VictoryMessage)
                ? () => ConsoleTextComponent.WriteWrappedLines(
                    gameWorld.EnemyWaves.VictoryMessage,
                    Math.Max(1, ResourcePanelComponent.GetLeftColumnWidth() - 1),
                    ConsoleColor.Blue)
                : null,
            showCurrentWave: false,
            showPlayerUnits: false);
    }
}
