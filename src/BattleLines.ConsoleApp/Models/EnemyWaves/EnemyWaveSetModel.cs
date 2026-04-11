namespace BattleLines.ConsoleApp.Models;

public class EnemyWaveSetModel
{
    public List<EnemyWaveModel> Waves { get; set; } = [];

    public string VictoryMessage { get; set; } = string.Empty;

    public EnemyWaveRewardType FinalRewardType { get; set; }

    public int FinalRewardAmount { get; set; }
}
