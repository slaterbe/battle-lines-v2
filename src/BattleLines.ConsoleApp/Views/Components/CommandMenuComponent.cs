namespace BattleLines.ConsoleApp.Views.Components;

public class CommandMenuComponent
{
    public void Render(IReadOnlyList<string> commandOptions, int selectedCommandIndex)
    {
        ConsoleTextComponent.WriteLine("Commands");

        for (var optionIndex = 0; optionIndex < commandOptions.Count; optionIndex++)
        {
            var isSelected = optionIndex == selectedCommandIndex;
            var prefix = isSelected ? "> " : "  ";
            var color = isSelected ? ConsoleColor.Yellow : ConsoleColor.Gray;

            ConsoleTextComponent.WriteLine($"{prefix}{commandOptions[optionIndex]}", color);
        }

        Console.WriteLine();
        ConsoleTextComponent.WriteLine("Use arrow keys to change selection and Enter to confirm.");
    }
}
