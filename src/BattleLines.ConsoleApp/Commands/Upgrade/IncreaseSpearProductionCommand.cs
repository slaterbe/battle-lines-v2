using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseSpearProductionCommand : IGameCommand
{
    private const int GoldCost = 10;

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Boost Spears";
    public string HelpText => $"Spend {GoldCost} gold to increase spear production by 1 and gain 1 spear.";

    public GameCommandCost GetCost() => new(Gold: GoldCost);
    public GameCommandCost GetSupply() => new(Gold: -GoldCost, Spears: 1);
    public GameCommandCost GetIncome() => new(Spears: 1);

    public bool IsVisible(GameWorld gameWorld) =>
        gameWorld.IsSpearControlsVisible &&
        gameWorld.IsUpgradesVisible;

    public bool Execute(GameWorld gameWorld)
    {
        if (!gameWorld.IsSpearControlsVisible
            || !gameWorld.IsUpgradesVisible
            || gameWorld.State != GameState.Village
            || gameWorld.Gold < GoldCost)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.SpearProduction += 1;
        gameWorld.Spears += 1;
        return false;
    }
}
