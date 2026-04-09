using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public static class BattleHistoryComponent
{
    public static string RenderPlayerHealth(GameWorld gameWorld)
    {
        if (gameWorld.PlayerHealthHistory.Count == 0)
        {
            return gameWorld.PlayerTotalHealth.ToString();
        }

        var history = gameWorld.PlayerHealthHistory.Select(health => health.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.PlayerTotalHealth}";
    }

    public static string RenderEnemyHealth(GameWorld gameWorld)
    {
        if (gameWorld.EnemyHealthHistory.Count == 0)
        {
            return gameWorld.CurrentWaveTotalHealth.ToString();
        }

        var history = gameWorld.EnemyHealthHistory.Select(health => health.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.CurrentWaveTotalHealth}";
    }

    public static string RenderEnemyAttack(GameWorld gameWorld)
    {
        if (gameWorld.EnemyAttackHistory.Count == 0)
        {
            return gameWorld.CurrentWaveTotalAttack.ToString();
        }

        var history = gameWorld.EnemyAttackHistory.Select(attack => attack.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.CurrentWaveTotalAttack}";
    }

    public static string RenderEnemyMaxAttack(GameWorld gameWorld)
    {
        if (gameWorld.EnemyMaxAttackHistory.Count == 0)
        {
            return gameWorld.CurrentWaveTotalMaxAttack.ToString();
        }

        var history = gameWorld.EnemyMaxAttackHistory.Select(attack => attack.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.CurrentWaveTotalMaxAttack}";
    }

    public static string RenderPlayerAttack(GameWorld gameWorld)
    {
        if (gameWorld.PlayerAttackHistory.Count == 0)
        {
            return gameWorld.PlayerTotalAttack.ToString();
        }

        var history = gameWorld.PlayerAttackHistory.Select(attack => attack.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.PlayerTotalAttack}";
    }

    public static string RenderPlayerMaxAttack(GameWorld gameWorld)
    {
        if (gameWorld.PlayerMaxAttackHistory.Count == 0)
        {
            return gameWorld.PlayerTotalMaxAttack.ToString();
        }

        var history = gameWorld.PlayerMaxAttackHistory.Select(attack => attack.ToString());
        return $"{string.Join(" -> ", history)} -> {gameWorld.PlayerTotalMaxAttack}";
    }
}
