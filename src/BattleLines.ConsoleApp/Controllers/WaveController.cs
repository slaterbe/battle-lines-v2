using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Controllers;

public class WaveController : IGameStateController
{
    private readonly BattleService battleService;

    public WaveController(BattleService battleService)
    {
        this.battleService = battleService;
    }

    public IReadOnlyList<string> GetCommandOptions()
    {
        return BuildCommandOptions();
    }

    public bool HandleCommand(GameWorld gameWorld, int selectedCommandIndex)
    {
        return selectedCommandIndex == 0;
    }

    public void Tick(GameWorld gameWorld)
    {
        battleService.ResolveBattleTick(gameWorld);
    }

    private static IReadOnlyList<string> BuildCommandOptions()
    {
        return [];
    }
}
