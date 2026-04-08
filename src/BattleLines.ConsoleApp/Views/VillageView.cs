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
        var selectedCommandLabel =
            selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count
                ? commandOptions[selectedCommandIndex].Label
                : string.Empty;

        Layout.Render(
            gameWorld,
            "The village waits for your command. Prepare the defenses.",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex,
            supplementalDetailsRenderer: () => RenderSupplementalDetails(selectedCommandLabel),
            playerUnitsRenderer: () => PlayerUnits.Render(gameWorld, selectedCommandLabel),
            showResources: true,
            showWaveOverview: false,
            showCurrentWave: false);
    }

    private static void RenderSupplementalDetails(
        string selectedCommandLabel)
    {
        ConsoleTextComponent.WriteLine("Village Status", ConsoleColor.DarkYellow);
        ConsoleTextComponent.NewLine();
        WriteVillageStatLine("Selection", GetSelectionPreview(selectedCommandLabel), false);
    }

    private static void WriteVillageStatLine(string label, string value, bool showIncrease)
    {
        ConsoleTextComponent.Write($"{label}: {value}", ConsoleColor.Cyan);

        if (showIncrease)
        {
            ConsoleTextComponent.Write(" [+1]", ConsoleColor.Green);
        }

        ConsoleTextComponent.NewLine();
    }

    private static string GetSelectionPreview(string selectedCommandLabel)
    {
        return selectedCommandLabel switch
        {
            "Boost Army Size" => "Raise army capacity by 1",
            "Boost Villagers" => "Increase villager production by 1",
            "Boost Spears" => "Increase spear production by 1",
            "Recruit Fighter" => "Train 1 fighter for the next battle",
            "Recruit Spearmen" => "Train 1 spearman for the next battle",
            _ => "Review the army, then choose the next order."
        };
    }
}
