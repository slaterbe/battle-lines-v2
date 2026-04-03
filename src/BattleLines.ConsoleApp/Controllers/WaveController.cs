using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Controllers;

public class WaveController : GameStateControllerBase
{
    public WaveController()
        : base(new ResolveBattleTickCommand())
    {
    }

    protected override IReadOnlyList<IGameCommand> CreateCommands()
    {
        return [];
    }
}
