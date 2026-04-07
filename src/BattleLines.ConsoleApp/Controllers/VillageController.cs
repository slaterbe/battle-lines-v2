using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class VillageController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld)
    {
        var commands = new List<IGameCommand>
        {
            new StartBattleCommand(),
            new AddFighterCommand(),
            new AddSpearmanCommand()
        };

        if (gameWorld.AreUpgradesAvailable)
        {
            commands.Add(new IncreaseVillagerProductionCommand());
            commands.Add(new IncreaseSpearProductionCommand());
            commands.Add(new IncreaseArmySizeCommand());
        }

        return commands;
    }
}
