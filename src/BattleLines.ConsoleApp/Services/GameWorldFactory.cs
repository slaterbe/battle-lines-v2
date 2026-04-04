using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class GameWorldFactory
{
    private readonly EnemyWaveFactory enemyWaveFactory = new();
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public GameWorld Create()
    {
        var enemyWaves = enemyWaveFactory.CreateGiantRatWaves();
        var gameWorld = new GameWorld
        {
            Villagers = 5,
            Spears = 5,
            VillagerProduction = 2,
            SpearProduction = 1,
            State = GameState.Village,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.SpearmenLvl1] = 5
            },
            EnemyWaveList = enemyWaves,
            TotalWaveCount = enemyWaves.Count,
            MaxArmySize = 8
        };

        gameWorldStatsService.Refresh(gameWorld);

        return gameWorld;
    }
}
