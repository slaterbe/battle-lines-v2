using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class AddFighterCommand : IGameCommand
{
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public GameCommandCategory Category => GameCommandCategory.Army;
    public string Label => "Recruit Fighter";
    public string GetHelpText() => "Spend 1 villager to recruit a fighter.";

    public GameCommandCost GetCost() => new(Villagers: 1);
    public GameCommandCost GetSupply() => new(Villagers: -1);
    public bool IsDisabled(GameWorld gameWorld) =>
        gameWorld.State != GameState.PreBattle ||
        gameWorld.Villagers < 1 ||
        gameWorld.PlayerUnits.Values.Sum() >= gameWorld.FrontLineCapacity;

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.PreBattle)
        {
            return false;
        }

        if (gameWorld.Villagers < 1)
        {
            return false;
        }

        var totalArmySize = gameWorld.PlayerUnits.Values.Sum();
        if (totalArmySize >= gameWorld.FrontLineCapacity)
        {
            return false;
        }

        gameWorld.PlayerUnits.TryGetValue(UnitType.Fighter, out var currentCount);

        gameWorld.Villagers -= 1;
        gameWorld.PlayerUnits[UnitType.Fighter] = currentCount + 1;
        gameWorld.FightersCreated += 1;
        gameWorldStatsService.Refresh(gameWorld);
        return false;
    }
}
