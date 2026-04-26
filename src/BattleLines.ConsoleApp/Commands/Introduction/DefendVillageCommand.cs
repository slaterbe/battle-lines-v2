using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class DefendVillageCommand : IGameCommand
{
    public GameCommandCategory Category => GameCommandCategory.Travel;
    public string Label => "Go To the Village";
    public string HelpText => "Leave the introduction and step into the village.";

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
