using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class IncreaseSpearProductionCommand : IGameCommand
{
    private const int GoldCost = 10;
    private const int MaxSpearMakerCount = 8;

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => "Spear Maker";
    public string GetHelpText() => $"Spend {GoldCost} gold to build a Poleturner, increasing spear production by 1 and gaining 1 spear.";

    public GameCommandCost GetCost() => new(Gold: GoldCost);
    public GameCommandCost GetSupply() => new(Gold: -GoldCost, Spears: 1);
    public GameCommandCost GetIncome() => new(Spears: 1);

    public bool IsVisible(GameWorld gameWorld) =>
        gameWorld.IsSpearControlsVisible &&
        gameWorld.IsUpgradesVisible;
    public bool IsDisabled(GameWorld gameWorld) =>
        !gameWorld.IsSpearControlsVisible ||
        !gameWorld.IsUpgradesVisible ||
        gameWorld.State != GameState.Village ||
        gameWorld.Gold < GoldCost ||
        gameWorld.SpearProduction >= MaxSpearMakerCount;

    public bool Execute(GameWorld gameWorld)
    {
        if (!gameWorld.IsSpearControlsVisible
            || !gameWorld.IsUpgradesVisible
            || gameWorld.State != GameState.Village
            || gameWorld.Gold < GoldCost
            || gameWorld.SpearProduction >= MaxSpearMakerCount)
        {
            return false;
        }

        gameWorld.Gold -= GoldCost;
        gameWorld.SpearProduction += 1;
        gameWorld.Spears += 1;
        return false;
    }
}
