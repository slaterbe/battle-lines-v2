using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseVillagerProductionCommand : IGameCommand
{
    private const int GoldCost = 5;

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Boost Villagers";
    public string HelpText => $"Spend {GoldCost} gold to increase villager production by 1 and gain 1 villager.";

    public GameCommandCost GetCost() => new(Gold: GoldCost);

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village || gameWorld.Gold < GoldCost)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.VillagerProduction += 1;
        gameWorld.Villagers += 1;
        return false;
    }
}
