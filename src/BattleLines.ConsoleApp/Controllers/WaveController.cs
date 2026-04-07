using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class WaveController : GameStateControllerBase
{
    public WaveController()
        : base(new ResolveBattleTickCommand())
    {
    }

    protected override IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld)
    {
        return [];
    }
}
