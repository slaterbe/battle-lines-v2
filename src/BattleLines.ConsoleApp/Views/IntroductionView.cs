using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class IntroductionView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();
    private static readonly string[] IntroductionLines =
    [
        "Scouts have spotted giant rats massing beyond the fields.",
        "You have only a handful of villagers and little time to prepare.",
        "Recruit fighters, strengthen the village, and hold the line."
    ];

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        Layout.Render(
            gameWorld,
            "The village braces for the coming attack.",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex,
            supplementalDetailsRenderer: () => RenderIntroductionText(gameWorld),
            playerUnitsRenderer: () => { },
            showResources: false,
            showWaveOverview: false,
            showCurrentWave: false);
    }

    private static void RenderIntroductionText(GameWorld gameWorld)
    {
        foreach (var line in IntroductionLines)
        {
            if (gameWorld.IsIntroductionTextFullyRevealed)
            {
                ConsoleTextComponent.WriteLine(line, ConsoleColor.Green);
                continue;
            }

            ConsoleTextComponent.WriteLineSlow(line, ConsoleColor.Green);
        }

        gameWorld.IsIntroductionTextFullyRevealed = true;
    }
}
