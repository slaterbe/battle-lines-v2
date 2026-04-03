using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public abstract class GameStateControllerBase : IGameStateController
{
    private IReadOnlyList<IGameCommand>? commands;
    private readonly IGameTickCommand? tickCommand;

    protected GameStateControllerBase(IGameTickCommand? tickCommand = null)
    {
        this.tickCommand = tickCommand;
    }

    public IReadOnlyList<GameCommandOption> GetCommandOptions()
    {
        return Commands
            .Select(command => new GameCommandOption(command.Label, command.HelpText.ReplaceLineEndings(" ")))
            .ToArray();
    }

    public bool HandleCommand(GameWorld gameWorld, int selectedCommandIndex)
    {
        if (selectedCommandIndex < 0 || selectedCommandIndex >= Commands.Count)
        {
            return false;
        }

        return Commands[selectedCommandIndex].Execute(gameWorld);
    }

    public void Tick(GameWorld gameWorld)
    {
        tickCommand?.Execute(gameWorld);
    }

    protected abstract IReadOnlyList<IGameCommand> CreateCommands();

    private IReadOnlyList<IGameCommand> Commands => commands ??= CreateCommands();
}
