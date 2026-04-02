using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class GameWorldFactory
{
    private readonly EnemyWaveFactory enemyWaveFactory = new();
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public GameWorld Create()
    {
        var gameWorld = new GameWorld
        {
            Commoners = 10,
            Spears = 5,
            CommonerProduction = 2,
            SpearProduction = 1,
            State = GameState.Village,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.SpearmenLvl1] = 5
            },
            EnemyWaveList = enemyWaveFactory.CreateGiantRatWaves(5)
        };

        gameWorldStatsService.Refresh(gameWorld);

        return gameWorld;
    }
}
