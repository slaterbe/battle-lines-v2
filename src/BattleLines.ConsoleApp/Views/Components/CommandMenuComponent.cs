using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Views.Components;

public class CommandMenuComponent
{
    public void Render(IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        ConsoleTextComponent.WriteLine("Commands", ConsoleColor.White);

        GameCommandCategory? currentCategory = null;
        for (var optionIndex = 0; optionIndex < commandOptions.Count; optionIndex++)
        {
            var commandOption = commandOptions[optionIndex];
            if (currentCategory != commandOption.Category)
            {
                currentCategory = commandOption.Category;
                ConsoleTextComponent.WriteLine($"  [{currentCategory}]", ConsoleColor.DarkCyan);
            }

            var isSelected = optionIndex == selectedCommandIndex;
            if (isSelected)
            {
                ConsoleTextComponent.Write("    ");
                ConsoleTextComponent.WriteHighlighted(" > ", ConsoleColor.Black, ConsoleColor.DarkYellow);
                ConsoleTextComponent.Write(" ");
                ConsoleTextComponent.WriteLine(commandOption.Label, ConsoleColor.Yellow);
                continue;
            }

            ConsoleTextComponent.WriteLine($"      {commandOption.Label}", ConsoleColor.Gray);
        }

        Console.WriteLine();
        if (commandOptions.Count > 0 && selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count)
        {
            ConsoleTextComponent.WriteLine("Hint", ConsoleColor.DarkYellow);
            ConsoleTextComponent.WriteLine($"  {commandOptions[selectedCommandIndex].HelpText}", ConsoleColor.Green);
            Console.WriteLine();
        }

        RenderControls();
    }

    private static void RenderControls()
    {
        ConsoleTextComponent.WriteLine("Controls", ConsoleColor.DarkYellow);
        ConsoleTextComponent.Write("  ");
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
