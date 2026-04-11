using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class VillageView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();
    private static readonly PlayerUnitsComponent PlayerUnits = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        Layout.Render(
            gameWorld,
            "The village waits for your command. Prepare the defenses.",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex,
            goalMessage: gameWorld.GoalMessage,
            playerUnitsRenderer: () => PlayerUnits.Render(gameWorld, GetSelectedCommandLabel(commandOptions, selectedCommandIndex)),
            showResources: true,
            showWaveOverview: false,
            showCurrentWave: false,
            showBattleLine: false);
    }

    private static string GetSelectedCommandLabel(
        IReadOnlyList<GameCommandOption> commandOptions,
        int selectedCommandIndex)
    {
        return selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
            ? commandOptions[selectedCommandIndex].Label
            : string.Empty;
    }
}
