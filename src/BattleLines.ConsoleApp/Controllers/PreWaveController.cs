using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Controllers;

public class PreWaveController : IGameStateController
{
    private readonly BattleService battleService;
    private readonly PreparationService preparationService;

    public PreWaveController(BattleService battleService, PreparationService preparationService)
    {
        this.battleService = battleService;
        this.preparationService = preparationService;
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
                battleService.BeginBattle(gameWorld);
                return false;
            case 1:
                preparationService.AddSpearman(gameWorld);
                return false;
            case 2:
                gameWorld.State = GameState.Village;
                return false;
            default:
                return false;
        }
    }

    public void Tick(GameWorld gameWorld)
    {
    }

    private static IReadOnlyList<string> BuildCommandOptions()
    {
        return ["Fight Wave", "Add to Spearmen", "Back to Village"];
    }
}
