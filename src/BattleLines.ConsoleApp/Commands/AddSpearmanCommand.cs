using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class AddSpearmanCommand : IGameCommand
{
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public string Label => "Add to Spearmen";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village && gameWorld.State != GameState.PreBattle)
        {
            return false;
        }

        if (gameWorld.Commoners < 1 || gameWorld.Spears < 1)
        {
            return false;
        }

        gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var currentCount);
        if (currentCount >= gameWorld.MaxSpearmenPositions)
        {
            return false;
        }

        gameWorld.Commoners -= 1;
        gameWorld.Spears -= 1;
        gameWorld.PlayerUnits[UnitType.SpearmenLvl1] = currentCount + 1;
        gameWorldStatsService.Refresh(gameWorld);
        return false;
    }
}
