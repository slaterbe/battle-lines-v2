using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class BattleOutcomeController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld)
    {
        return
        [
            new ExitPostBattleCommand(
                "Return to Village",
                "Review the battle outcome and return to the village.")
        ];
    }
}
