using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class DebugPanelComponent
{
    public void Render(GameWorld gameWorld)
    {
        ConsoleTextComponent.NewLine();
        ConsoleTextComponent.WriteLine("--- Debug ---", ConsoleColor.DarkGray);
        ConsoleTextComponent.WriteLine($"View: {gameWorld.State}", ConsoleColor.DarkGray);
    }
}
