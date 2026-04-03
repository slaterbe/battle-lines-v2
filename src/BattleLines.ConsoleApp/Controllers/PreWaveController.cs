using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Controllers;

public class PreWaveController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands()
    {
        return [new BeginBattleCommand(), new AddSpearmanCommand(), new ResetCurrentWaveCommand()];
    }
}
