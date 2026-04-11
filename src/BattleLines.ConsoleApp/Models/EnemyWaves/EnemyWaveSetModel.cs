namespace BattleLines.ConsoleApp.Models;

public class EnemyWaveSetModel
{
    public List<EnemyWaveModel> Waves { get; set; } = [];

    public string FlashingVictoryMessage { get; set; } = string.Empty;

    public string FlavourVictoryMessage { get; set; } = string.Empty;

    public EnemyWaveRewardType FinalRewardType { get; set; }

    public int FinalRewardAmount { get; set; }
}
