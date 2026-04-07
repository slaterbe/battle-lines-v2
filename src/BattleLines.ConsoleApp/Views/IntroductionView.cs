using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class IntroductionView : IGameView
{
    private static readonly GameTitleComponent GameTitle = new();
    private static readonly CommandMenuComponent CommandMenu = new();
    private static readonly string[] IntroductionLines =
    [
        "The village is moments from collapse.\n",
        "Scouts report giant rats surging out of the fields in growing numbers.",
        "You have only a few villagers, almost no weapons, and barely any time.",
        "Recruit fighters, rally the village, and hold the line before everything is lost."
    ];

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        GameTitle.Render();

        Console.WriteLine();
        RenderIntroductionText(gameWorld);
        Console.WriteLine();
        Console.WriteLine();
        CommandMenu.Render(commandOptions, selectedCommandIndex);
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
