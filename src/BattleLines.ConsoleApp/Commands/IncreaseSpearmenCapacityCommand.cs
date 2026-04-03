using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseSpearmenCapacityCommand : IGameCommand
{
    private const int GoldCost = 5;

    public string Label => "Boost Capacity";
    public string HelpText => $"Spend {GoldCost} gold to increase spearmen capacity by 1.";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village || gameWorld.Gold < GoldCost)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.MaxSpearmenPositions += 1;
        return false;
    }
}
