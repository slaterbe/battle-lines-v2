using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class ReturnToVillageCommand : IGameCommand
{
    private readonly VillageTransitionService villageTransitionService = new();

    public GameCommandCategory Category => GameCommandCategory.Battle;
    public string Label => "Back to Village";
    public string HelpText => "Leave battle prep and return to the village screen.";

    public bool Execute(GameWorld gameWorld)
    {
        villageTransitionService.MoveToVillage(gameWorld, applyProduction: true);
        return false;
    }
}
