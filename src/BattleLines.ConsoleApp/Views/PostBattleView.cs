using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class PostBattleView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();
    private static readonly TimeSpan VictoryFlashInterval = TimeSpan.FromMilliseconds(850);

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
            postWaveOverviewRenderer: gameWorld.LastBattleWon &&
                                      HasPostBattleVictoryContent(gameWorld.EnemyWaves)
                ? () => RenderVictoryMessages(gameWorld.EnemyWaves)
                : null,
            showCurrentWave: false,
            showPlayerUnits: false);
    }

    private static bool HasPostBattleVictoryContent(EnemyWaveSetModel enemyWaves)
    {
        return !string.IsNullOrWhiteSpace(enemyWaves.FlavourVictoryMessage) ||
               !string.IsNullOrWhiteSpace(enemyWaves.FlashingVictoryMessage);
    }

    private static void RenderVictoryMessages(EnemyWaveSetModel enemyWaves)
    {
        var maxWidth = Math.Max(1, ResourcePanelComponent.GetLeftColumnWidth() - 1);

        if (!string.IsNullOrWhiteSpace(enemyWaves.FlavourVictoryMessage))
        {
            ConsoleTextComponent.WriteWrappedLines(
                enemyWaves.FlavourVictoryMessage,
                maxWidth,
                ConsoleColor.Green);

            if (!string.IsNullOrWhiteSpace(enemyWaves.FlashingVictoryMessage))
            {
                ConsoleTextComponent.NewLine();
            }
        }

        if (!string.IsNullOrWhiteSpace(enemyWaves.FlashingVictoryMessage))
        {
            RenderFlashingVictoryMessage(enemyWaves.FlashingVictoryMessage, maxWidth);
        }
    }

    private static void RenderFlashingVictoryMessage(string flashingVictoryMessage, int maxWidth)
    {
        var wrappedLines = WrapVictoryMessage(flashingVictoryMessage, maxWidth);
        var flashPhase = (Environment.TickCount64 / (long)VictoryFlashInterval.TotalMilliseconds) % 2;

        foreach (var line in wrappedLines)
        {
            if (flashPhase == 0)
            {
                ConsoleTextComponent.WriteLine(line, ConsoleColor.Blue);
                continue;
            }

            ConsoleTextComponent.WriteLine(new string(' ', line.Length));
        }
    }

    private static IReadOnlyList<string> WrapVictoryMessage(string text, int maxWidth)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [string.Empty];
        }

        var wrappedLines = new List<string>();
        var effectiveWidth = Math.Max(1, maxWidth);

        foreach (var rawLine in text.Replace("\r", string.Empty).Split('\n'))
        {
            var remaining = rawLine.Trim();
            if (remaining.Length == 0)
            {
                wrappedLines.Add(string.Empty);
                continue;
            }

            while (remaining.Length > effectiveWidth)
            {
                var splitIndex = remaining.LastIndexOf(' ', effectiveWidth);
                if (splitIndex <= 0)
                {
                    splitIndex = effectiveWidth;
                }

                wrappedLines.Add(remaining[..splitIndex].TrimEnd());
                remaining = remaining[splitIndex..].TrimStart();
            }

            wrappedLines.Add(remaining);
        }

        return wrappedLines;
    }
}
