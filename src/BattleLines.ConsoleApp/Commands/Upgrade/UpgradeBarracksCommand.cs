using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class UpgradeBarracksCommand : IGameCommand
{
    private const int GoldCost = 5;
    private const int FrontLineCapacityIncrease = 1;
    private const int MaxBarracksLevel = 8;

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Upgrade Barracks";
    public string GetHelpText() => $"Spend {GoldCost} gold to upgrade the barracks and add {FrontLineCapacityIncrease} battle position.";

    public GameCommandCost GetCost() => new(Gold: GoldCost);
    public GameCommandCost GetSupply() => new(Gold: -GoldCost);

    public bool IsVisible(GameWorld gameWorld) => gameWorld.IsUpgradesVisible;
    public bool IsDisabled(GameWorld gameWorld) =>
        !gameWorld.IsUpgradesVisible ||
        gameWorld.State != GameState.Village ||
        gameWorld.Gold < GoldCost ||
        gameWorld.BarracksLevel >= MaxBarracksLevel;

    public bool Execute(GameWorld gameWorld)
    {
        if (!gameWorld.IsUpgradesVisible ||
            gameWorld.State != GameState.Village ||
            gameWorld.Gold < GoldCost ||
            gameWorld.BarracksLevel >= MaxBarracksLevel)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.FrontLineCapacity += FrontLineCapacityIncrease;
        return false;
    }
}
