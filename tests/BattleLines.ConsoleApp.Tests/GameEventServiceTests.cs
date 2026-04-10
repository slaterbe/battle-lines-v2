using BattleLines.ConsoleApp.Events;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Tests;

public class GameEventServiceTests
{
    [Fact]
    public void CheckEvents_InvokesEveryRegisteredEvent()
    {
        var firstEvent = new TestGameEvent();
        var secondEvent = new TestGameEvent();
        var eventService = new GameEventService([firstEvent, secondEvent]);
        var world = new GameWorld();

        eventService.CheckEvents(world);

        Assert.Equal(1, firstEvent.CheckCount);
        Assert.Equal(1, secondEvent.CheckCount);
        Assert.Same(world, firstEvent.LastWorld);
        Assert.Same(world, secondEvent.LastWorld);
    }

    [Fact]
    public void CheckEvents_SkipsDisabledEvents()
    {
        var enabledEvent = new TestGameEvent(isEnabled: true);
        var disabledEvent = new TestGameEvent(isEnabled: false);
        var eventService = new GameEventService([enabledEvent, disabledEvent]);
        var world = new GameWorld();

        eventService.CheckEvents(world);

        Assert.Equal(1, enabledEvent.CheckCount);
        Assert.Equal(0, disabledEvent.CheckCount);
    }

    [Fact]
    public void DiscoverEvents_FindsDummyGameEvent()
    {
        var discoveredEvents = GameEventService.DiscoverEvents();

        Assert.Contains(discoveredEvents, gameEvent => gameEvent is DummyGameEvent);
    }

    [Fact]
    public void DummyGameEvent_CheckEvent_DoesNotThrow()
    {
        var gameEvent = new DummyGameEvent();
        var world = new GameWorld();

        var exception = Record.Exception(() => gameEvent.CheckEvent(world));

        Assert.Null(exception);
    }

    private sealed class TestGameEvent : IGameEvent
    {
        private readonly bool isEnabled;

        public TestGameEvent(bool isEnabled = true)
        {
            this.isEnabled = isEnabled;
        }

        public int CheckCount { get; private set; }

        public GameWorld? LastWorld { get; private set; }

        public bool IsEnabled(GameWorld world) => isEnabled;

        public void CheckEvent(GameWorld world)
        {
            CheckCount++;
            LastWorld = world;
        }
    }
}
