using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class RenderService
{
    public void Render(GameWorld gameWorld)
    {
        Console.SetCursorPosition(0, 0);

        Console.WriteLine("Battle Lines");
        Console.WriteLine(
            $"Commoners: {gameWorld.Commoners} (+{gameWorld.CommonerProduction}/tick)    Spears: {gameWorld.Spears} (+{gameWorld.SpearProduction}/tick)    State: {(gameWorld.IsPaused ? "Paused" : "Running")}        ");
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("Controls: [P] Pause/Resume  [Q] Quit");
        Console.WriteLine();

        if (gameWorld.IsPaused)
        {
            Console.WriteLine("Production is paused. Press P to resume.                ");
        }
        else
        {
            Console.WriteLine("Production is active. Resources increase each tick.     ");
        }
    }
}
