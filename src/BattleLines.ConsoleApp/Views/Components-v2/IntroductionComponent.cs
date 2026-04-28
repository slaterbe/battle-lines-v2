using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.ComponentsV2.Rendering;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class IntroductionComponent : ITextContentComponent
{
    private static readonly string[] IntroductionLines =
    [
        "The village is moments from collapse.",
        string.Empty,
        "Scouts report giant rats surging out of the fields in growing numbers.",
        "You have only a few villagers, almost no weapons, and barely any time.",
        "Recruit fighters, rally the village, and hold the line before everything is lost."
    ];

    public IReadOnlyList<string> GetLines(GameWorld gameWorld)
    {
        return IntroductionLines;
    }
}
