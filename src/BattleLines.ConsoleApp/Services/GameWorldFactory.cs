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
            IsSkipIntroduction = false,
            IsSpearControlsVisible = false,
            IsUpgradesVisible = false,
            IsIntroductionTextFullyRevealed = false,
            FightersCreated = 0,
            Villagers = 5,
            Spears = 0,
            VillagerProduction = 2,
            SpearProduction = 1,
            State = GameState.Introduction,
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

        if (gameWorld.IsSkipIntroduction)
        {
            gameWorld.State = GameState.Village;
        }

        gameWorldStatsService.Refresh(gameWorld);

        return gameWorld;
    }
}
