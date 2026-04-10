using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Events;

public class DummyGameEvent : IGameEvent
{
    public bool IsEnabled(GameWorld world) => true;

    public void CheckEvent(GameWorld world)
    {
    }
}
