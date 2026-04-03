using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Controllers;

public class VillageController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands()
    {
        return
        [
            new StartBattleCommand(),
            new AddSpearmanCommand(),
            new IncreaseCommonerProductionCommand(),
            new IncreaseSpearProductionCommand(),
            new IncreaseSpearmenCapacityCommand()
        ];
    }
}
