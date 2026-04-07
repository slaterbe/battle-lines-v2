using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class EnterVillageCommand : IGameCommand
{
    public GameCommandCategory Category => GameCommandCategory.Battle;
    public string Label => "Enter the village";
    public string HelpText => "Continue into the village and prepare your defenses.";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Introduction)
        {
            return false;
        }

        gameWorld.State = GameState.Village;
        return false;
    }
}
