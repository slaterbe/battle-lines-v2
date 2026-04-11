using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Events;

public class UnlockSpearsAfterSecondBattleWonEvent : IGameEvent
{
    public bool IsEnabled(GameWorld world) => !world.IsSpearControlsVisible && world.BattlePosition == 2;

    public void CheckEvent(GameWorld world)
    {
        world.IsSpearControlsVisible = true;
        world.Spears += 1;
        world.SpearProduction += 1;
        world.GoalMessage = world.TotalWaveCount > 0
            ? $"Goal: Defeat all {world.TotalWaveCount} enemy waves."
            : "Goal: Defeat every enemy wave.";
    }
}
