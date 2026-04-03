using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class EnemyWaveFactory
{
    public List<EnemyWaveModel> CreateGiantRatWaves()
    {
        return
        [
            new EnemyWaveModel
            {
                Enemies =
                [
                    new EnemyWaveUnitModel
                    {
                        EnemyType = UnitType.GiantRat,
                        Count = 3
                    }
                ],
                RewardType = EnemyWaveRewardType.Spears,
                RewardAmount = 2
            },
            new EnemyWaveModel
            {
                Enemies =
                [
                    new EnemyWaveUnitModel
                    {
                        EnemyType = UnitType.GiantRat,
                        Count = 5
                    }
                ],
                RewardType = EnemyWaveRewardType.Gold,
                RewardAmount = 10
            },
            new EnemyWaveModel
            {
                Enemies =
                [
                    new EnemyWaveUnitModel
                    {
                        EnemyType = UnitType.GiantRat,
                        Count = 7
                    }
                ],
                RewardType = EnemyWaveRewardType.Gold,
                RewardAmount = 15
            },
            new EnemyWaveModel
            {
                Enemies =
                [
                    new EnemyWaveUnitModel
                    {
                        EnemyType = UnitType.GiantRat,
                        Count = 9
                    }
                ],
                RewardType = EnemyWaveRewardType.Spears,
                RewardAmount = 5
            },
            new EnemyWaveModel
            {
                Enemies =
                [
                    new EnemyWaveUnitModel
                    {
                        EnemyType = UnitType.GiantRat,
                        Count = 11
                    }
                ],
                RewardType = EnemyWaveRewardType.Spears,
                RewardAmount = 25
            }
        ];
    }
}
