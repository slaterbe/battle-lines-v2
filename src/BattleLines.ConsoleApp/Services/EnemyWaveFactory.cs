using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class EnemyWaveFactory
{
    public EnemyWaveSetModel CreateBattle(int battlePosition)
    {
        return battlePosition switch
        {
            0 => CreateOpeningBattleWaves(),
            1 => CreateBattleOneWaves(),
            2 => CreateBattleTwoWaves(),
            _ => new EnemyWaveSetModel()
        };
    }

    public bool HasBattle(int battlePosition)
    {
        return battlePosition is 0 or 1 or 2;
    }

    private static EnemyWaveSetModel CreateOpeningBattleWaves()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "The first swarm breaks beneath your defense, and the village dares to hope.",
            FlashingVictoryMessage = "Unlocked Village upgrades",
            DetailedVictoryMessage = "Invest in stronger village production before the next assault arrives.",
            Waves =
            [
                CreateWave(2, 10, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 5))
            ]
        };
    }

    private static EnemyWaveSetModel CreateBattleOneWaves()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "Your line holds through the assault, and the village stands unbroken.",
            FlashingVictoryMessage = "Unlocked spears",
            DetailedVictoryMessage = "Train spearmen to add a harder-hitting unit to your battle line.",
            Waves =
            [
                CreateWave(3, 10, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 5)),
                CreateWave(5, 15, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 10)),
                CreateWave(7, 20, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 15)),
                CreateWave(9, 25, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 20)),
                CreateWave(11, 30, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 25))
            ]
        };
    }

    private static EnemyWaveSetModel CreateBattleTwoWaves()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "The warband shatters against your defense. For now, the frontier is safe.",
            FlashingVictoryMessage = "More to unlock",
            DetailedVictoryMessage = "This victory marks the current frontier. More upgrades and enemies can be added next.",
            Waves =
            [
                CreateWave(6, 10, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 10)),
                CreateWave(8, 15, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 2)),
                CreateWave(10, 20, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 15)),
                CreateWave(12, 25, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 3)),
                CreateWave(14, 30, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 20)),
                CreateWave(16, 35, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 4)),
                CreateWave(18, 40, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 25)),
                CreateWave(20, 45, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 5))
            ]
        };
    }

    private static EnemyWaveModel CreateWave(int enemyCount, int foodRewardAmount, params EnemyWaveRewardModel[] additionalRewards)
    {
        var rewards = new List<EnemyWaveRewardModel>
        {
            new(EnemyWaveRewardType.Food, foodRewardAmount)
        };
        rewards.AddRange(additionalRewards);

        return new EnemyWaveModel
        {
            Enemies =
            [
                new EnemyWaveUnitModel
                {
                    EnemyType = UnitType.GiantRat,
                    Count = enemyCount
                }
            ],
            Rewards = rewards
        };
    }
}
