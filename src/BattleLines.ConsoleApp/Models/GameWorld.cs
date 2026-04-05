namespace BattleLines.ConsoleApp.Models;

public class GameWorld
{
    public int Villagers { get; set; }

    public int Spears { get; set; }

    public int Gold { get; set; }

    public int VillagerProduction { get; set; }

    public int SpearProduction { get; set; }

    public GameState State { get; set; } = GameState.Village;

    public Dictionary<UnitType, int> PlayerUnits { get; set; } = [];

    public EnemyWaveSetModel EnemyWaves { get; set; } = new();

    public int TotalWaveCount { get; set; }

    public int WavePosition { get; set; }

    public int BattlePosition { get; set; }

    public int MaxArmySize { get; set; }

    public int PlayerTotalHealth { get; set; }

    public int PlayerTotalAttack { get; set; }

    public int CurrentWaveTotalHealth { get; set; }

    public int CurrentWaveTotalAttack { get; set; }

    public bool LastBattleWon { get; set; }

    public bool HasPendingPostBattleResolution { get; set; }

    public int PlayerHealthAtBattleStart { get; set; }

    public Dictionary<UnitType, int> PlayerUnitsAtBattleStart { get; set; } = [];

    public List<int> PlayerHealthHistory { get; set; } = [];

    public List<int> PlayerAttackHistory { get; set; } = [];

    public List<int> EnemyHealthHistory { get; set; } = [];

    public List<int> EnemyAttackHistory { get; set; } = [];
}
