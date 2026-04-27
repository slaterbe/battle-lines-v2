namespace BattleLines.ConsoleApp.Models;

public class EnemyWaveModel
{
    public List<EnemyWaveUnitModel> Enemies { get; set; } = [];
    public List<EnemyWaveRewardModel> Rewards { get; set; } = [];
}
