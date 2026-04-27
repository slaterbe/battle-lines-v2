using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class ExitGameCommand : IGameCommand
{
    public GameCommandCategory Category => GameCommandCategory.Battle;
    public string Label => "Exit";
    public string GetHelpText() => "Close the game.";

    public bool Execute(GameWorld gameWorld)
    {
        return true;
    }
}
