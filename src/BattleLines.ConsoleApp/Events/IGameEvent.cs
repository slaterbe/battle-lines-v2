using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Events;

public interface IGameEvent
{
    bool IsEnabled(GameWorld world);

    void CheckEvent(GameWorld world);
}
