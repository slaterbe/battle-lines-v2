using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class GatherGoldCommand : IHoldToExecuteCommand
{
    public const string CommandLabel = "Gather Gold";

    public GameCommandCategory Category => GameCommandCategory.Gather;
    public string Label => CommandLabel;
    public string HelpText => "Press Enter to start gathering. Stay on it longer to speed up gold generation.";
    public GameCommandCost GetCost() => new(GoldGain: 1);

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State != GameState.Village)
        {
            return false;
        }

        gameWorld.Gold += 1;
        gameWorld.VillageGoldGatherProgress = 0;
        return false;
    }
}
