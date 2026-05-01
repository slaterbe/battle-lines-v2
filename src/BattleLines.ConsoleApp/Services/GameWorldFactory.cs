using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class GameWorldFactory
{
    private const int StartingFrontLineCapacity = 6;

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
            IsFiveFightersCreated = true,
            FightersCreated = 0,
            Food = 40,
            Villagers = 2,
            Spears = 0,
            Gold = 2,
            FoodProduction = 0,
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
            FrontLineCapacity = StartingFrontLineCapacity,
            GateHouseLevel = 0,
            GoalMessage = "Goal: Defend the village!!!"
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
