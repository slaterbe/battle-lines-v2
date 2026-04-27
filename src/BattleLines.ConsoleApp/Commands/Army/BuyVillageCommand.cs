using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class BuyVillageCommand : IGameCommand
{
    private const int FoodCost = 5;

    public GameCommandCategory Category => GameCommandCategory.Buy;
    public string Label => "Buy Villager";
    public string HelpText => $"Spend {FoodCost} food to gain 1 villager.";

    public GameCommandCost GetCost() => new(Food: FoodCost);

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village || gameWorld.Food < FoodCost)
        {
            return false;
        }

        gameWorld.Food -= FoodCost;
        gameWorld.Villagers += 1;
        return false;
    }
}
