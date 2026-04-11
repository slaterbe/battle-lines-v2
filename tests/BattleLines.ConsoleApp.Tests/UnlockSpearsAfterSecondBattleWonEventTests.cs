using BattleLines.ConsoleApp.Events;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Tests;

public class UnlockSpearsAfterSecondBattleWonEventTests
{
    [Fact]
    public void CheckEvent_UnlocksSpearsAndUpdatesGoal_WhenSecondBattleHasBeenCleared()
    {
        var gameEvent = new UnlockSpearsAfterSecondBattleWonEvent();
        var world = new GameWorld
        {
            BattlePosition = 2,
            TotalWaveCount = 8,
            IsSpearControlsVisible = false,
            Spears = 0,
            SpearProduction = 0,
            GoalMessage = "Goal: Defeat all 5 enemy waves."
        };

        gameEvent.CheckEvent(world);

        Assert.True(world.IsSpearControlsVisible);
        Assert.Equal(1, world.Spears);
        Assert.Equal(1, world.SpearProduction);
        Assert.Equal("Goal: Defeat all 8 enemy waves.", world.GoalMessage);
    }

    [Fact]
    public void IsEnabled_ReturnsFalse_WhenSpearsAreAlreadyUnlocked()
    {
        var gameEvent = new UnlockSpearsAfterSecondBattleWonEvent();
        var world = new GameWorld
        {
            BattlePosition = 2,
            IsSpearControlsVisible = true
        };

        Assert.False(gameEvent.IsEnabled(world));
    }
}
