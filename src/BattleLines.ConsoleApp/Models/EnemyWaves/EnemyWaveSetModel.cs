namespace BattleLines.ConsoleApp.Models;

public class EnemyWaveSetModel
{
    public List<EnemyWaveModel> Waves { get; set; } = [];

    public string FlashingVictoryMessage { get; set; } = string.Empty;

    public string DetailedVictoryMessage { get; set; } = string.Empty;

    public string FlavourVictoryMessage { get; set; } = string.Empty;
    public List<EnemyWaveRewardModel> FinalRewards { get; set; } = [];
}
