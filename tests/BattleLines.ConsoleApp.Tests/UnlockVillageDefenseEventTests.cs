using BattleLines.ConsoleApp.Events;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Tests;

public class UnlockVillageDefenseEventTests
{
    [Fact]
    public void CheckEvent_UnlocksVillageDefense_WhenFiveFightersHaveBeenCreated()
    {
        var gameEvent = new UnlockVillageDefenseEvent();
        var world = new GameWorld
        {
            FightersCreated = 5,
            IsFiveFightersCreated = false,
            GoalMessage = "Goal: Recruit 5 fighters."
        };

        gameEvent.CheckEvent(world);

        Assert.True(world.IsFiveFightersCreated);
        Assert.Equal("Goal: Defend the village!!!", world.GoalMessage);
    }

    [Fact]
    public void IsEnabled_ReturnsFalse_WhenVillageDefenseIsAlreadyUnlocked()
    {
        var gameEvent = new UnlockVillageDefenseEvent();
        var world = new GameWorld
        {
            IsFiveFightersCreated = true
        };

        Assert.False(gameEvent.IsEnabled(world));
    }
}
