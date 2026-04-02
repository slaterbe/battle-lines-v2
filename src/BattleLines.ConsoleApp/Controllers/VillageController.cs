using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Controllers;

public class VillageController : IGameStateController
{
    private readonly PreparationService preparationService;
    private readonly BattleService battleService;

    public VillageController(PreparationService preparationService, BattleService battleService)
    {
        this.preparationService = preparationService;
        this.battleService = battleService;
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
                battleService.StartBattle(gameWorld);
                return false;
            case 1:
                return true;
            default:
                return false;
        }
    }

    public void Tick(GameWorld gameWorld)
    {
        preparationService.Tick(gameWorld);
    }

    private static IReadOnlyList<string> BuildCommandOptions()
    {
        return ["Start Battle"];
    }
}
