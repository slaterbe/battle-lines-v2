using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2;

namespace BattleLines.ConsoleApp.Views;

public class IntroductionView : IGameView
{
    private static readonly ComponentsV2.GameTitleComponent GameTitle = new();
    private static readonly IntroductionComponent Introduction = new();
    private static readonly ComponentsV2.CommandMenuComponent CommandMenu = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        const int titleStartX = 0;
        const int titleStartY = 1;
        const int introductionStartX = 0;
        const int introductionStartY = 3;
        const int commandMenuStartX = 0;
        const int commandMenuStartY = -3;

        GameTitle.Render(titleStartX, titleStartY);
        Introduction.Render(gameWorld, introductionStartX, introductionStartY);
        var commandMenuState = new CommandMenuState(commandOptions, selectedCommandIndex);
        CommandMenu.Render(commandMenuState, commandMenuStartX, commandMenuStartY);
    }
}
