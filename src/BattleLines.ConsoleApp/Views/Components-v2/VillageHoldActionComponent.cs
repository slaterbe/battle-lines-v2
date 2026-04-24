using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class VillageHoldActionComponent
{
    private const int ProgressBarWidth = 22;

    public int MeasureHeight() => 3;

    public void Render(string label, double progress, int startX, int startY)
    {
        var clampedProgress = Math.Clamp(progress, 0, 1);
        var filledWidth = (int)Math.Round(clampedProgress * ProgressBarWidth, MidpointRounding.AwayFromZero);
        var bar = new string('#', filledWidth) + new string('-', ProgressBarWidth - filledWidth);
        var percentText = $"{clampedProgress * 100,3:0}%";

        ConsoleTextComponent.SetCursorPosition(startX, startY);
        ConsoleTextComponent.Write("Hold Action", ConsoleColor.DarkYellow);
        ConsoleTextComponent.Write(": ", ConsoleColor.DarkYellow);
        ConsoleTextComponent.WriteLine(label, ConsoleColor.Yellow);

        ConsoleTextComponent.Write("[", ConsoleColor.DarkGray);
        ConsoleTextComponent.Write(new string('#', filledWidth), ConsoleColor.Green);
        ConsoleTextComponent.Write(new string('-', ProgressBarWidth - filledWidth), ConsoleColor.DarkGray);
        ConsoleTextComponent.Write("] ", ConsoleColor.DarkGray);
        ConsoleTextComponent.WriteLine(percentText, ConsoleColor.White);

        ConsoleTextComponent.WriteLine("Keep holding Enter to earn 1 gold.", ConsoleColor.DarkGray);
    }
}
