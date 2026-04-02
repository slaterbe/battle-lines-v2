using BattleLines.ConsoleApp.Models;
using System.Text;

namespace BattleLines.ConsoleApp.Services;

public class RenderService
{
    public void Render(GameWorld gameWorld)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        WriteLineWithColor("Battle Lines", ConsoleColor.White);
        WriteLineWithColor(
            $"Commoners: {gameWorld.Commoners} (+{gameWorld.CommonerProduction}/tick)    Spears: {gameWorld.Spears} (+{gameWorld.SpearProduction}/tick)    State: {(gameWorld.IsPaused ? "Paused" : "Running")}        ");
        WriteLineWithColor(new string('=', 80));

        if (gameWorld.IsPaused)
        {
            WriteLineWithColor("Production is paused. Press P to resume.                ", ConsoleColor.Yellow);
        }
        else
        {
            WriteLineWithColor("Production is active. Resources increase each tick.     ", ConsoleColor.Green);
        }

        Console.WriteLine();
        WriteLineWithColor("Current Wave", ConsoleColor.Red);
        WriteLineWithColor(RenderCurrentWave(gameWorld), ConsoleColor.Red);
        Console.WriteLine();
        Console.WriteLine();
        WriteLineWithColor("Player Units", ConsoleColor.Blue);
        WriteLineWithColor(RenderPlayerUnits(gameWorld), ConsoleColor.Blue);
        Console.WriteLine();
        WriteLineWithColor("Controls: [P] Pause/Resume  [Q] Quit");
    }

    private static void WriteLineWithColor(string text, ConsoleColor color = ConsoleColor.Gray)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = originalColor;
    }

    private static string RenderPlayerUnits(GameWorld gameWorld)
    {
        if (gameWorld.PlayerUnits.Count == 0)
        {
            return "No player units.";
        }

        var builder = new StringBuilder();

        foreach (var playerUnit in gameWorld.PlayerUnits)
        {
            builder.AppendLine($"{playerUnit.Key}: {playerUnit.Value}");
        }

        return builder.ToString().TrimEnd();
    }

    private static string RenderCurrentWave(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaveList.Count == 0)
        {
            return "No enemy waves queued.";
        }

        var currentWave = gameWorld.EnemyWaveList[0];
        var builder = new StringBuilder();

        foreach (var enemy in currentWave.Enemies)
        {
            builder.AppendLine($"{enemy.EnemyType}: {enemy.Count}");
        }

        builder.Append($"Reward: {currentWave.RewardAmount} {currentWave.RewardType}");

        return builder.ToString();
    }
}
