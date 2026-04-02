using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class PreparationService
{
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public void Tick(GameWorld gameWorld)
    {
        // Resources no longer increment during preparation.
    }

    public void AddSpearman(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village && gameWorld.State != GameState.PreBattle)
        {
            return;
        }

        if (gameWorld.Commoners < 1 || gameWorld.Spears < 1)
        {
            return;
        }

        gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var currentCount);
        if (currentCount >= gameWorld.MaxSpearmenPositions)
        {
            return;
        }

        gameWorld.Commoners -= 1;
        gameWorld.Spears -= 1;
        gameWorld.PlayerUnits[UnitType.SpearmenLvl1] = currentCount + 1;
        gameWorldStatsService.Refresh(gameWorld);
    }
}
