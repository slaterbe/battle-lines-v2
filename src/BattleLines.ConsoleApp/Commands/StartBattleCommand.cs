using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class StartBattleCommand : IGameCommand
{
    public string Label => "Start Battle";
    public string HelpText => "Move to battle prep for the next enemy wave.";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State == GameState.Battle ||
            gameWorld.State == GameState.PostWave ||
            gameWorld.State == GameState.PostBattle ||
            gameWorld.EnemyWaveList.Count == 0)
        {
            return false;
        }

        gameWorld.State = GameState.PreBattle;
        return false;
    }
}
