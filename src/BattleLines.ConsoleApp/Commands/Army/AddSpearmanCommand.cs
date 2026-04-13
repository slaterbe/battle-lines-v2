using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Commands;

public class AddSpearmanCommand : IGameCommand
{
    private readonly GameWorldStatsService gameWorldStatsService = new();

    public GameCommandCategory Category => GameCommandCategory.Army;
    public string Label => "Recruit Spearmen";
    public string HelpText => "Spend 1 villager and 1 spear to recruit a spearman.";

    public GameCommandCost GetCost() => new(Villagers: 1, Spears: 1);

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village && gameWorld.State != GameState.PreBattle)
        {
            return false;
        }

        if (gameWorld.Villagers < 1 || gameWorld.Spears < 1)
        {
            return false;
        }

        var totalArmySize = gameWorld.PlayerUnits.Values.Sum();
        if (totalArmySize >= gameWorld.FrontLineCapacity)
        {
            return false;
        }

        gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var currentCount);
        gameWorld.Villagers -= 1;
        gameWorld.Spears -= 1;
        gameWorld.PlayerUnits[UnitType.SpearmenLvl1] = currentCount + 1;
        gameWorldStatsService.Refresh(gameWorld);
        return false;
    }
}
