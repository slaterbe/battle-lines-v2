using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public interface IGameCommand
{
    string Label { get; }

    bool Execute(GameWorld gameWorld);
}
