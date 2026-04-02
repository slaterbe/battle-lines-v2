using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class BattleService
{
    public void StartBattle(GameWorld gameWorld)
    {
        if (gameWorld.State == GameState.Battle || gameWorld.State == GameState.PostBattle || gameWorld.EnemyWaveList.Count == 0)
        {
            return;
        }

        gameWorld.State = GameState.PreBattle;
    }

    public void BeginBattle(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.PreBattle)
        {
            return;
        }

        gameWorld.PlayerHealthAtBattleStart = gameWorld.PlayerTotalHealth;
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
    }

    public void ResolveBattleTick(GameWorld gameWorld)
    {
        gameWorld.CurrentWaveTotalHealth = Math.Max(0, gameWorld.CurrentWaveTotalHealth - gameWorld.PlayerTotalAttack);
        gameWorld.PlayerTotalHealth = Math.Max(0, gameWorld.PlayerTotalHealth - gameWorld.CurrentWaveTotalAttack);

        if (gameWorld.CurrentWaveTotalHealth > 0 && gameWorld.PlayerTotalHealth > 0)
        {
            return;
        }

        gameWorld.LastBattleWon = gameWorld.CurrentWaveTotalHealth == 0;
        gameWorld.HasPendingPostBattleResolution = true;
        gameWorld.State = GameState.PostBattle;
    }
}
