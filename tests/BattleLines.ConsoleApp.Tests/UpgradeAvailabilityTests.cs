using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Controllers;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Tests;

public class UpgradeAvailabilityTests
{
    [Fact]
    public void VillageController_HidesUpgradeCommands_UntilUnlocked()
    {
        var gameWorld = new GameWorldFactory().Create();
        var controller = new VillageController();

        var commandLabels = controller.GetCommandOptions(gameWorld).Select(option => option.Label).ToArray();

        Assert.DoesNotContain("Boost Villagers", commandLabels);
        Assert.DoesNotContain("Boost Spears", commandLabels);
        Assert.DoesNotContain("Boost Army Size", commandLabels);
    }

    [Fact]
    public void VillageController_HidesStartBattleCommand_UntilFiveFightersCreated()
    {
        var gameWorld = new GameWorldFactory().Create();
        var controller = new VillageController();

        var commandLabels = controller.GetCommandOptions(gameWorld).Select(option => option.Label).ToArray();

        Assert.DoesNotContain("Defend the village", commandLabels);
    }

    [Fact]
    public void VillageController_ShowsStartBattleCommand_AfterFiveFightersCreated()
    {
        var gameWorld = new GameWorldFactory().Create();
        var controller = new VillageController();
        gameWorld.IsFiveFightersCreated = true;

        var commandLabels = controller.GetCommandOptions(gameWorld).Select(option => option.Label).ToArray();

        Assert.Contains("Defend the village", commandLabels);
    }

    [Fact]
    public void FirstBattleWin_UnlocksUpgrades_AfterReturningToVillage()
    {
        var gameWorld = new GameWorldFactory().Create();
        var eventService = new GameEventService();
        gameWorld.IsFiveFightersCreated = true;
        gameWorld.GoalMessage = "Goal: Defend the village!!!";
        gameWorld.State = BattleLines.ConsoleApp.Models.GameState.Village;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        gameWorld.PlayerTotalAttack = gameWorld.CurrentWaveTotalHealth;
        gameWorld.PlayerTotalMaxAttack = gameWorld.CurrentWaveTotalHealth;

        new ResolveBattleTickCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);
        eventService.CheckEvents(gameWorld);

        Assert.True(gameWorld.IsUpgradesVisible);
        Assert.Equal("Goal: Defeat all 5 enemy waves.", gameWorld.GoalMessage);
    }

    [Fact]
    public void IncreaseVillagerProduction_DoesNothing_WhenUpgradesAreLocked()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.Gold = 10;

        new IncreaseVillagerProductionCommand().Execute(gameWorld);

        Assert.Equal(10, gameWorld.Gold);
        Assert.Equal(2, gameWorld.VillagerProduction);
        Assert.Equal(5, gameWorld.Villagers);
    }
}
