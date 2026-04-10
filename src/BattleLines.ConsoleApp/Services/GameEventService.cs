using BattleLines.ConsoleApp.Events;
using BattleLines.ConsoleApp.Models;
using System.Reflection;

namespace BattleLines.ConsoleApp.Services;

public class GameEventService
{
    private readonly IReadOnlyList<IGameEvent> gameEvents;

    public GameEventService()
        : this(DiscoverEvents())
    {
    }

    public GameEventService(IReadOnlyList<IGameEvent> gameEvents)
    {
        this.gameEvents = gameEvents;
    }

    public static IReadOnlyList<IGameEvent> DiscoverEvents()
    {
        return typeof(IGameEvent).Assembly
            .GetTypes()
            .Where(type =>
                type is
                {
                    IsClass: true,
                    IsAbstract: false
                } &&
                typeof(IGameEvent).IsAssignableFrom(type) &&
                type.GetConstructor(Type.EmptyTypes) is not null)
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .Select(type => Activator.CreateInstance(type))
            .OfType<IGameEvent>()
            .ToArray();
    }

    public void CheckEvents(GameWorld world)
    {
        foreach (var gameEvent in gameEvents)
        {
            if (!gameEvent.IsEnabled(world))
            {
                continue;
            }

            gameEvent.CheckEvent(world);
        }
    }
}
