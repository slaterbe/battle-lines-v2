using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class CommandMenuComponent
{
    private const int CategoryColumnWidth = 11;

    public int MeasureHeight(CommandMenuState state)
    {
        var height = state.CommandOptions.Count;
        height++;

        var hasSelectedCommand = HasSelectedCommand(state);
        height += hasSelectedCommand ? 2 : 1;
        return height;
    }

    public void Render(CommandMenuState state, int startX, int startY)
    {
        ConsoleTextComponent.SetCursorPosition(startX, startY);

        GameCommandCategory? currentCategory = null;
        for (var optionIndex = 0; optionIndex < state.CommandOptions.Count; optionIndex++)
        {
            var commandOption = state.CommandOptions[optionIndex];
            var isSelected = optionIndex == state.SelectedCommandIndex;
            var categoryLabel = currentCategory != commandOption.Category
                ? $"[{commandOption.Category}]".PadRight(CategoryColumnWidth)
                : new string(' ', CategoryColumnWidth);

            currentCategory = commandOption.Category;

            ConsoleTextComponent.Write(categoryLabel, ConsoleColor.DarkCyan);

            if (isSelected)
            {
                RenderSelectionMarker(commandOption.IsDisabled);
                ConsoleTextComponent.Write(" ");
                ConsoleTextComponent.Write(commandOption.Label, commandOption.IsDisabled ? ConsoleColor.DarkGray : ConsoleColor.Yellow);

                if (state.ShowAnimatedEnterPrompt && !commandOption.IsDisabled)
                {
                    ConsoleTextComponent.Write(" ");
                    RenderAnimatedEnterPrompt();
                }

                ConsoleTextComponent.NewLine();
                continue;
            }

            ConsoleTextComponent.Write("   ", ConsoleColor.Gray);
            ConsoleTextComponent.WriteLine(commandOption.Label, commandOption.IsDisabled ? ConsoleColor.DarkGray : ConsoleColor.Gray);
        }

        ConsoleTextComponent.NewLine();
        RenderFooter(state);
    }

    private static void RenderSelectionMarker(bool isDisabled)
    {
        if (isDisabled)
        {
            ConsoleTextComponent.Write(" > ", ConsoleColor.DarkGray);
            return;
        }

        var totalMilliseconds = Environment.TickCount64;
        var isBrightPhase = (totalMilliseconds / 500) % 2 == 0;
        if (!isBrightPhase)
        {
            ConsoleTextComponent.Write("   ", ConsoleColor.Gray);
            return;
        }

        ConsoleTextComponent.WriteHighlighted(" > ", ConsoleColor.Black, ConsoleColor.DarkYellow);
    }

    private static bool HasSelectedCommand(CommandMenuState state)
    {
        return state.CommandOptions.Count > 0 &&
            state.SelectedCommandIndex >= 0 &&
            state.SelectedCommandIndex < state.CommandOptions.Count;
    }

    private static void RenderAnimatedEnterPrompt()
    {
        var totalMilliseconds = Environment.TickCount64;
        var isBrightPhase = (totalMilliseconds / 700) % 2 == 0;
        var foregroundColor = isBrightPhase ? ConsoleColor.Black : ConsoleColor.DarkGray;
        var backgroundColor = isBrightPhase ? ConsoleColor.Green : ConsoleColor.DarkGreen;
        WriteKeycap("ENTER", foregroundColor, backgroundColor);
    }

    private static void RenderFooter(CommandMenuState state)
    {
        if (HasSelectedCommand(state))
        {
            ConsoleTextComponent.Write("Hint", ConsoleColor.White);
            ConsoleTextComponent.Write(": ", ConsoleColor.White);
            var helpColor = state.CommandOptions[state.SelectedCommandIndex].IsDisabled ? ConsoleColor.DarkGray : ConsoleColor.White;
            ConsoleTextComponent.WriteLine(
                state.CommandOptions[state.SelectedCommandIndex].HelpText,
                helpColor);
        }

        ConsoleTextComponent.Write("Controls", ConsoleColor.DarkYellow);
        ConsoleTextComponent.Write(": ", ConsoleColor.DarkYellow);
        WriteKeycap("Up", ConsoleColor.Black, ConsoleColor.Cyan);
        ConsoleTextComponent.Write(" ");
        WriteKeycap("Down", ConsoleColor.Black, ConsoleColor.Cyan);
        ConsoleTextComponent.Write(" to change selection, ", ConsoleColor.DarkGray);
        WriteKeycap("Enter", ConsoleColor.Black, ConsoleColor.Green);
        ConsoleTextComponent.WriteLine(" to confirm.", ConsoleColor.DarkGray);
    }

    private static void WriteKeycap(string label, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        ConsoleTextComponent.WriteHighlighted($"[{label}]", foregroundColor, backgroundColor);
    }
}
