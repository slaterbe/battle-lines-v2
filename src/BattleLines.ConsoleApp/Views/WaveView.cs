using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2;

namespace BattleLines.ConsoleApp.Views;

public class WaveView : IGameView
{
    private static readonly ComponentsV2.GameHeaderComponent Header = new();
    private static readonly ComponentsV2.ResourcePanelComponent ResourcePanel = new();
    private static readonly WaveBattlefieldComponent Battlefield = new();
    private static readonly ComponentsV2.CommandMenuComponent CommandMenu = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var selectedCommandLabel = GetSelectedCommandLabel(commandOptions, selectedCommandIndex);
        var selectedCommandCost = GetSelectedCommandCost(commandOptions, selectedCommandIndex);

        Header.Render(
            "The clash is joined. Hold the line as steel and fury collide.",
            ConsoleColor.Green,
            gameWorld.GoalMessage,
            GameViewLayout.LeftColumnStartX,
            GameViewLayout.HeaderStartY,
            GameViewLayout.HeaderWidth);

        ResourcePanel.Render(
            gameWorld,
            selectedCommandCost,
            selectedCommandLabel,
            GameViewLayout.RightColumnStartX,
            GameViewLayout.ResourcePanelStartY,
            GameViewLayout.ResourcePanelWidth);

        Battlefield.Render(
            gameWorld,
            selectedCommandLabel,
            GameViewLayout.LeftColumnStartX,
            GameViewLayout.WaveBattlefieldStartY,
            GameViewLayout.HeaderWidth);

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
}
