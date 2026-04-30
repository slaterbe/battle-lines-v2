using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2;

namespace BattleLines.ConsoleApp.Views;

public class BattleOutcomeView : IGameView
{
    private static readonly TimeSpan DefeatFlashInterval = TimeSpan.FromMilliseconds(900);
    private static readonly ComponentsV2.GameTitleComponent GameTitle = new();
    private static readonly ComponentsV2.ResourcePanelComponent ResourcePanel = new();
    private static readonly CurrentWaveComponent CurrentWave = new();
    private static readonly BattleLineComponent BattleLine = new();
    private static readonly PlayerUnitsComponent PlayerUnits = new();
    private static readonly ComponentsV2.CommandMenuComponent CommandMenu = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        const int titleStartX = 0;
        const int titleStartY = 1;
        const int statusStartX = 0;
        const int statusStartY = 3;

        var selectedCommandLabel = GetSelectedCommandLabel(commandOptions, selectedCommandIndex);
        var selectedCommandCost = GetSelectedCommandCost(commandOptions, selectedCommandIndex);
        var selectedCommandSupply = GetSelectedCommandSupply(commandOptions, selectedCommandIndex);
        var selectedCommandIncome = GetSelectedCommandIncome(commandOptions, selectedCommandIndex);

        GameTitle.Render(titleStartX, titleStartY);
        ConsoleTextComponent.SetCursorPosition(statusStartX, statusStartY);
        RenderFlashingDefeat();
        ConsoleTextComponent.WriteLine("Your warband has been driven back.", ConsoleColor.Red);
        ConsoleTextComponent.NewLine();

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
            GameViewLayout.WaveBattlefieldStartY);
        CurrentWave.Render(gameWorld);
        BattleLine.Render(gameWorld, GameViewLayout.HeaderWidth);
        PlayerUnits.Render(gameWorld, selectedCommandLabel);

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

    private static void RenderFlashingDefeat()
    {
        var flashPhase = (Environment.TickCount64 / (long)DefeatFlashInterval.TotalMilliseconds) % 2;
        if (flashPhase == 0)
        {
            ConsoleTextComponent.WriteLine("DEFEAT", ConsoleColor.Red);
            return;
        }

        ConsoleTextComponent.WriteLine("      ");
    }
}
