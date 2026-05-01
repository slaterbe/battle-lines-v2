using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class UpgradeMilitiaYardCommand : IGameCommand
{
    private const int GoldCost = 20;
    private const int VillagerCost = 4;

    private readonly GameWorldStatsService gameWorldStatsService = new();

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Upgrade Militia Yard";
    public string GetHelpText() => $"Spend {GoldCost} gold and {VillagerCost} villagers to add {GameWorld.MilitiaYardHealthIncreasePerLevel} health to each unit.";

    public GameCommandCost GetCost() => new(Villagers: VillagerCost, Gold: GoldCost);
    public GameCommandCost GetSupply() => new(Villagers: -VillagerCost, Gold: -GoldCost);

    public bool IsVisible(GameWorld gameWorld) => gameWorld.IsMilitiaYardVisible;
    public bool IsDisabled(GameWorld gameWorld) =>
        !gameWorld.IsMilitiaYardVisible ||
        gameWorld.State != GameState.Village ||
        gameWorld.Gold < GoldCost ||
        gameWorld.Villagers < VillagerCost ||
        gameWorld.MilitiaYardLevel >= GameWorld.MaxMilitiaYardLevel;

    public bool Execute(GameWorld gameWorld)
    {
        if (!gameWorld.IsMilitiaYardVisible ||
            gameWorld.State != GameState.Village ||
            gameWorld.Gold < GoldCost ||
            gameWorld.Villagers < VillagerCost ||
            gameWorld.MilitiaYardLevel >= GameWorld.MaxMilitiaYardLevel)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.Villagers -= VillagerCost;
        gameWorld.MilitiaYardLevel += 1;
        gameWorldStatsService.Refresh(gameWorld);
        return false;
    }
}
