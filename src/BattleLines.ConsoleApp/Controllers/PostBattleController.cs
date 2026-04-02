using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Controllers;

public class PostBattleController : IGameStateController
{
    private readonly PostBattleService postBattleService;

    public PostBattleController(PostBattleService postBattleService)
    {
        this.postBattleService = postBattleService;
    }

    public IReadOnlyList<string> GetCommandOptions()
    {
        return BuildCommandOptions();
    }

    public bool HandleCommand(GameWorld gameWorld, int selectedCommandIndex)
    {
        switch (selectedCommandIndex)
        {
            case 0:
                postBattleService.ExitBattleScreen(gameWorld);
                return false;
            case 1:
                return true;
            default:
                return false;
        }
    }

    public void Tick(GameWorld gameWorld)
    {
    }

    private static IReadOnlyList<string> BuildCommandOptions()
    {
        return ["Continue"];
    }
}
