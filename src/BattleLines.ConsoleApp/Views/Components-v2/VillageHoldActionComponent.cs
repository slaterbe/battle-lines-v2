using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class VillageHoldActionComponent
{
    private const int ProgressBarWidth = 22;

    public int MeasureHeight() => 3;

    public void Render(string label, double progress, bool isActive, double speedMultiplier, int startX, int startY)
    {
        var clampedProgress = Math.Clamp(progress, 0, 1);
        var filledWidth = (int)Math.Round(clampedProgress * ProgressBarWidth, MidpointRounding.AwayFromZero);
        var percentText = $"{clampedProgress * 100,3:0}%";
        var statusText = isActive
            ? $"Gathering... {speedMultiplier:0.0}x speed. Escape or move to cancel."
            : "Press Enter to start gathering.";

        ConsoleTextComponent.SetCursorPosition(startX, startY);
        ConsoleTextComponent.Write("Gather Action", ConsoleColor.DarkYellow);
        ConsoleTextComponent.Write(": ", ConsoleColor.DarkYellow);
        ConsoleTextComponent.WriteLine(label, ConsoleColor.Yellow);

        ConsoleTextComponent.Write("[", ConsoleColor.DarkGray);
        ConsoleTextComponent.Write(new string('#', filledWidth), ConsoleColor.Green);
        ConsoleTextComponent.Write(new string('-', ProgressBarWidth - filledWidth), ConsoleColor.DarkGray);
        ConsoleTextComponent.Write("] ", ConsoleColor.DarkGray);
        ConsoleTextComponent.WriteLine(percentText, ConsoleColor.White);

        ConsoleTextComponent.WriteLine(statusText, ConsoleColor.DarkGray);
    }
}
