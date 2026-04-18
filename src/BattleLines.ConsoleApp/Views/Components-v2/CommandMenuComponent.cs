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
                ConsoleTextComponent.WriteHighlighted(" > ", ConsoleColor.Black, ConsoleColor.DarkYellow);
                ConsoleTextComponent.Write(" ");
                ConsoleTextComponent.Write(commandOption.Label, ConsoleColor.Yellow);

                if (state.ShowAnimatedEnterPrompt)
                {
                    ConsoleTextComponent.Write(" ");
                    RenderAnimatedEnterPrompt();
                }

                ConsoleTextComponent.NewLine();
                continue;
            }

            ConsoleTextComponent.Write("   ", ConsoleColor.Gray);
            ConsoleTextComponent.WriteLine(commandOption.Label, ConsoleColor.Gray);
        }

        ConsoleTextComponent.NewLine();
        RenderFooter(state);
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
            ConsoleTextComponent.WriteLine(
                state.CommandOptions[state.SelectedCommandIndex].HelpText,
                ConsoleColor.White);
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
