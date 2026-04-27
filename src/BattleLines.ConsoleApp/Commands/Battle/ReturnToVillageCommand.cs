using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class ReturnToVillageCommand : IGameCommand
{
    private readonly VillageTransitionService villageTransitionService = new();

    public GameCommandCategory Category => GameCommandCategory.Retreat;
    public string Label => "Back to Village";
    public string GetHelpText() => "Leave battle prep and return to the village screen.";
    public bool IsDisabled(GameWorld gameWorld) => gameWorld.State != GameState.PreBattle;

    public bool Execute(GameWorld gameWorld)
    {
        var applyProduction = gameWorld.WavePosition > 1;
        villageTransitionService.MoveToVillage(gameWorld, applyProduction: applyProduction);
        return false;
    }
}
