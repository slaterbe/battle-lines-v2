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
                ConsoleTextComponent.WriteHighlighted(" > ", ConsoleColor.Black, ConsoleColor.DarkYellow);
                ConsoleTextComponent.Write(" ");
                ConsoleTextComponent.Write(commandOption.Label, ConsoleColor.Yellow);

                if (showAnimatedEnterPrompt)
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
        RenderFooter(commandOptions, selectedCommandIndex);
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
            ConsoleTextComponent.Write("Hint", ConsoleColor.DarkYellow);
            ConsoleTextComponent.Write(": ", ConsoleColor.DarkYellow);
            ConsoleTextComponent.WriteLine(commandOptions[selectedCommandIndex].HelpText, ConsoleColor.Green);
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
