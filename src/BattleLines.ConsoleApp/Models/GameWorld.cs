namespace BattleLines.ConsoleApp.Models;

public class GameWorld
{
    // Progression and feature flags
    public bool IsSkipIntroduction { get; set; }
    public bool IsSpearControlsVisible { get; set; }
    public bool IsUpgradesVisible { get; set; }
    public int FightersCreated { get; set; }

    // Resources and production
    public int Villagers { get; set; }
    public int Spears { get; set; }
    public int Gold { get; set; }
    public int VillagerProduction { get; set; }
    public int SpearProduction { get; set; }

    // World and encounter state
    public GameState State { get; set; } = GameState.Village;
    public EnemyWaveSetModel EnemyWaves { get; set; } = new();
    public int TotalWaveCount { get; set; }
    public int WavePosition { get; set; }
    public int BattlePosition { get; set; }
    public bool LastBattleWon { get; set; }
    public bool HasPendingPostBattleResolution { get; set; }

    // Army state
    public Dictionary<UnitType, int> PlayerUnits { get; set; } = [];
    public int MaxArmySize { get; set; }
    public int PlayerTotalHealth { get; set; }
    public int PlayerTotalAttack { get; set; }
    public int CurrentWaveTotalHealth { get; set; }
    public int CurrentWaveTotalAttack { get; set; }

    // Battle snapshots and history
    public int PlayerHealthAtBattleStart { get; set; }
    public Dictionary<UnitType, int> PlayerUnitsAtBattleStart { get; set; } = [];
    public List<int> PlayerHealthHistory { get; set; } = [];
    public List<int> PlayerAttackHistory { get; set; } = [];
    public List<int> EnemyHealthHistory { get; set; } = [];
    public List<int> EnemyAttackHistory { get; set; } = [];
}
