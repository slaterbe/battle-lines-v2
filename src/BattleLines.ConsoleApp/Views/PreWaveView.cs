using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2;

namespace BattleLines.ConsoleApp.Views;

public class PreWaveView : IGameView
{
    private static readonly ComponentsV2.GameHeaderComponent Header = new();
    private static readonly ComponentsV2.ResourcePanelComponent ResourcePanel = new();
    private static readonly WaveBattlefieldComponent Battlefield = new();
    private static readonly ComponentsV2.CommandMenuComponent CommandMenu = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        const int headerStartX = 0;
        const int headerStartY = 1;
        const int resourcePanelStartX = -35;
        const int resourcePanelStartY = 1;
        const int battlefieldStartX = 0;
        const int commandMenuStartX = 0;

        var selectedCommandLabel = GetSelectedCommandLabel(commandOptions, selectedCommandIndex);
        var selectedCommandCost = GetSelectedCommandCost(commandOptions, selectedCommandIndex);
        var resourcePanelLeft = ConsoleRenderLayout.ResolveLeft(resourcePanelStartX, ConsoleTextComponent.WindowWidth);
        var headerMaxWidth = Math.Max(1, resourcePanelLeft - headerStartX - 1);

        Header.Render(
            "Steel your nerve. Rally your warriors and meet the enemy head-on.",
            ConsoleColor.Green,
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

        var battlefieldStartY = ConsoleTextComponent.CursorTop + 1;
        Battlefield.Render(
            gameWorld,
            selectedCommandLabel,
            battlefieldStartX,
            battlefieldStartY,
            resourcePanelLeft);

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
}
