using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseArmySizeCommand : IGameCommand
{
    private const int GoldCost = 5;

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Boost Army Size";
    public string HelpText => $"Spend {GoldCost} gold to increase army size by 1.";

    public GameCommandCost GetCost() => new(Gold: GoldCost);

    public bool Execute(GameWorld gameWorld)
    {
        if (!gameWorld.IsUpgradesVisible || gameWorld.State != GameState.Village || gameWorld.Gold < GoldCost)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.MaxArmySize += 1;
        return false;
    }
}
