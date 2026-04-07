using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class PreWaveController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld)
    {
        return [new BeginBattleCommand(), new AddFighterCommand(), new AddSpearmanCommand(), new ReturnToVillageCommand()];
    }
}
