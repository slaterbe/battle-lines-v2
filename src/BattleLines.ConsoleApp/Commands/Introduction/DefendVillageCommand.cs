using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class DefendVillageCommand : IGameCommand
{
    public GameCommandCategory Category => GameCommandCategory.Battle;
    public string Label => "Defend the village!!!";
    public string HelpText => "Move straight into the opening defense.";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Introduction)
        {
            return false;
        }

        gameWorld.WavePosition = gameWorld.EnemyWaves.Waves.Count > 0 ? 1 : 0;
        gameWorld.State = GameState.Village;
        return false;
    }
}
