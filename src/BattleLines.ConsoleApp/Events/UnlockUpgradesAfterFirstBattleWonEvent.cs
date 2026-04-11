using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Events;

public class UnlockUpgradesAfterFirstBattleWonEvent : IGameEvent
{
    public bool IsEnabled(GameWorld world) => !world.IsUpgradesVisible && world.BattlePosition == 1;

    public void CheckEvent(GameWorld world)
    {
        world.IsUpgradesVisible = true;
        world.GoalMessage = world.TotalWaveCount > 0
            ? $"Goal: Defeat all {world.TotalWaveCount} enemy waves."
            : "Goal: Defeat every enemy wave.";
    }
}
