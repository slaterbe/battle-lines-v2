using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class EnemyWaveFactory
{
    public EnemyWaveSetModel CreateBattle(int battlePosition)
    {
        return battlePosition switch
        {
            1 => CreateBattleOneWaves(),
            2 => CreateBattleTwoWaves(),
            _ => new EnemyWaveSetModel()
        };
    }

    public bool HasBattle(int battlePosition)
    {
        return battlePosition is 1 or 2;
    }

    private static EnemyWaveSetModel CreateBattleOneWaves()
    {
        return new EnemyWaveSetModel
        {
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
