using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public interface IGameCommand
{
    GameCommandCategory Category { get; }

    string Label { get; }

    string GetHelpText();

    GameCommandCost? GetCost()
    {
        return null;
    }

    GameCommandCost? GetSupply()
    {
        return null;
    }

    GameCommandCost? GetIncome()
    {
        return null;
    }

    bool IsVisible(GameWorld gameWorld)
    {
        return true;
    }

    bool IsDisabled(GameWorld gameWorld)
    {
        return false;
    }

    bool Execute(GameWorld gameWorld);
}
