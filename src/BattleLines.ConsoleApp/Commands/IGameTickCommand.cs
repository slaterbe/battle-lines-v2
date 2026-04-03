using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public interface IGameTickCommand
{
    void Execute(GameWorld gameWorld);
}
