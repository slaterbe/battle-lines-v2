using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Tests;

public class UpgradeGateHouseCommandTests
{
    [Fact]
    public void UpgradeGateHouse_IsHidden_UntilUpgradesAreUnlocked()
    {
        var command = new UpgradeGateHouseCommand();
        var gameWorld = new GameWorld
        {
            IsUpgradesVisible = false,
            State = GameState.Village
        };

        Assert.False(command.IsVisible(gameWorld));
    }
}
