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
            Waves =
            [
                CreateWave(2, EnemyWaveRewardType.Gold, 5)
            ]
        };
    }

    private static EnemyWaveSetModel CreateBattleOneWaves()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "Your line holds through the assault, and the village stands unbroken.",
            FlashingVictoryMessage = "Unlocked spears",
            Waves =
            [
                CreateWave(3, EnemyWaveRewardType.Gold, 5),
                CreateWave(5, EnemyWaveRewardType.Gold, 10),
                CreateWave(7, EnemyWaveRewardType.Gold, 15),
                CreateWave(9, EnemyWaveRewardType.Gold, 20),
                CreateWave(11, EnemyWaveRewardType.Gold, 25)
            ]
        };
    }

    private static EnemyWaveSetModel CreateBattleTwoWaves()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "The warband shatters against your defense. For now, the frontier is safe.",
            FlashingVictoryMessage = "More to unlock",
            Waves =
            [
                CreateWave(6, EnemyWaveRewardType.Gold, 10),
                CreateWave(8, EnemyWaveRewardType.Spears, 2),
                CreateWave(10, EnemyWaveRewardType.Gold, 15),
                CreateWave(12, EnemyWaveRewardType.Spears, 3),
                CreateWave(14, EnemyWaveRewardType.Gold, 20),
                CreateWave(16, EnemyWaveRewardType.Spears, 4),
                CreateWave(18, EnemyWaveRewardType.Gold, 25),
                CreateWave(20, EnemyWaveRewardType.Spears, 5)
            ]
        };
    }

    private static EnemyWaveModel CreateWave(int enemyCount, EnemyWaveRewardType rewardType, int rewardAmount)
    {
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
            RewardType = rewardType,
            RewardAmount = rewardAmount
        };
    }
}
