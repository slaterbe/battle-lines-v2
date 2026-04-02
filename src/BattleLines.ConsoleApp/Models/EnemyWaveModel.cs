namespace BattleLines.ConsoleApp.Models;

public class EnemyWaveModel
{
    public List<EnemyWaveUnitModel> Enemies { get; set; } = [];

    public EnemyWaveRewardType RewardType { get; set; }

    public int RewardAmount { get; set; }
}
