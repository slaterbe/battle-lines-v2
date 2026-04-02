using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class EnemyWaveFactory
{
    public List<EnemyWaveModel> CreateGiantRatWaves(int waveCount, int startingCount = 3, int countIncreasePerWave = 2)
    {
        if (waveCount <= 0)
        {
            return [];
        }

        var waves = new List<EnemyWaveModel>(waveCount);
        var rewardTypes = Enum.GetValues<EnemyWaveRewardType>();

        for (var waveIndex = 0; waveIndex < waveCount; waveIndex++)
        {
            waves.Add(new EnemyWaveModel
            {
                Enemies =
                [
                    new EnemyWaveUnitModel
                    {
                        EnemyType = UnitType.GiantRat,
                        Count = startingCount + (waveIndex * countIncreasePerWave)
                    }
                ],
                RewardType = rewardTypes[waveIndex % rewardTypes.Length],
                RewardAmount = 5 + (waveIndex * 5)
            });
        }

        return waves;
    }
}
