using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Views.Components;
using BattleLines.ConsoleApp.Views.ComponentsV2.Rendering;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class IntroductionCommandComponent : IDelayedContentComponent<IReadOnlyList<GameCommandOption>>
{
    public int MeasureHeight(IReadOnlyList<GameCommandOption> commandOptions)
    {
        return 1;
    }

    public void Render(IReadOnlyList<GameCommandOption> commandOptions, int startX, int startY)
    {
        if (commandOptions.Count == 0)
        {
            return;
        }

        var commandOption = commandOptions[0];
        var commandColor = commandOption.IsDisabled ? ConsoleColor.DarkGray : ConsoleColor.Yellow;
        var categoryColor = commandOption.IsDisabled ? ConsoleColor.DarkGray : ConsoleColor.DarkCyan;
        var isBrightPhase = IsBrightPhase();

        ConsoleTextComponent.SetCursorPosition(startX, startY);
        ConsoleTextComponent.Write($"[{commandOption.Category}]", categoryColor);
        ConsoleTextComponent.Write(" ", ConsoleColor.Gray);
        RenderCaret(commandOption.IsDisabled, isBrightPhase);
        ConsoleTextComponent.Write(" ", ConsoleColor.Gray);
        ConsoleTextComponent.Write(commandOption.Label, commandColor);
        ConsoleTextComponent.Write(" ", ConsoleColor.Gray);
        RenderAnimatedEnterPrompt(commandOption.IsDisabled, isBrightPhase);
    }

    private static void RenderCaret(bool isDisabled, bool isBrightPhase)
    {
        if (isDisabled)
        {
            ConsoleTextComponent.Write(">", ConsoleColor.DarkGray);
            return;
        }

        ConsoleTextComponent.Write(isBrightPhase ? ">" : " ", ConsoleColor.DarkYellow);
    }

    private static void RenderAnimatedEnterPrompt(bool isDisabled, bool isBrightPhase)
    {
        if (isDisabled)
        {
            WriteKeycap(ConsoleColor.DarkGray, ConsoleColor.Black);
            return;
        }

        if (!isBrightPhase)
        {
            ConsoleTextComponent.Write("       ", ConsoleColor.Gray);
            return;
        }

        WriteKeycap(ConsoleColor.Black, ConsoleColor.Green);
    }

    private static bool IsBrightPhase()
    {
        var totalMilliseconds = Environment.TickCount64;
        return (totalMilliseconds / 700) % 2 == 0;
    }

    private static void WriteKeycap(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        ConsoleTextComponent.WriteHighlighted("[Enter]", foregroundColor, backgroundColor);
    }
}
