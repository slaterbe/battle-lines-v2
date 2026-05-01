using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class EnemyWaveFactory
{
    public EnemyWaveSetModel CreateBattle(int battlePosition)
    {
        return battlePosition switch
        {
            0 => CreateFirstBattleWaveSet(),
            1 => CreateSecondBattleWaveSet(),
            2 => CreateThirdBattleWaveSet(),
            3 => CreateFourthBattleWaveSet(),
            4 => CreateFifthBattleWaveSet(),
            _ => new EnemyWaveSetModel()
        };
    }

    public bool HasBattle(int battlePosition)
    {
        return battlePosition is 0 or 1 or 2 or 3 or 4;
    }

    private static EnemyWaveSetModel CreateFirstBattleWaveSet()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "The first swarm breaks beneath your defense, and the village dares to hope.",
            FlashingVictoryMessage = "Unlocked Village upgrades",
            DetailedVictoryMessage = "Invest in stronger village production before the next assault arrives.",
            Waves =
            [
                CreateWave(UnitType.GiantRat, 2, 10, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 5))
            ]
        };
    }

    private static EnemyWaveSetModel CreateSecondBattleWaveSet()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "Your line holds through the assault, and the village stands unbroken.",
            FlashingVictoryMessage = "Unlocked spears",
            DetailedVictoryMessage = "Train spearmen to add a harder-hitting unit to your battle line.",
            Waves =
            [
                CreateWave(UnitType.GiantRat, 3, 10, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 5)),
                CreateWave(UnitType.GiantRat, 5, 15, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 10)),
                CreateWave(UnitType.GiantRat, 7, 20, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 15)),
                CreateWave(UnitType.GiantRat, 9, 25, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 20)),
                CreateWave(UnitType.GiantRat, 11, 30, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 25))
            ]
        };
    }

    private static EnemyWaveSetModel CreateThirdBattleWaveSet()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "The warband shatters against your defense. For now, the frontier is safe.",
            FlashingVictoryMessage = "More to unlock",
            DetailedVictoryMessage = "This victory marks the current frontier. More upgrades and enemies can be added next.",
            Waves =
            [
                CreateWave(UnitType.GiantRat, 6, 10, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 10)),
                CreateWave(UnitType.GiantRat, 8, 15, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 2)),
                CreateWave(UnitType.GiantRat, 10, 20, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 15)),
                CreateWave(UnitType.GiantRat, 12, 25, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 3)),
                CreateWave(UnitType.GiantRat, 14, 30, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 20)),
                CreateWave(UnitType.GiantRat, 16, 35, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 4)),
                CreateWave(UnitType.GiantRat, 18, 40, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 25)),
                CreateWave(UnitType.GiantRat, 20, 45, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 5))
            ]
        };
    }

    private static EnemyWaveSetModel CreateFourthBattleWaveSet()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "The raiders break against your walls, and the village answers with hardened resolve.",
            FlashingVictoryMessage = "Unlocked Militia Yard",
            DetailedVictoryMessage = "The village can now raise a militia yard to harden every soldier for the next frontier.",
            Waves =
            [
                CreateWave(UnitType.Raider, 4, 15, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 10)),
                CreateWave(UnitType.Raider, 5, 20, new EnemyWaveRewardModel(EnemyWaveRewardType.Villagers, 1)),
                CreateWave(UnitType.Raider, 6, 25, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 15)),
                CreateWave(UnitType.Raider, 7, 30, new EnemyWaveRewardModel(EnemyWaveRewardType.Villagers, 2)),
                CreateWave(UnitType.Raider, 8, 35, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 20)),
                CreateWave(UnitType.Raider, 9, 40, new EnemyWaveRewardModel(EnemyWaveRewardType.Villagers, 3))
            ]
        };
    }

    private static EnemyWaveSetModel CreateFifthBattleWaveSet()
    {
        return new EnemyWaveSetModel
        {
            FlavourVictoryMessage = "The final assault buckles under a veteran host. The frontier holds because your village learned to endure.",
            FlashingVictoryMessage = "Frontier Secured",
            DetailedVictoryMessage = "The militia yard has tempered your defenders. This is the current end of the campaign.",
            Waves =
            [
                CreateWave(UnitType.Raider, 6, 20, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 15)),
                CreateWave(UnitType.GiantRat, 18, 25, new EnemyWaveRewardModel(EnemyWaveRewardType.Villagers, 2)),
                CreateWave(UnitType.Raider, 8, 30, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 20)),
                CreateWave(UnitType.GiantRat, 22, 35, new EnemyWaveRewardModel(EnemyWaveRewardType.Spears, 4)),
                CreateWave(UnitType.Raider, 10, 40, new EnemyWaveRewardModel(EnemyWaveRewardType.Gold, 25)),
                CreateWave(UnitType.Raider, 12, 45, new EnemyWaveRewardModel(EnemyWaveRewardType.Villagers, 3))
            ]
        };
    }

    private static EnemyWaveModel CreateWave(UnitType enemyType, int enemyCount, int foodRewardAmount, params EnemyWaveRewardModel[] additionalRewards)
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
                    EnemyType = enemyType,
                    Count = enemyCount
                }
            ],
            Rewards = rewards
        };
    }
}
