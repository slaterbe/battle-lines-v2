using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public interface IGameStateController
{
    IReadOnlyList<GameCommandOption> GetCommandOptions(GameWorld gameWorld);

    bool HandleCommand(GameWorld gameWorld, int selectedCommandIndex);

    void Tick(GameWorld gameWorld);
}
