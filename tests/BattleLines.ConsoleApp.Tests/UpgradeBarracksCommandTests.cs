using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Tests;

public class UpgradeBarracksCommandTests
{
    [Fact]
    public void UpgradeBarracks_IsHidden_UntilUpgradesAreUnlocked()
    {
        var command = new UpgradeBarracksCommand();
        var gameWorld = new GameWorld
        {
            IsUpgradesVisible = false,
            State = GameState.Village
        };

        Assert.False(command.IsVisible(gameWorld));
    }
}
