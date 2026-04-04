using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseSpearProductionCommand : IGameCommand
{
    private const int GoldCost = 5;

    public string Label => "Boost Spears";
    public string HelpText => $"Spend {GoldCost} gold to increase spear production by 1.";

    public GameCommandCost GetCost() => new(Gold: GoldCost);

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village || gameWorld.Gold < GoldCost)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.SpearProduction += 1;
        return false;
    }
}
