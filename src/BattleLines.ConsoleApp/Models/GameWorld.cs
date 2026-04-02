namespace BattleLines.ConsoleApp.Models;

public class GameWorld
{
    public int Commoners { get; set; }

    public int Spears { get; set; }

    public int Gold { get; set; }

    public int CommonerProduction { get; set; }

    public int SpearProduction { get; set; }

    public GameState State { get; set; } = GameState.Village;

    public Dictionary<UnitType, int> PlayerUnits { get; set; } = [];

    public List<EnemyWaveModel> EnemyWaveList { get; set; } = [];

    public int TotalWaveCount { get; set; }

    public int PlayerTotalHealth { get; set; }

    public int PlayerTotalAttack { get; set; }

    public int CurrentWaveTotalHealth { get; set; }

    public int CurrentWaveTotalAttack { get; set; }

    public bool LastBattleWon { get; set; }

    public bool HasPendingPostBattleResolution { get; set; }

    public int PlayerHealthAtBattleStart { get; set; }
}
