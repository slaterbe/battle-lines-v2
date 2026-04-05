namespace BattleLines.ConsoleApp.Models;

public class EnemyWaveSetModel
{
    public List<EnemyWaveModel> Waves { get; set; } = [];

    public EnemyWaveRewardType FinalRewardType { get; set; }

    public int FinalRewardAmount { get; set; }
}
