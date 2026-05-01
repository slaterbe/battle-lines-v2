using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class VillageStatusComponent
{
    private const int MinimumFarmSlots = 8;
    private const int FoodProductionPerFarm = 2;
    private const int MaximumGateHouseSlots = GameWorld.MaxGateHouseLevel;

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

        if (!gameWorld.IsUpgradesVisible)
        {
            return;
        }

        var gateHouseLevel = gameWorld.GateHouseLevel;
        var showPreviewGateHouse = selectedCommandLabel == "Upgrade GateHouse" && gateHouseLevel < MaximumGateHouseSlots;
        var gateHouseSlotCount = Math.Max(MaximumGateHouseSlots, gateHouseLevel + (showPreviewGateHouse ? 1 : 0));

        ConsoleTextComponent.Write("GateHouse", ConsoleColor.DarkYellow);
        ConsoleTextComponent.Write(": ", ConsoleColor.DarkYellow);

        for (var gateHouseIndex = 0; gateHouseIndex < gateHouseSlotCount; gateHouseIndex++)
        {
            if (gateHouseIndex < gateHouseLevel)
            {
                ConsoleTextComponent.Write("G", ConsoleColor.DarkYellow);
                continue;
            }

            if (showPreviewGateHouse && gateHouseIndex == gateHouseLevel)
            {
                ConsoleTextComponent.Write("G", ConsoleColor.Green);
                continue;
            }

            ConsoleTextComponent.Write("0", ConsoleColor.DarkYellow);
        }

        ConsoleTextComponent.NewLine();
    }
}
