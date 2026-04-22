using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2;

namespace BattleLines.ConsoleApp.Views;

public class PostBattleView : IGameView
{
    private static readonly ComponentsV2.GameHeaderComponent Header = new();
    private static readonly ComponentsV2.ResourcePanelComponent ResourcePanel = new();
    private static readonly PostBattleSummaryComponent Summary = new();
    private static readonly ComponentsV2.CommandMenuComponent CommandMenu = new();
    private static readonly TimeSpan VictoryFlashInterval = TimeSpan.FromMilliseconds(850);

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var message = gameWorld.LastBattleWon
            ? GetVictoryFlavourText(gameWorld.EnemyWaves)
            : "Battle Lost: Review the outcome and return to the village";
        var messageColor = gameWorld.LastBattleWon
            ? ConsoleColor.Green
            : ConsoleColor.Yellow;

        const int headerStartX = 0;
        const int headerStartY = 1;
        const int resourcePanelStartX = -35;
        const int resourcePanelStartY = 1;
        const int summaryStartX = 0;
        const int commandMenuStartX = 0;

        var selectedCommandLabel = GetSelectedCommandLabel(commandOptions, selectedCommandIndex);
        var selectedCommandCost = GetSelectedCommandCost(commandOptions, selectedCommandIndex);
        var resourcePanelLeft = ConsoleRenderLayout.ResolveLeft(resourcePanelStartX, ConsoleTextComponent.WindowWidth);
        var headerMaxWidth = Math.Max(1, resourcePanelLeft - headerStartX - 1);

        Header.Render(
            message,
            messageColor,
            gameWorld.GoalMessage,
            headerStartX,
            headerStartY,
            headerMaxWidth);

        ResourcePanel.Render(
            gameWorld,
            selectedCommandCost,
            selectedCommandLabel,
            resourcePanelStartX,
            resourcePanelStartY);

        var summaryStartY = ConsoleTextComponent.CursorTop;
        Summary.Render(gameWorld, summaryStartX, summaryStartY, resourcePanelLeft);

        var commandMenuState = new CommandMenuState(commandOptions, selectedCommandIndex);
        var commandMenuHeight = CommandMenu.MeasureHeight(commandMenuState);
        var commandMenuStartY = Math.Min(
            ConsoleTextComponent.CursorTop + 1,
            ConsoleTextComponent.WindowHeight - commandMenuHeight);
        CommandMenu.Render(commandMenuState, commandMenuStartX, commandMenuStartY);
    }

    private static string GetSelectedCommandLabel(
        IReadOnlyList<GameCommandOption> commandOptions,
        int selectedCommandIndex)
    {
        return selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
            ? commandOptions[selectedCommandIndex].Label
            : string.Empty;
    }

    private static GameCommandCost? GetSelectedCommandCost(
        IReadOnlyList<GameCommandOption> commandOptions,
        int selectedCommandIndex)
    {
        return selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
            ? commandOptions[selectedCommandIndex].Cost
            : null;
    }

    private static string GetVictoryFlavourText(EnemyWaveSetModel enemyWaves)
    {
        return !string.IsNullOrWhiteSpace(enemyWaves.FlavourVictoryMessage)
            ? enemyWaves.FlavourVictoryMessage
            : "The last of the enemy scatters. The village stands, and your warriors breathe again.";
    }

    public static bool HasPostBattleVictoryContent(EnemyWaveSetModel enemyWaves)
    {
        return !string.IsNullOrWhiteSpace(enemyWaves.FlashingVictoryMessage) ||
               !string.IsNullOrWhiteSpace(enemyWaves.DetailedVictoryMessage);
    }

    public static void RenderVictoryMessages(EnemyWaveSetModel enemyWaves, int maxWidth)
    {
        if (!string.IsNullOrWhiteSpace(enemyWaves.FlashingVictoryMessage))
        {
            RenderFlashingVictoryMessage(enemyWaves.FlashingVictoryMessage, maxWidth);
        }

        if (!string.IsNullOrWhiteSpace(enemyWaves.DetailedVictoryMessage))
        {
            if (!string.IsNullOrWhiteSpace(enemyWaves.FlashingVictoryMessage))
            {
                ConsoleTextComponent.NewLine();
            }

            ConsoleTextComponent.WriteWrappedLines(
                enemyWaves.DetailedVictoryMessage,
                maxWidth,
                ConsoleColor.White);
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
