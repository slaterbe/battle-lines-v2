using BattleLines.ConsoleApp.Models;
using System.Text;

namespace BattleLines.ConsoleApp.Views.Components;

public class PlayerUnitsComponent
{
    public string Render(GameWorld gameWorld)
    {
        if (gameWorld.PlayerUnits.Count == 0)
        {
            return "No player units.";
        }

        var builder = new StringBuilder();

        foreach (var playerUnit in gameWorld.PlayerUnits)
        {
            builder.AppendLine($"{playerUnit.Key}: {UnitDisplayComponent.RenderUnitCount(gameWorld, playerUnit.Key, playerUnit.Value)}");
        }

        builder.AppendLine($"Total Health: {BattleHistoryComponent.RenderPlayerHealth(gameWorld)}");
        builder.AppendLine($"Total Attack: {BattleHistoryComponent.RenderPlayerAttack(gameWorld)}");

        return builder.ToString().TrimEnd();
    }
}
