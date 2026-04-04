using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public interface IGameCommand
{
    GameCommandCategory Category { get; }

    string Label { get; }

    string HelpText { get; }

    GameCommandCost? GetCost()
    {
        return null;
    }

    bool Execute(GameWorld gameWorld);
}
