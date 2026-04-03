using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class ResetCurrentWaveCommand : IGameCommand
{
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public string Label => "Back to Village";

    public bool Execute(GameWorld gameWorld)
    {
        gameWorld.PlayerHealthAtBattleStart = 0;
        gameWorld.SpearmenCountAtBattleStart = 0;
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
        gameWorld.State = GameState.Village;
        gameWorldStatsService.Refresh(gameWorld);
        return false;
    }
}
