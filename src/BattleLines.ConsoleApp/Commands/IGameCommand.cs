using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public interface IGameCommand
{
    string Label { get; }

    string HelpText { get; }

    bool Execute(GameWorld gameWorld);
}
