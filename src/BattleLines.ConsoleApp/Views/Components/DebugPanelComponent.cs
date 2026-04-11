using BattleLines.ConsoleApp.Debug;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class DebugPanelComponent
{
    private readonly RenderDiagnostics renderDiagnostics;

    public DebugPanelComponent(RenderDiagnostics renderDiagnostics)
    {
        this.renderDiagnostics = renderDiagnostics;
    }

    public void Render(GameWorld gameWorld)
    {
        var snapshot = renderDiagnostics.GetSnapshot();

        ConsoleTextComponent.NewLine();
        ConsoleTextComponent.NewLine();
        ConsoleTextComponent.WriteLine("--- Debug ---", ConsoleColor.DarkGray);
        ConsoleTextComponent.WriteLine($"View: {gameWorld.State}", ConsoleColor.DarkGray);
        ConsoleTextComponent.WriteLine(
            $"Render attempts/s: {snapshot.RenderAttemptsPerSecond}",
            ConsoleColor.DarkGray);
        ConsoleTextComponent.WriteLine(
            $"Terminal writes/s: {snapshot.TerminalWritesPerSecond}",
            ConsoleColor.DarkGray);
    }
}
