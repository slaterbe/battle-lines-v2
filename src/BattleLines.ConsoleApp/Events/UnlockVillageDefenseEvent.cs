using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Events;

public class UnlockVillageDefenseEvent : IGameEvent
{
    private const int RequiredFighterCount = 5;

    public bool IsEnabled(GameWorld world) => !world.IsFiveFightersCreated;

    public void CheckEvent(GameWorld world)
    {
        if (world.FightersCreated >= RequiredFighterCount)
        {
            world.IsFiveFightersCreated = true;
            world.GoalMessage = "Goal: Defend the village!!!";
        }
    }
}
