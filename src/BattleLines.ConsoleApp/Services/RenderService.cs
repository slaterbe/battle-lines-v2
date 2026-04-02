using BattleLines.ConsoleApp.Models;
using System.Text;

namespace BattleLines.ConsoleApp.Services;

public class RenderService
{
    public void Render(GameWorld gameWorld, IReadOnlyList<string> commandOptions, int selectedCommandIndex)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);

        WriteLineWithColor("Battle Lines", ConsoleColor.White);
        WriteLineWithColor(
            $"Commoners: {gameWorld.Commoners}    Spears: {gameWorld.Spears}    Gold: {gameWorld.Gold}    State: {gameWorld.State}        ");
        WriteLineWithColor(new string('=', 80));

        if (gameWorld.State == GameState.Battle)
        {
            WriteLineWithColor("Battle is active. Both sides deal damage each tick.     ", ConsoleColor.Yellow);
        }
        else if (gameWorld.State == GameState.PreBattle)
        {
            WriteLineWithColor("Pre-battle is active. Review your army and begin combat.", ConsoleColor.Yellow);
        }
        else if (gameWorld.State == GameState.PostBattle)
        {
            var resultText = gameWorld.LastBattleWon
                ? "Battle ended. Claim your reward and continue.          "
                : "Battle ended. Continue to the next wave.               ";
            WriteLineWithColor(resultText, ConsoleColor.Yellow);
        }
        else
        {
            WriteLineWithColor("Village is active. Resources only change from rewards.  ", ConsoleColor.Green);
        }

        Console.WriteLine();
        WriteLineWithColor("Current Wave", ConsoleColor.Red);
        RenderCurrentWave(gameWorld);
        Console.WriteLine();
        Console.WriteLine();
        WriteLineWithColor("Player Units", ConsoleColor.Blue);
        WriteLineWithColor(RenderPlayerUnits(gameWorld), ConsoleColor.Blue);
        Console.WriteLine();
        WriteLineWithColor("Commands");
        RenderCommandOptions(commandOptions, selectedCommandIndex);
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

        builder.AppendLine($"Total Health: {gameWorld.PlayerTotalHealth}");
        builder.AppendLine($"Total Attack: {gameWorld.PlayerTotalAttack}");

        return builder.ToString().TrimEnd();
    }

    private static void RenderCurrentWave(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaveList.Count == 0)
        {
            WriteLineWithColor("No enemy waves queued.", ConsoleColor.Red);
            return;
        }

        var currentWave = gameWorld.EnemyWaveList[0];

        foreach (var enemy in currentWave.Enemies)
        {
            WriteLineWithColor($"{enemy.EnemyType}: {enemy.Count}", ConsoleColor.Red);
        }

        WriteLineWithColor($"Total Health: {gameWorld.CurrentWaveTotalHealth}", ConsoleColor.Red);
        WriteLineWithColor($"Total Attack: {gameWorld.CurrentWaveTotalAttack}", ConsoleColor.Red);
        WriteLineWithColor($"Reward: {currentWave.RewardAmount} {currentWave.RewardType}", ConsoleColor.Yellow);
    }

    private static void RenderCommandOptions(IReadOnlyList<string> commandOptions, int selectedCommandIndex)
    {
        for (var optionIndex = 0; optionIndex < commandOptions.Count; optionIndex++)
        {
            var isSelected = optionIndex == selectedCommandIndex;
            var prefix = isSelected ? "> " : "  ";
            var color = isSelected ? ConsoleColor.Yellow : ConsoleColor.Gray;

            WriteLineWithColor($"{prefix}{commandOptions[optionIndex]}", color);
        }

        Console.WriteLine();
        WriteLineWithColor("Use arrow keys to change selection and Enter to confirm.");
    }
}
