using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class GameScreenLayoutComponent
{
    private readonly GameHeaderComponent gameHeaderComponent = new();
    private readonly ResourcePanelComponent resourcePanelComponent = new();
    private readonly WaveOverviewComponent waveOverviewComponent = new();
    private readonly CurrentWaveComponent currentWaveComponent = new();
    private readonly BattleLineComponent battleLineComponent = new();
    private readonly PlayerUnitsComponent playerUnitsComponent = new();
    private readonly CommandMenuComponent commandMenuComponent = new();

    public void Render(
        GameWorld gameWorld,
        string statusMessage,
        ConsoleColor statusColor,
        IReadOnlyList<GameCommandOption> commandOptions,
        int selectedCommandIndex,
        string? goalMessage = null,
        string? supplementalDetails = null,
        Action? supplementalDetailsRenderer = null,
        Action? postWaveOverviewRenderer = null,
        Action? playerUnitsRenderer = null,
        bool showResources = true,
        bool showWaveOverview = true,
        bool showCurrentWave = true,
        bool showPlayerUnits = true,
        bool showBattleLine = true)
    {
        var selectedCommandLabel =
            selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
                ? commandOptions[selectedCommandIndex].Label
                : string.Empty;
        var selectedCommandCost =
            selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
                ? commandOptions[selectedCommandIndex].Cost
                : null;

        gameHeaderComponent.Render(statusMessage, statusColor, goalMessage);

        var leftColumnTop = ConsoleTextComponent.CursorTop;
        if (showResources)
        {
            resourcePanelComponent.Render(gameWorld, selectedCommandCost, selectedCommandLabel);
            ConsoleTextComponent.SetCursorPosition(0, leftColumnTop);
        }

        ConsoleTextComponent.NewLine();
        if (supplementalDetailsRenderer is not null)
        {
            supplementalDetailsRenderer();
            ConsoleTextComponent.NewLine();
        }
        else if (!string.IsNullOrWhiteSpace(supplementalDetails))
        {
            ConsoleTextComponent.WriteLine(supplementalDetails, ConsoleColor.Cyan);
            ConsoleTextComponent.NewLine();
        }

        if (showWaveOverview)
        {
            waveOverviewComponent.Render(gameWorld);

            if (postWaveOverviewRenderer is not null)
            {
                ConsoleTextComponent.NewLine();
                ConsoleTextComponent.NewLine();
                postWaveOverviewRenderer();
                ConsoleTextComponent.NewLine();
            }

            ConsoleTextComponent.NewLine();
        }

        if (showCurrentWave)
        {
            currentWaveComponent.Render(gameWorld);
        }

        if (showBattleLine && showCurrentWave && showPlayerUnits)
        {
            ConsoleTextComponent.NewLine();
            battleLineComponent.Render(gameWorld);
        }

        if (showPlayerUnits)
        {
            ConsoleTextComponent.NewLine();
            if (playerUnitsRenderer is not null)
            {
                playerUnitsRenderer();
            }
            else
            {
                playerUnitsComponent.Render(gameWorld, selectedCommandLabel);
            }
        }

        var commandMenuHeight = commandMenuComponent.MeasureHeight(commandOptions, selectedCommandIndex);
        var commandTop = Math.Max(
            ConsoleTextComponent.CursorTop + 1,
            ConsoleTextComponent.WindowHeight - commandMenuHeight);

        ConsoleTextComponent.SetCursorPosition(0, commandTop);
        commandMenuComponent.Render(commandOptions, selectedCommandIndex);
    }
}
