using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2;

namespace BattleLines.ConsoleApp.Views;

public class VillageView : IGameView
{
    private static readonly ComponentsV2.GameHeaderComponent Header = new();
    private static readonly ComponentsV2.ResourcePanelComponent ResourcePanel = new();
    private static readonly VillagePlayerUnitsComponent PlayerUnits = new();
    private static readonly ComponentsV2.CommandMenuComponent CommandMenu = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        const int headerStartX = 0;
        const int headerStartY = 1;
        const int resourcePanelStartX = -35;
        const int resourcePanelStartY = 1;
        const int playerUnitsStartX = 0;
        const int playerUnitsStartY = 6;
        const int commandMenuStartX = 0;
        const int commandMenuStartY = 14;

        var selectedCommandLabel = GetSelectedCommandLabel(commandOptions, selectedCommandIndex);
        var selectedCommandCost = GetSelectedCommandCost(commandOptions, selectedCommandIndex);
        var resourcePanelLeft = ConsoleRenderLayout.ResolveLeft(resourcePanelStartX, ConsoleTextComponent.WindowWidth);
        var headerMaxWidth = Math.Max(1, resourcePanelLeft - headerStartX - 1);

        Header.Render(
            "The village waits for your command. Prepare the defenses.",
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

        PlayerUnits.Render(gameWorld, selectedCommandLabel, playerUnitsStartX, playerUnitsStartY);

        var commandMenuState = new CommandMenuState(commandOptions, selectedCommandIndex);
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
