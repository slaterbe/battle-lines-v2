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
        Assert.DoesNotContain("Expand Battle Line", commandLabels);
        Assert.Contains("Expand Farm", commandLabels);
    }

    [Fact]
    public void VillageController_ShowsStartBattleCommand_ByDefault()
    {
        var gameWorld = new GameWorldFactory().Create();
        var controller = new VillageController();

        var commandLabels = controller.GetCommandOptions(gameWorld).Select(option => option.Label).ToArray();

        Assert.Contains("Go to the Gates", commandLabels);
    }

    [Fact]
    public void VillageController_HidesArmyCommands()
    {
        var gameWorld = new GameWorldFactory().Create();
        var controller = new VillageController();

        var commandLabels = controller.GetCommandOptions(gameWorld).Select(option => option.Label).ToArray();

        Assert.DoesNotContain("Recruit Fighter", commandLabels);
        Assert.DoesNotContain("Recruit Spearmen", commandLabels);
    }

    [Fact]
    public void FirstBattleWin_UnlocksUpgrades_AfterReturningToVillage()
    {
        var gameWorld = new GameWorldFactory().Create();
        var eventService = new GameEventService();
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
    public void IncreaseFoodProduction_Works_BeforeUpgradesAreUnlocked()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.Food = 20;
        gameWorld.Gold = 4;
        gameWorld.State = BattleLines.ConsoleApp.Models.GameState.Village;

        new IncreaseFoodProductionCommand().Execute(gameWorld);

        Assert.Equal(10, gameWorld.Food);
        Assert.Equal(2, gameWorld.Gold);
        Assert.Equal(2, gameWorld.FoodProduction);
    }

    [Fact]
    public void VillageController_ShowsBuyVillageCommand()
    {
        var gameWorld = new GameWorldFactory().Create();
        var controller = new VillageController();

        var commandLabels = controller.GetCommandOptions(gameWorld).Select(option => option.Label).ToArray();

        Assert.Contains("Buy Villager", commandLabels);
    }

    [Fact]
    public void PreWaveController_ShowsArmyCommands()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.State = BattleLines.ConsoleApp.Models.GameState.PreBattle;
        var controller = new PreWaveController();

        var commandLabels = controller.GetCommandOptions(gameWorld).Select(option => option.Label).ToArray();

        Assert.Contains("Recruit Fighter", commandLabels);
        Assert.Contains("Fight Wave", commandLabels);
    }
}
