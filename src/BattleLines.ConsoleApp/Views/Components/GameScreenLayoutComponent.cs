using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class GameScreenLayoutComponent
{
    private readonly GameHeaderComponent gameHeaderComponent = new();
    private readonly WaveOverviewComponent waveOverviewComponent = new();
    private readonly CurrentWaveComponent currentWaveComponent = new();
    private readonly PlayerUnitsComponent playerUnitsComponent = new();
    private readonly CommandMenuComponent commandMenuComponent = new();

    public void Render(
        GameWorld gameWorld,
        string statusMessage,
        ConsoleColor statusColor,
        IReadOnlyList<GameCommandOption> commandOptions,
        int selectedCommandIndex,
        string? supplementalDetails = null,
        bool showWaveOverview = true,
        bool showCurrentWave = true)
    {
        var selectedCommandCost =
            selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
                ? commandOptions[selectedCommandIndex].Cost
                : null;

        gameHeaderComponent.Render(gameWorld, statusMessage, statusColor, selectedCommandCost);

        Console.WriteLine();
        if (!string.IsNullOrWhiteSpace(supplementalDetails))
        {
            ConsoleTextComponent.WriteLine(supplementalDetails, ConsoleColor.Cyan);
            Console.WriteLine();
        }

        if (showWaveOverview)
        {
            waveOverviewComponent.Render(gameWorld);
            Console.WriteLine();
        }

        if (showCurrentWave)
        {
            currentWaveComponent.Render(gameWorld);
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine();
        ConsoleTextComponent.WriteLine(playerUnitsComponent.Render(gameWorld), ConsoleColor.Blue);
        Console.WriteLine();
        commandMenuComponent.Render(commandOptions, selectedCommandIndex);
    }
}
