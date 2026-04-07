using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class VillageTransitionService
{
    private readonly EnemyWaveFactory enemyWaveFactory = new();
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public void MoveToVillage(GameWorld gameWorld, bool applyProduction, bool advanceBattle = false)
    {
        if (applyProduction)
        {
            gameWorld.Villagers += gameWorld.VillagerProduction;
            gameWorld.Spears += gameWorld.SpearProduction;
        }

        gameWorld.PlayerHealthAtBattleStart = 0;
        gameWorld.PlayerUnitsAtBattleStart.Clear();
        gameWorld.PlayerUnitHistory.Clear();
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
        var targetBattlePosition = advanceBattle
            ? gameWorld.BattlePosition + 1
            : gameWorld.BattlePosition;

        gameWorld.BattlePosition = targetBattlePosition;
        gameWorld.IsSpearControlsVisible = gameWorld.IsSpearControlsVisible || gameWorld.BattlePosition > 1;
        gameWorld.EnemyWaves = enemyWaveFactory.HasBattle(gameWorld.BattlePosition)
            ? enemyWaveFactory.CreateBattle(gameWorld.BattlePosition)
            : new EnemyWaveSetModel();
        gameWorld.TotalWaveCount = gameWorld.EnemyWaves.Waves.Count;
        gameWorld.WavePosition = 0;
        gameWorld.State = GameState.Village;
        gameWorldStatsService.Refresh(gameWorld);
    }
}
