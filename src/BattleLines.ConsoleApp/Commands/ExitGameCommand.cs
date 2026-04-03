using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class ExitGameCommand : IGameCommand
{
    public string Label => "Exit";
    public string HelpText => "Close the game.";

    public bool Execute(GameWorld gameWorld)
    {
        return true;
    }
}
