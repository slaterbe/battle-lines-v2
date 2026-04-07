using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public abstract class GameStateControllerBase : IGameStateController
{
    private readonly IGameTickCommand? tickCommand;

    protected GameStateControllerBase(IGameTickCommand? tickCommand = null)
    {
        this.tickCommand = tickCommand;
    }

    public IReadOnlyList<GameCommandOption> GetCommandOptions(GameWorld gameWorld)
    {
        return GetCommands(gameWorld)
            .Select(command => new GameCommandOption(
                command.Category,
                command.Label,
                command.HelpText.ReplaceLineEndings(" "),
                command.GetCost()))
            .ToArray();
    }

    public bool HandleCommand(GameWorld gameWorld, int selectedCommandIndex)
    {
        var commands = GetCommands(gameWorld);
        if (selectedCommandIndex < 0 || selectedCommandIndex >= commands.Count)
        {
            return false;
        }

        return commands[selectedCommandIndex].Execute(gameWorld);
    }

    public void Tick(GameWorld gameWorld)
    {
        tickCommand?.Execute(gameWorld);
    }

    protected abstract IReadOnlyList<IGameCommand> CreateCommands(GameWorld gameWorld);

    private IReadOnlyList<IGameCommand> GetCommands(GameWorld gameWorld) => CreateCommands(gameWorld)
        .OrderBy(command => command.Category)
        .ToArray();
}
