using BattleLines.ConsoleApp.Debug;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class DebugPanelComponent
{
    private readonly RenderDiagnostics renderDiagnostics;
    private const int DebugSpacingRows = 2;

    public DebugPanelComponent(RenderDiagnostics renderDiagnostics)
    {
        this.renderDiagnostics = renderDiagnostics;
    }

    public void Render(GameWorld gameWorld)
    {
        var snapshot = renderDiagnostics.GetSnapshot();
        var startRow = ConsoleRenderLayout.MaxLineCount + DebugSpacingRows;

        WriteDebugLine(0, startRow++, "--- Debug ---");
        WriteDebugLine(0, startRow++, $"View: {gameWorld.State}");
        WriteDebugLine(0, startRow++, $"Render attempts/s: {snapshot.RenderAttemptsPerSecond}");
        WriteDebugLine(0, startRow, $"Terminal writes/s: {snapshot.TerminalWritesPerSecond}");
    }

    private static void WriteDebugLine(int left, int top, string text)
    {
        if (top >= Console.BufferHeight)
        {
            return;
        }

        var availableWidth = Math.Max(1, Console.WindowWidth - left);
        var paddedText = text.Length >= availableWidth
            ? text[..availableWidth]
            : text.PadRight(availableWidth);

        var originalForegroundColor = Console.ForegroundColor;
        Console.SetCursorPosition(left, top);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(paddedText);
        Console.ForegroundColor = originalForegroundColor;
    }
}
