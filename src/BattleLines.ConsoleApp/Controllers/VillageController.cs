using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class VillageController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld)
    {
        var commands = new List<IGameCommand>
        {
            new AddFighterCommand()
        };

        if (gameWorld.FightersCreated >= 5)
        {
            commands.Insert(0, new StartBattleCommand());
        }

        if (gameWorld.IsSpearControlsVisible)
        {
            commands.Add(new AddSpearmanCommand());
        }

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
