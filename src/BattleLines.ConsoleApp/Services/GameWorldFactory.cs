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
            Spears = 0,
            VillagerProduction = 2,
            SpearProduction = 1,
            FightersCreated = 0,
            AreSpearControlsVisible = false,
            State = GameState.Village,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.Fighter] = 1,
                [UnitType.SpearmenLvl1] = 0
            },
            EnemyWaves = enemyWaves,
            TotalWaveCount = enemyWaves.Waves.Count,
            WavePosition = 0,
            BattlePosition = 0,
            MaxArmySize = 8
        };

        gameWorldStatsService.Refresh(gameWorld);

        return gameWorld;
    }
}
