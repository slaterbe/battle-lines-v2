using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class VillageController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld)
    {
        return
        [
            new StartBattleCommand(),
            new BuyVillageCommand(),
            new IncreaseFoodProductionCommand(),
            new IncreaseSpearProductionCommand(),
            new IncreaseArmySizeCommand()
        ];
    }
}
