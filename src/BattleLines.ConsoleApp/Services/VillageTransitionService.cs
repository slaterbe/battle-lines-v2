using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class VillageTransitionService
{
    private readonly EnemyWaveFactory enemyWaveFactory = new();
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public void MoveToVillage(GameWorld gameWorld, bool applyProduction)
    {
        if (applyProduction)
        {
            gameWorld.Villagers += gameWorld.VillagerProduction;
            gameWorld.Spears += gameWorld.SpearProduction;
        }

        gameWorld.PlayerHealthAtBattleStart = 0;
        gameWorld.PlayerUnitsAtBattleStart.Clear();
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
        gameWorld.EnemyWaveList = enemyWaveFactory.CreateGiantRatWaves();
        gameWorld.TotalWaveCount = gameWorld.EnemyWaveList.Count;
        gameWorld.State = GameState.Village;
        gameWorldStatsService.Refresh(gameWorld);
    }
}
