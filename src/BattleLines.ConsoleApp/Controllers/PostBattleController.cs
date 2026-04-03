using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Controllers;

public class PostBattleController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands()
    {
        return [new ExitPostBattleCommand()];
    }
}
