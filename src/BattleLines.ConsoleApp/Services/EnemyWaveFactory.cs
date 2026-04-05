using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class EnemyWaveFactory
{
    public EnemyWaveSetModel CreateGiantRatWaves()
    {
        return new EnemyWaveSetModel
        {
            FinalRewardType = EnemyWaveRewardType.Gold,
            FinalRewardAmount = 100,
            Waves =
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
                    RewardAmount = 5
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
                    RewardType = EnemyWaveRewardType.Spears,
                    RewardAmount = 4
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
                    RewardType = EnemyWaveRewardType.Gold,
                    RewardAmount = 7
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
                    RewardAmount = 5
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 13
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
                            Count = 15
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Spears,
                    RewardAmount = 7
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 18
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Gold,
                    RewardAmount = 8
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 21
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Spears,
                    RewardAmount = 13
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 24
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Gold,
                    RewardAmount = 9
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 27
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Spears,
                    RewardAmount = 10
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 30
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Gold,
                    RewardAmount = 17
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 34
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Spears,
                    RewardAmount = 11
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 38
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Gold,
                    RewardAmount = 12
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 42
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Spears,
                    RewardAmount = 23
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 46
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Gold,
                    RewardAmount = 13
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 50
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Spears,
                    RewardAmount = 15
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 55
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Gold,
                    RewardAmount = 30
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 66
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Spears,
                    RewardAmount = 18
                },
                new EnemyWaveModel
                {
                    Enemies =
                    [
                        new EnemyWaveUnitModel
                        {
                            EnemyType = UnitType.GiantRat,
                            Count = 72
                        }
                    ],
                    RewardType = EnemyWaveRewardType.Gold,
                    RewardAmount = 39
                }
            ]
        };
    }
}
