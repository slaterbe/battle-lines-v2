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
    public void ResolveBattleTick_UnlocksUpgrades_AfterFirstCombat()
    {
        var gameWorld = new GameWorldFactory().Create();

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        gameWorld.PlayerTotalAttack = gameWorld.CurrentWaveTotalHealth;

        new ResolveBattleTickCommand().Execute(gameWorld);

        Assert.True(gameWorld.AreUpgradesAvailable);
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
