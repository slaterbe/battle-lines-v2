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
            new GatherGoldCommand()
        };

        if (gameWorld.IsUpgradesVisible)
        {
            commands.Add(new IncreaseVillagerProductionCommand());

            if (gameWorld.IsSpearControlsVisible)
            {
                commands.Add(new IncreaseSpearProductionCommand());
            }

            commands.Add(new IncreaseArmySizeCommand());
        }

        return commands;
    }
}
