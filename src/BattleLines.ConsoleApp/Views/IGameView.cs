using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views;

public interface IGameView
{
    void Render(GameWorld gameWorld, IReadOnlyList<string> commandOptions, int selectedCommandIndex);
}
