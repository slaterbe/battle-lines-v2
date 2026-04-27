using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseArmySizeCommand : IGameCommand
{
    private const int GoldCost = 5;

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Expand Battle Line";
    public string GetHelpText() => $"Spend {GoldCost} gold to increase battle line by 1.";

    public GameCommandCost GetCost() => new(Gold: GoldCost);
    public GameCommandCost GetSupply() => new(Gold: -GoldCost);

    public bool IsVisible(GameWorld gameWorld) => gameWorld.IsUpgradesVisible;
    public bool IsDisabled(GameWorld gameWorld) =>
        !gameWorld.IsUpgradesVisible ||
        gameWorld.State != GameState.Village ||
        gameWorld.Gold < GoldCost;

    public bool Execute(GameWorld gameWorld)
    {
        if (!gameWorld.IsUpgradesVisible || gameWorld.State != GameState.Village || gameWorld.Gold < GoldCost)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.FrontLineCapacity += 1;
        return false;
    }
}
