using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class UpgradeGateHouseCommand : IGameCommand
{
    private const int GoldCost = 5;
    private const int FrontLineCapacityIncrease = 1;

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Upgrade GateHouse";
    public string GetHelpText() => $"Spend {GoldCost} gold to upgrade the gatehouse and add {FrontLineCapacityIncrease} battle position.";

    public GameCommandCost GetCost() => new(Gold: GoldCost);
    public GameCommandCost GetSupply() => new(Gold: -GoldCost);

    public bool IsVisible(GameWorld gameWorld) => gameWorld.IsUpgradesVisible;
    public bool IsDisabled(GameWorld gameWorld) =>
        !gameWorld.IsUpgradesVisible ||
        gameWorld.State != GameState.Village ||
        gameWorld.Gold < GoldCost ||
        gameWorld.GateHouseLevel >= GameWorld.MaxGateHouseLevel;

    public bool Execute(GameWorld gameWorld)
    {
        if (!gameWorld.IsUpgradesVisible ||
            gameWorld.State != GameState.Village ||
            gameWorld.Gold < GoldCost ||
            gameWorld.GateHouseLevel >= GameWorld.MaxGateHouseLevel)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.GateHouseLevel += 1;
        gameWorld.FrontLineCapacity += FrontLineCapacityIncrease;
        return false;
    }
}
