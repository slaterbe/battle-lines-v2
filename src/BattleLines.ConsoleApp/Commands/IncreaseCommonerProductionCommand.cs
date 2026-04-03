using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseCommonerProductionCommand : IGameCommand
{
    private const int GoldCost = 5;

    public string Label => "Boost Commoners";
    public string HelpText => $"Spend {GoldCost} gold to increase commoner production by 1.";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village || gameWorld.Gold < GoldCost)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.CommonerProduction += 1;
        return false;
    }
}
