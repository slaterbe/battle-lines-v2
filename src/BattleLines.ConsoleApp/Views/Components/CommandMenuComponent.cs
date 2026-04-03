using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Views.Components;

public class CommandMenuComponent
{
    public void Render(IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        ConsoleTextComponent.WriteLine("Commands");

        for (var optionIndex = 0; optionIndex < commandOptions.Count; optionIndex++)
        {
            var isSelected = optionIndex == selectedCommandIndex;
            var prefix = isSelected ? "> " : "  ";
            var color = isSelected ? ConsoleColor.Yellow : ConsoleColor.Gray;

            ConsoleTextComponent.WriteLine($"{prefix}{commandOptions[optionIndex].Label}", color);
        }

        Console.WriteLine();
        if (commandOptions.Count > 0 && selectedCommandIndex >= 0 && selectedCommandIndex < commandOptions.Count)
        {
            ConsoleTextComponent.WriteLine(commandOptions[selectedCommandIndex].HelpText, ConsoleColor.Green);
            Console.WriteLine();
        }

        ConsoleTextComponent.WriteLine("Use arrow keys to change selection and Enter to confirm.");
    }
}
