using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class IntroductionComponent
{
    private static readonly string[] IntroductionLines =
    [
        "The village is moments from collapse.",
        string.Empty,
        "Scouts report giant rats surging out of the fields in growing numbers.",
        "You have only a few villagers, almost no weapons, and barely any time.",
        "Recruit fighters, rally the village, and hold the line before everything is lost."
    ];

    public void Render(GameWorld gameWorld, int startX, int startY)
    {
        ConsoleTextComponent.SetCursorPosition(startX, startY);
        RenderIntroductionText(gameWorld, startX);
    }

    private static void RenderIntroductionText(GameWorld gameWorld, int startX)
    {
        var maxWidth = Math.Max(1, ConsoleTextComponent.WindowWidth - startX);

        foreach (var line in IntroductionLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                ConsoleTextComponent.NewLine();
                continue;
            }

            if (gameWorld.IsIntroductionTextFullyRevealed)
            {
                ConsoleTextComponent.WriteWrappedLines(line, maxWidth, ConsoleColor.Green);
                continue;
            }

            ConsoleTextComponent.WriteLineSlow(line, ConsoleColor.Green);
        }

        gameWorld.IsIntroductionTextFullyRevealed = true;
    }
}
