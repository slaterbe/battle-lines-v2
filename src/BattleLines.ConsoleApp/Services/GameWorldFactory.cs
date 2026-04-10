using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class GameWorldFactory
{
    private readonly EnemyWaveFactory enemyWaveFactory = new();
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public GameWorld Create(bool skipIntroduction = false)
    {
        const int startingBattlePosition = 0;
        var enemyWaves = enemyWaveFactory.CreateBattle(startingBattlePosition);
        var gameWorld = new GameWorld
        {
            IsSkipIntroduction = skipIntroduction,
            IsSpearControlsVisible = false,
            IsUpgradesVisible = false,
            IsIntroductionTextFullyRevealed = false,
            IsFiveFightersCreated = false,
            FightersCreated = 0,
            Villagers = 5,
            Spears = 0,
            VillagerProduction = 2,
            SpearProduction = 0,
            State = GameState.Introduction,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.Fighter] = 1,
                [UnitType.SpearmenLvl1] = 0
            },
            EnemyWaves = enemyWaves,
            TotalWaveCount = enemyWaves.Waves.Count,
            WavePosition = 0,
            BattlePosition = startingBattlePosition,
            MaxArmySize = 8
        };

        if (gameWorld.IsSkipIntroduction)
        {
            gameWorld.WavePosition = 1;
            gameWorld.State = GameState.PreBattle;
        }

        gameWorldStatsService.Refresh(gameWorld);

        return gameWorld;
    }
}
