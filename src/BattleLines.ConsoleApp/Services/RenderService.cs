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
            WriteLineWithColor("Battle: Reinforce your army and start combat.", ConsoleColor.Green);
        }
        else if (gameWorld.State == GameState.PostBattle)
        {
            var resultText = gameWorld.LastBattleWon
                ? "Wave Defeated: Claim your reward and prepare for the next wave"
                : "Battle ended. Continue to the next wave.               ";
            WriteLineWithColor(resultText, ConsoleColor.Yellow);
        }
        else
        {
            WriteLineWithColor("Village: Choose upgrades or start a battle", ConsoleColor.Green);
        }

        Console.WriteLine();
        RenderWaveProgress(gameWorld);
        RenderWaveReward(gameWorld);
        Console.WriteLine();
        RenderCurrentWave(gameWorld);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
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
            builder.AppendLine($"{playerUnit.Key}: {RenderUnitCount(gameWorld, playerUnit.Key, playerUnit.Value)}");
        }

        builder.AppendLine($"Total Health: {RenderPlayerHealth(gameWorld)}");
        builder.AppendLine($"Total Attack: {RenderPlayerAttack(gameWorld)}");

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
            WriteLineWithColor($"{enemy.EnemyType}: {RenderUnitCount(gameWorld, enemy.EnemyType, enemy.Count)}", ConsoleColor.Red);
        }

        WriteLineWithColor($"Total Health: {RenderEnemyHealth(gameWorld)}", ConsoleColor.Red);
        WriteLineWithColor($"Total Attack: {gameWorld.CurrentWaveTotalAttack}", ConsoleColor.Red);
    }

    private static void RenderWaveProgress(GameWorld gameWorld)
    {
        var totalWaveCount = Math.Max(0, gameWorld.TotalWaveCount);
        var remainingWaveCount = Math.Max(0, gameWorld.EnemyWaveList.Count);
        var defeatedWaveCount = Math.Max(0, totalWaveCount - remainingWaveCount);
        const int progressBarWidth = 20;

        var filledSegments = totalWaveCount == 0
            ? 0
            : (int)Math.Round((double)defeatedWaveCount / totalWaveCount * progressBarWidth, MidpointRounding.AwayFromZero);
        filledSegments = Math.Clamp(filledSegments, 0, progressBarWidth);

        var progressBar = $"[{new string('#', filledSegments)}{new string('-', progressBarWidth - filledSegments)}]";
        WriteLineWithColor(
            $"Progress: {progressBar} {defeatedWaveCount}/{totalWaveCount} defeated, {remainingWaveCount} remaining",
            ConsoleColor.Cyan);
    }

    private static void RenderWaveReward(GameWorld gameWorld)
    {
        if (gameWorld.EnemyWaveList.Count == 0)
        {
            return;
        }

        var currentWave = gameWorld.EnemyWaveList[0];
        WriteLineWithColor($"Reward: {currentWave.RewardAmount} {currentWave.RewardType}", ConsoleColor.Yellow);
    }

    private static string RenderPlayerHealth(GameWorld gameWorld)
    {
        if (gameWorld.PlayerHealthHistory.Count == 0)
        {
            return gameWorld.PlayerTotalHealth.ToString();
        }

        var history = gameWorld.PlayerHealthHistory.Select(health => health.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.PlayerTotalHealth}";
    }

    private static string RenderEnemyHealth(GameWorld gameWorld)
    {
        if (gameWorld.EnemyHealthHistory.Count == 0)
        {
            return gameWorld.CurrentWaveTotalHealth.ToString();
        }

        var history = gameWorld.EnemyHealthHistory.Select(health => health.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.CurrentWaveTotalHealth}";
    }

    private static string RenderPlayerAttack(GameWorld gameWorld)
    {
        if (gameWorld.PlayerAttackHistory.Count == 0)
        {
            return gameWorld.PlayerTotalAttack.ToString();
        }

        var history = gameWorld.PlayerAttackHistory.Select(attack => attack.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.PlayerTotalAttack}";
    }

    private static string RenderUnitCount(GameWorld gameWorld, UnitType unitType, int count)
    {
        return unitType switch
        {
            UnitType.SpearmenLvl1 => RenderSpearmenPositions(gameWorld, count),
            UnitType.GiantRat => new string('|', Math.Max(0, count)),
            _ => count.ToString()
        };
    }

    private static string RenderSpearmenPositions(GameWorld gameWorld, int count)
    {
        var clampedMaxPositions = Math.Max(0, gameWorld.MaxSpearmenPositions);
        var displayedCount = Math.Clamp(count, 0, clampedMaxPositions);

        if (gameWorld.State != GameState.Battle &&
            !(gameWorld.State == GameState.PostBattle && gameWorld.HasPendingPostBattleResolution))
        {
            return $"{new string('|', displayedCount)}{new string('O', clampedMaxPositions - displayedCount)}";
        }

        if (!UnitCatalog.DefaultUnits.TryGetValue(UnitType.SpearmenLvl1, out var spearmanModel) || spearmanModel.Health <= 0)
        {
            return $"{new string('|', displayedCount)}{new string('O', clampedMaxPositions - displayedCount)}";
        }

        var healthLost = Math.Max(0, gameWorld.PlayerHealthAtBattleStart - gameWorld.PlayerTotalHealth);
        var spearmenLost = Math.Min(gameWorld.SpearmenCountAtBattleStart, healthLost / spearmanModel.Health);
        var survivingSpearmen = Math.Max(0, gameWorld.SpearmenCountAtBattleStart - spearmenLost);
        var emptyPositions = Math.Max(0, clampedMaxPositions - gameWorld.SpearmenCountAtBattleStart);

        return $"{new string('|', survivingSpearmen)}{new string('X', spearmenLost)}{new string('O', emptyPositions)}";
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
