using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2;

namespace BattleLines.ConsoleApp.Views;

public class VillageView : IGameView
{
    private static readonly ComponentsV2.GameHeaderComponent Header = new();
    private static readonly ComponentsV2.ResourcePanelComponent ResourcePanel = new();
    private static readonly BattleLineComponent BattleLine = new();
    private static readonly VillagePlayerUnitsComponent PlayerUnits = new();
    private static readonly ComponentsV2.CommandMenuComponent CommandMenu = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var selectedCommandLabel = GetSelectedCommandLabel(commandOptions, selectedCommandIndex);
        var selectedCommandCost = GetSelectedCommandCost(commandOptions, selectedCommandIndex);
        var selectedCommandSupply = GetSelectedCommandSupply(commandOptions, selectedCommandIndex);
        var selectedCommandIncome = GetSelectedCommandIncome(commandOptions, selectedCommandIndex);

        Header.Render(
            "The village waits for your command. Prepare the defenses.",
            ConsoleColor.Green,
            gameWorld.GoalMessage,
            GameViewLayout.LeftColumnStartX,
            GameViewLayout.HeaderStartY,
            GameViewLayout.HeaderWidth);

        ResourcePanel.Render(
            gameWorld,
            selectedCommandCost,
            selectedCommandSupply,
            selectedCommandIncome,
            selectedCommandLabel,
            GameViewLayout.RightColumnStartX,
            GameViewLayout.ResourcePanelStartY,
            GameViewLayout.ResourcePanelWidth);

        ConsoleTextComponent.SetCursorPosition(
            GameViewLayout.LeftColumnStartX,
            GameViewLayout.VillageUnitsStartY);
        BattleLine.Render(gameWorld, GameViewLayout.HeaderWidth);

        PlayerUnits.Render(
            gameWorld,
            selectedCommandLabel,
            GameViewLayout.LeftColumnStartX,
            GameViewLayout.VillageUnitsStartY + 2);

        var commandMenuState = new CommandMenuState(commandOptions, selectedCommandIndex);
        var commandMenuStartY = GameViewLayout.GetBottomAnchoredStartY(CommandMenu.MeasureHeight(commandMenuState));
        CommandMenu.Render(
            commandMenuState,
            GameViewLayout.LeftColumnStartX,
            commandMenuStartY);
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

    private static GameCommandCost? GetSelectedCommandSupply(
        IReadOnlyList<GameCommandOption> commandOptions,
        int selectedCommandIndex)
    {
        return selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
            ? commandOptions[selectedCommandIndex].Supply
            : null;
    }

    private static GameCommandCost? GetSelectedCommandIncome(
        IReadOnlyList<GameCommandOption> commandOptions,
        int selectedCommandIndex)
    {
        return selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
            ? commandOptions[selectedCommandIndex].Income
            : null;
    }
}
