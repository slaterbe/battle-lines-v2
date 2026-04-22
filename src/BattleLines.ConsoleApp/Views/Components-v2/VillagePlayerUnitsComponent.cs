using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class VillagePlayerUnitsComponent
{
    private readonly PlayerUnitsComponent playerUnitsComponent = new();

    public void Render(GameWorld gameWorld, string selectedCommandLabel, int startX, int startY)
    {
        ConsoleTextComponent.SetCursorPosition(startX, startY);
        playerUnitsComponent.Render(gameWorld, selectedCommandLabel);
    }
}
