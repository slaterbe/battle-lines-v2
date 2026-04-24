using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class GatherGoldCommand : IHoldToExecuteCommand
{
    public const string CommandLabel = "Gather Gold";

    public GameCommandCategory Category => GameCommandCategory.Upgrade;
    public string Label => CommandLabel;
    public string HelpText => "Hold Enter to fill the bar. When it completes, gain 1 gold and restart the bar.";

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
