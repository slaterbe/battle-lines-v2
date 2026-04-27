using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Views.Components;

public class CommandMenuComponent
{
    private const int CategoryColumnWidth = 11;

    public int MeasureHeight(IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var height = commandOptions.Count;
        height++;

        var hasSelectedCommand = commandOptions.Count > 0 &&
            selectedCommandIndex >= 0 &&
            selectedCommandIndex < commandOptions.Count;

        height += hasSelectedCommand ? 2 : 1;
        return height;
    }

    public void Render(
        IReadOnlyList<GameCommandOption> commandOptions,
        int selectedCommandIndex,
        bool showAnimatedEnterPrompt = false)
    {
        GameCommandCategory? currentCategory = null;
        for (var optionIndex = 0; optionIndex < commandOptions.Count; optionIndex++)
        {
            var commandOption = commandOptions[optionIndex];
            var isSelected = optionIndex == selectedCommandIndex;
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

                if (showAnimatedEnterPrompt && !commandOption.IsDisabled)
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
        RenderFooter(commandOptions, selectedCommandIndex);
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

    private static void RenderAnimatedEnterPrompt()
    {
        var totalMilliseconds = Environment.TickCount64;
        var isBrightPhase = (totalMilliseconds / 700) % 2 == 0;
        var foregroundColor = isBrightPhase ? ConsoleColor.Black : ConsoleColor.DarkGray;
        var backgroundColor = isBrightPhase ? ConsoleColor.Green : ConsoleColor.DarkGreen;
        WriteKeycap("ENTER", foregroundColor, backgroundColor);
    }

    private static void RenderFooter(IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var hasSelectedCommand = commandOptions.Count > 0 &&
            selectedCommandIndex >= 0 &&
            selectedCommandIndex < commandOptions.Count;

        if (hasSelectedCommand)
        {
            ConsoleTextComponent.Write("Hint", ConsoleColor.White);
            ConsoleTextComponent.Write(": ", ConsoleColor.White);
            var helpColor = commandOptions[selectedCommandIndex].IsDisabled ? ConsoleColor.DarkGray : ConsoleColor.White;
            ConsoleTextComponent.WriteLine(commandOptions[selectedCommandIndex].HelpText, helpColor);
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
