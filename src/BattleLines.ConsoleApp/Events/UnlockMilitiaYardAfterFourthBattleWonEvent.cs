using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Events;

public class UnlockMilitiaYardAfterFourthBattleWonEvent : IGameEvent
{
    public bool IsEnabled(GameWorld world) => !world.IsMilitiaYardVisible && world.BattlePosition == 4;

    public void CheckEvent(GameWorld world)
    {
        world.IsMilitiaYardVisible = true;
        world.GoalMessage = world.TotalWaveCount > 0
            ? $"Goal: Defeat all {world.TotalWaveCount} enemy waves."
            : "Goal: Defeat every enemy wave.";
    }
}
