using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseFoodProductionCommand : IGameCommand
{
    private const int FoodCost = 10;
    private const int GoldCost = 2;
    private const int FoodProductionIncrease = 2;

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Expand Farm";
    public string HelpText => $"Spend {GoldCost} gold and {FoodCost} food to increase food income by {FoodProductionIncrease}.";

    public GameCommandCost GetCost() => new(Food: FoodCost, Gold: GoldCost);

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village || gameWorld.Food < FoodCost || gameWorld.Gold < GoldCost)
        {
            return false;
        }

        gameWorld.Food -= FoodCost;
        gameWorld.Gold -= GoldCost;
        gameWorld.FoodProduction += FoodProductionIncrease;
        return false;
    }
}
