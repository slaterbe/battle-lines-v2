using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class PostBattleController : GameStateControllerBase
{
    protected override IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld)
    {
        return
        [
            new ExitPostBattleCommand(
                "Return to Village",
                "Apply battle results and head back to the village.")
        ];
    }
}
