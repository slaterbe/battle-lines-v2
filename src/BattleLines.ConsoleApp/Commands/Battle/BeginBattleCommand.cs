using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class BeginBattleCommand : IGameCommand
{
    private readonly PlayerArmyBattleService playerArmyBattleService = new();

    public GameCommandCategory Category => GameCommandCategory.Battle;
    public string Label => "Fight Wave";
    public string HelpText => "Lock in your army and begin the current wave.";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.PreBattle)
        {
            return false;
        }

        playerArmyBattleService.CaptureBattleStart(gameWorld);
        gameWorld.PlayerUnitHistory.Clear();
        gameWorld.PlayerUnitHistory.Add(gameWorld.PlayerUnitsAtBattleStart.ToDictionary(entry => entry.Key, entry => entry.Value));
        gameWorld.PlayerHealthHistory.Clear();
        gameWorld.PlayerAttackHistory.Clear();
        gameWorld.PlayerMaxAttackHistory.Clear();
        gameWorld.EnemyHealthHistory.Clear();
        gameWorld.EnemyAttackHistory.Clear();
        gameWorld.EnemyMaxAttackHistory.Clear();
        gameWorld.LastBattleWon = false;
        gameWorld.HasPendingPostBattleResolution = false;
        gameWorld.State = GameState.Battle;
        return false;
    }
}
