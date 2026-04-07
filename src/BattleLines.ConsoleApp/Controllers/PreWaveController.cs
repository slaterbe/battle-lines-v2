using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class PreWaveController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld)
    {
        var commands = new List<IGameCommand>
        {
            new BeginBattleCommand(),
            new AddFighterCommand()
        };

        if (gameWorld.IsSpearControlsVisible)
        {
            commands.Add(new AddSpearmanCommand());
        }

        commands.Add(new ReturnToVillageCommand());

        return commands;
    }
}
