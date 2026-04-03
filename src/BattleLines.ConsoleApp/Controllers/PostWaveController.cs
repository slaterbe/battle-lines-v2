using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Controllers;

public class PostWaveController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands()
    {
        return [new ExitPostBattleCommand()];
    }
}
