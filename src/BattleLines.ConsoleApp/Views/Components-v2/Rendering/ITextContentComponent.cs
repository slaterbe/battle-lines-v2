using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.ComponentsV2.Rendering;

public interface ITextContentComponent
{
    IReadOnlyList<string> GetLines(GameWorld gameWorld);
}
