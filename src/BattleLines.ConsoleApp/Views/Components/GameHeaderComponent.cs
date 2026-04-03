using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class GameHeaderComponent
{
    public void Render(GameWorld gameWorld, string statusMessage, ConsoleColor statusColor)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        ConsoleTextComponent.WriteLine("Battle Lines", ConsoleColor.White);
        ConsoleTextComponent.WriteLine(
            $"Villagers: {gameWorld.Villagers}    Spears: {gameWorld.Spears}    Gold: {gameWorld.Gold}    State: {gameWorld.State}        ");
        ConsoleTextComponent.WriteLine(new string('=', 80));
        ConsoleTextComponent.WriteLine(statusMessage, statusColor);
    }
}
