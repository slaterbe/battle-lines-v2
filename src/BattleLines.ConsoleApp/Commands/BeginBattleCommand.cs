using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class BeginBattleCommand : IGameCommand
{
    public string Label => "Fight Wave";
    public string HelpText => "Lock in your army and begin the current wave.";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.PreBattle)
        {
            return false;
        }

        gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var spearmenCount);
        gameWorld.PlayerHealthAtBattleStart = gameWorld.PlayerTotalHealth;
        gameWorld.SpearmenCountAtBattleStart = spearmenCount;
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
        gameWorld.State = GameState.Battle;
        return false;
    }
}
