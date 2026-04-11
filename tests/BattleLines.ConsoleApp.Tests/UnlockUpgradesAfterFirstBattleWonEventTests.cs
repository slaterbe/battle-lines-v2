using BattleLines.ConsoleApp.Events;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Tests;

public class UnlockUpgradesAfterFirstBattleWonEventTests
{
    [Fact]
    public void CheckEvent_UnlocksUpgradesAndUpdatesGoal_WhenFirstBattleHasBeenCleared()
    {
        var gameEvent = new UnlockUpgradesAfterFirstBattleWonEvent();
        var world = new GameWorld
        {
            BattlePosition = 1,
            TotalWaveCount = 5,
            IsUpgradesVisible = false,
            GoalMessage = "Goal: Defend the village!!!"
        };

        gameEvent.CheckEvent(world);

        Assert.True(world.IsUpgradesVisible);
        Assert.Equal("Goal: Defeat all 5 enemy waves.", world.GoalMessage);
    }

    [Fact]
    public void IsEnabled_ReturnsFalse_WhenUpgradesAreAlreadyUnlocked()
    {
        var gameEvent = new UnlockUpgradesAfterFirstBattleWonEvent();
        var world = new GameWorld
        {
            BattlePosition = 1,
            IsUpgradesVisible = true
        };

        Assert.False(gameEvent.IsEnabled(world));
    }

    [Fact]
    public void IsEnabled_ReturnsFalse_WhenPlayerHasReachedALaterBattle()
    {
        var gameEvent = new UnlockUpgradesAfterFirstBattleWonEvent();
        var world = new GameWorld
        {
            BattlePosition = 2,
            IsUpgradesVisible = false
        };

        Assert.False(gameEvent.IsEnabled(world));
    }
}
