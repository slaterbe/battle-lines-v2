using BattleLines.ConsoleApp.Events;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Tests;

public class UnlockMilitiaYardAfterFourthBattleWonEventTests
{
    [Fact]
    public void CheckEvent_UnlocksMilitiaYard_WhenFourthBattleHasBeenCleared()
    {
        var gameEvent = new UnlockMilitiaYardAfterFourthBattleWonEvent();
        var world = new GameWorld
        {
            BattlePosition = 4,
            IsMilitiaYardVisible = false,
            TotalWaveCount = 3,
            GoalMessage = "Goal: Hold the line."
        };

        gameEvent.CheckEvent(world);

        Assert.True(world.IsMilitiaYardVisible);
        Assert.Equal("Goal: Defeat all 3 enemy waves.", world.GoalMessage);
    }

    [Fact]
    public void IsEnabled_ReturnsFalse_WhenMilitiaYardIsAlreadyUnlocked()
    {
        var gameEvent = new UnlockMilitiaYardAfterFourthBattleWonEvent();
        var world = new GameWorld
        {
            BattlePosition = 4,
            IsMilitiaYardVisible = true
        };

        Assert.False(gameEvent.IsEnabled(world));
    }

    [Fact]
    public void IsEnabled_ReturnsFalse_WhenFourthBattleHasNotBeenCleared()
    {
        var gameEvent = new UnlockMilitiaYardAfterFourthBattleWonEvent();
        var world = new GameWorld
        {
            BattlePosition = 3,
            IsMilitiaYardVisible = false
        };

        Assert.False(gameEvent.IsEnabled(world));
    }
}
