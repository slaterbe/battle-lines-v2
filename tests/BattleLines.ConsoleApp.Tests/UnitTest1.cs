using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Tests;

public class GameServiceTests
{
    [Fact]
    public void CreateGameWorld_StartsPlayerWithOneSpearmenLvl1()
    {
        var gameService = new GameService();

        var gameWorld = gameService.CreateGameWorld();

        Assert.True(gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var count));
        Assert.Equal(1, count);
    }

    [Fact]
    public void Tick_IncrementsResourcesByProduction_WhenRunning()
    {
        var gameService = new GameService();
        var gameWorld = new GameWorld
        {
            Commoners = 10,
            Spears = 5,
            CommonerProduction = 2,
            SpearProduction = 1
        };

        gameService.TogglePause(gameWorld);
        gameService.Tick(gameWorld);

        Assert.Equal(12, gameWorld.Commoners);
        Assert.Equal(6, gameWorld.Spears);
    }

    [Fact]
    public void Tick_DoesNotIncrementResources_WhenPaused()
    {
        var gameService = new GameService();
        var gameWorld = new GameWorld
        {
            Commoners = 10,
            Spears = 5,
            CommonerProduction = 2,
            SpearProduction = 1
        };

        gameService.Tick(gameWorld);

        Assert.Equal(10, gameWorld.Commoners);
        Assert.Equal(5, gameWorld.Spears);
    }

    [Fact]
    public void TogglePause_ChangesPauseState()
    {
        var gameService = new GameService();
        var gameWorld = new GameWorld { IsPaused = true };

        gameService.TogglePause(gameWorld);

        Assert.False(gameWorld.IsPaused);
    }
}
