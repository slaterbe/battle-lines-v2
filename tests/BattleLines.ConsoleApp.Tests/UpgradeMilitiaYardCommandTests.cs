using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Tests;

public class UpgradeMilitiaYardCommandTests
{
    [Fact]
    public void UpgradeMilitiaYard_IsHidden_UntilUnlocked()
    {
        var command = new UpgradeMilitiaYardCommand();
        var gameWorld = new GameWorld
        {
            IsMilitiaYardVisible = false,
            State = GameState.Village
        };

        Assert.False(command.IsVisible(gameWorld));
    }
}
