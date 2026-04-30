using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2;
using BattleLines.ConsoleApp.Views.ComponentsV2.Rendering;

namespace BattleLines.ConsoleApp.Views;

public class IntroductionView : IGameView
{
    private static readonly ComponentsV2.GameTitleComponent GameTitle = new();
    private static readonly DelayedTextRender<IntroductionComponent> Introduction =
        new(new IntroductionComponent(), 60, ConsoleColor.Green);
    private static readonly DelayedContentRender<IntroductionCommandComponent, IReadOnlyList<GameCommandOption>> IntroductionCommand =
        new(new IntroductionCommandComponent(), TimeSpan.FromSeconds(5));

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        const int titleStartX = 0;
        const int titleStartY = 1;
        const int introductionStartX = 0;
        const int introductionStartY = 3;
        const int commandStartX = 0;

        GameTitle.Render(titleStartX, titleStartY);
        Introduction.Render(gameWorld, introductionStartX, introductionStartY);
        var commandStartY = ConsoleTextComponent.CursorTop + 1;
        IntroductionCommand.Render(commandOptions, commandStartX, commandStartY);
    }
}
