using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class VillageStatusComponent
{
    private const int MinimumFarmSlots = 8;
    private const int FoodProductionPerFarm = 2;

    public void Render(GameWorld gameWorld, string selectedCommandLabel, int startX, int startY)
    {
        var farmCount = Math.Max(0, gameWorld.FoodProduction / FoodProductionPerFarm);
        var showPreviewFarm = selectedCommandLabel == "Expand Farm";
        var slotCount = Math.Max(MinimumFarmSlots, farmCount + (showPreviewFarm ? 1 : 0));

        ConsoleTextComponent.SetCursorPosition(startX, startY);
        ConsoleTextComponent.Write("Farm", ConsoleColor.Blue);
        ConsoleTextComponent.Write(": ", ConsoleColor.Blue);

        for (var farmIndex = 0; farmIndex < slotCount; farmIndex++)
        {
            if (farmIndex < farmCount)
            {
                ConsoleTextComponent.Write("F", ConsoleColor.Blue);
                continue;
            }

            if (showPreviewFarm && farmIndex == farmCount)
            {
                ConsoleTextComponent.Write("F", ConsoleColor.Green);
                continue;
            }

            ConsoleTextComponent.Write("0", ConsoleColor.Blue);
        }

        ConsoleTextComponent.NewLine();
    }
}
