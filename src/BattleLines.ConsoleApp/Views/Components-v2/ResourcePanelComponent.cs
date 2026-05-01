using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public class ResourcePanelComponent
{
    private const int MinimumPanelWidth = 34;

    public void Render(
        GameWorld gameWorld,
        GameCommandCost? selectedCommandCost,
        GameCommandCost? selectedCommandSupply,
        GameCommandCost? selectedCommandIncome,
        string selectedCommandLabel,
        int startX,
        int startY,
        int panelWidth)
    {
        var resolvedStartX = ConsoleRenderLayout.ResolveLeft(startX, ConsoleTextComponent.WindowWidth);
        var resolvedStartY = ConsoleRenderLayout.ResolveTop(startY, ConsoleTextComponent.WindowHeight);
        var layout = new ResourcePanelLayout(
            resolvedStartX,
            resolvedStartY,
            Math.Max(MinimumPanelWidth, panelWidth));
        var rows = BuildRows(gameWorld, selectedCommandCost, selectedCommandSupply, selectedCommandIncome);
        var currentRow = layout.StartY;

        WriteAt(layout.StartX, currentRow++, $"+{new string('-', layout.InnerWidth)}+", ConsoleColor.DarkGray);
        WriteHeaderRow(layout.StartX, currentRow++, layout);

        foreach (var row in rows)
        {
            WriteResourceRow(layout.StartX, currentRow++, row, layout);
        }

        WriteCenteredDivider(layout.StartX, currentRow++, "Buildings", layout);
        WriteFarmBuildRow(layout.StartX, currentRow++, gameWorld, selectedCommandLabel, layout);
        if (gameWorld.IsUpgradesVisible)
        {
            WriteGateHouseBuildRow(layout.StartX, currentRow++, gameWorld, selectedCommandLabel, layout);
        }
        if (gameWorld.IsMilitiaYardVisible)
        {
            WriteMilitiaYardBuildRow(layout.StartX, currentRow++, gameWorld, selectedCommandLabel, layout);
        }
        if (gameWorld.IsSpearControlsVisible)
        {
            WritePoleturnerBuildRow(layout.StartX, currentRow++, gameWorld, selectedCommandLabel, layout);
        }
        WriteAt(layout.StartX, currentRow, $"+{new string('-', layout.InnerWidth)}+", ConsoleColor.DarkGray);
    }

    private static List<ResourcePanelRow> BuildRows(
        GameWorld gameWorld,
        GameCommandCost? selectedCommandCost,
        GameCommandCost? selectedCommandSupply,
        GameCommandCost? selectedCommandIncome)
    {
        var rows = new List<ResourcePanelRow>
        {
            new(
                "Food",
                gameWorld.Food,
                $"+{gameWorld.FoodProduction}",
                selectedCommandCost?.Food ?? 0,
                Math.Max(0, selectedCommandSupply?.Food ?? 0),
                Math.Max(0, selectedCommandIncome?.Food ?? 0)),
            new(
                "Villagers",
                gameWorld.Villagers,
                $"+{gameWorld.VillagerProduction}",
                selectedCommandCost?.Villagers ?? 0,
                Math.Max(0, selectedCommandSupply?.Villagers ?? 0),
                Math.Max(0, selectedCommandIncome?.Villagers ?? 0))
        };

        if (gameWorld.IsSpearControlsVisible)
        {
            rows.Add(
                new(
                    "Spears",
                    gameWorld.Spears,
                    $"+{gameWorld.SpearProduction}",
                    selectedCommandCost?.Spears ?? 0,
                    Math.Max(0, selectedCommandSupply?.Spears ?? 0),
                    Math.Max(0, selectedCommandIncome?.Spears ?? 0)));
        }

        rows.Add(
            new(
                "Gold",
                gameWorld.Gold,
                "--",
                selectedCommandCost?.Gold ?? 0,
                0,
                0));

        return rows;
    }

    private static void WriteResourceRow(int startX, int row, ResourcePanelRow resourceRow, ResourcePanelLayout layout)
    {
        WriteStatRow(
            startX,
            row,
            resourceRow.Label,
            resourceRow.Amount.ToString(),
            resourceRow.ProductionDisplay,
            resourceRow.StockCost,
            resourceRow.StockIncrease,
            resourceRow.ProductionIncrease,
            layout);
    }

    private static void WriteStatRow(
        int startX,
        int row,
        string label,
        string value,
        string trailingValue,
        int stockCost,
        int stockIncrease,
        int productionIncrease,
        ResourcePanelLayout layout)
    {
        WriteAt(startX, row, "| ", ConsoleColor.DarkGray);
        WriteAt(startX + 2, row, label.PadRight(layout.LabelWidth) + " ", ConsoleColor.Gray);

        var stockStartX = startX + 2 + layout.StockColumnStart;
        var stockText = value;
        if (stockCost > 0)
        {
            stockText += $"[-{stockCost}]";
        }

        if (stockIncrease > 0)
        {
            stockText += $"[+{stockIncrease}]";
        }

        var paddedStockText = stockText.PadLeft(layout.StockWidth);
        WriteAt(stockStartX, row, paddedStockText[..Math.Min(paddedStockText.Length, layout.StockWidth)], ConsoleColor.Gray);

        if (stockCost > 0)
        {
            var costText = $"[-{stockCost}]";
            var stockIncreaseText = stockIncrease > 0 ? $"[+{stockIncrease}]" : string.Empty;
            WriteAt(stockStartX + paddedStockText.Length - costText.Length - stockIncreaseText.Length, row, costText, ConsoleColor.Red);
        }

        if (stockIncrease > 0)
        {
            var stockIncreaseText = $"[+{stockIncrease}]";
            WriteAt(stockStartX + paddedStockText.Length - stockIncreaseText.Length, row, stockIncreaseText, ConsoleColor.Green);
        }

        var trailingStartX = startX + 2 + layout.ProductionColumnStart;
        var suffix = productionIncrease > 0 ? $"[+{productionIncrease}]" : string.Empty;
        var productionText = trailingValue + suffix;
        var paddedProductionText = productionText.PadLeft(layout.ProductionWidth);
        WriteAt(trailingStartX, row, paddedProductionText[..Math.Min(paddedProductionText.Length, layout.ProductionWidth)], ConsoleColor.Gray);

        if (productionIncrease > 0)
        {
            WriteAt(trailingStartX + paddedProductionText.Length - suffix.Length, row, suffix, ConsoleColor.Green);
        }

        var rightBorderX = startX + 2 + layout.ContentWidth;
        WriteAt(rightBorderX, row, " |", ConsoleColor.DarkGray);
    }

    private static void WriteHeaderRow(int startX, int row, ResourcePanelLayout layout)
    {
        var stockGapWidth = layout.ProductionColumnStart - layout.StockColumnStart - layout.StockWidth;
        var content =
            "Resource".PadRight(layout.LabelWidth) +
            " " +
            "Supply".PadLeft(layout.StockWidth) +
            " ".PadRight(Math.Max(1, stockGapWidth)) +
            "Income".PadLeft(layout.ProductionWidth);
        WriteBorderedLine(startX, row, content, layout.ContentWidth, ConsoleColor.DarkYellow);
    }

    private static void WriteCenteredDivider(int startX, int row, string label, ResourcePanelLayout layout)
    {
        var centeredLabel = $" {label} ";
        var availableWidth = layout.ContentWidth;
        var leftDashCount = Math.Max(0, (availableWidth - centeredLabel.Length) / 2);
        var rightDashCount = Math.Max(0, availableWidth - centeredLabel.Length - leftDashCount);
        var content = new string('-', leftDashCount) + centeredLabel + new string('-', rightDashCount);
        WriteBorderedLine(startX, row, content, availableWidth, ConsoleColor.DarkYellow);
    }

    private static void WriteFarmBuildRow(
        int startX,
        int row,
        GameWorld gameWorld,
        string selectedCommandLabel,
        ResourcePanelLayout layout)
    {
        const int minimumFarmSlots = 8;
        const int foodProductionPerFarm = 2;

        var farmCount = Math.Max(0, gameWorld.FoodProduction / foodProductionPerFarm);
        var showPreviewFarm = selectedCommandLabel == "Expand Farm" && farmCount < minimumFarmSlots;
        var slotCount = Math.Max(minimumFarmSlots, farmCount + (showPreviewFarm ? 1 : 0));
        var builtFarms = new string('F', farmCount);
        var previewFarm = showPreviewFarm ? "F" : string.Empty;
        var emptyFarms = new string('0', Math.Max(0, slotCount - farmCount - (showPreviewFarm ? 1 : 0)));
        WriteBuildRow(startX, row, "Farm", builtFarms, previewFarm, emptyFarms, layout);
    }

    private static void WritePoleturnerBuildRow(
        int startX,
        int row,
        GameWorld gameWorld,
        string selectedCommandLabel,
        ResourcePanelLayout layout)
    {
        const int minimumPoleturnerSlots = 8;
        var poleturnerCount = Math.Max(0, gameWorld.SpearProduction);
        var showPreviewPoleturner = selectedCommandLabel == "Spear Maker" && poleturnerCount < minimumPoleturnerSlots;
        var slotCount = Math.Max(minimumPoleturnerSlots, poleturnerCount + (showPreviewPoleturner ? 1 : 0));
        var builtPoleturners = new string('P', poleturnerCount);
        var previewPoleturner = showPreviewPoleturner ? "P" : string.Empty;
        var emptyPoleturners = new string('0', Math.Max(0, slotCount - poleturnerCount - (showPreviewPoleturner ? 1 : 0)));
        WriteBuildRow(startX, row, "Spear Mkr", builtPoleturners, previewPoleturner, emptyPoleturners, layout);
    }

    private static void WriteGateHouseBuildRow(
        int startX,
        int row,
        GameWorld gameWorld,
        string selectedCommandLabel,
        ResourcePanelLayout layout)
    {
        const int maximumGateHouseSlots = GameWorld.MaxGateHouseLevel;

        var gateHouseLevel = gameWorld.GateHouseLevel;
        var showPreviewGateHouse = selectedCommandLabel == "Upgrade GateHouse" && gateHouseLevel < maximumGateHouseSlots;
        var slotCount = Math.Max(maximumGateHouseSlots, gateHouseLevel + (showPreviewGateHouse ? 1 : 0));
        var builtGateHouse = new string('G', gateHouseLevel);
        var previewGateHouse = showPreviewGateHouse ? "G" : string.Empty;
        var emptyGateHouse = new string('0', Math.Max(0, slotCount - gateHouseLevel - (showPreviewGateHouse ? 1 : 0)));
        WriteBuildRow(startX, row, "GateHouse", builtGateHouse, previewGateHouse, emptyGateHouse, layout);
    }

    private static void WriteMilitiaYardBuildRow(
        int startX,
        int row,
        GameWorld gameWorld,
        string selectedCommandLabel,
        ResourcePanelLayout layout)
    {
        const int maximumMilitiaYardSlots = GameWorld.MaxMilitiaYardLevel;

        var militiaYardLevel = gameWorld.MilitiaYardLevel;
        var showPreviewMilitiaYard = selectedCommandLabel == "Upgrade Militia Yard" && militiaYardLevel < maximumMilitiaYardSlots;
        var slotCount = Math.Max(maximumMilitiaYardSlots, militiaYardLevel + (showPreviewMilitiaYard ? 1 : 0));
        var builtMilitiaYard = new string('M', militiaYardLevel);
        var previewMilitiaYard = showPreviewMilitiaYard ? "M" : string.Empty;
        var emptyMilitiaYard = new string('0', Math.Max(0, slotCount - militiaYardLevel - (showPreviewMilitiaYard ? 1 : 0)));
        WriteBuildRow(startX, row, "MilitiaYd", builtMilitiaYard, previewMilitiaYard, emptyMilitiaYard, layout);
    }

    private static void WriteBuildRow(
        int startX,
        int row,
        string label,
        string builtDisplay,
        string previewDisplay,
        string emptyDisplay,
        ResourcePanelLayout layout)
    {
        var totalDisplay = builtDisplay + previewDisplay + emptyDisplay;

        WriteAt(startX, row, "| ", ConsoleColor.DarkGray);
        WriteAt(startX + 2, row, label.PadRight(layout.LabelWidth) + " ", ConsoleColor.White);

        var stockStartX = startX + 2 + layout.StockColumnStart;
        var paddedDisplay = totalDisplay.PadLeft(layout.StockWidth);
        WriteAt(stockStartX, row, paddedDisplay[..Math.Min(paddedDisplay.Length, layout.StockWidth)], ConsoleColor.White);

        var nextLeft = stockStartX + Math.Max(0, paddedDisplay.Length - totalDisplay.Length);
        WriteAt(nextLeft, row, builtDisplay, ConsoleColor.White);
        nextLeft += builtDisplay.Length;
        if (!string.IsNullOrEmpty(previewDisplay))
        {
            WriteAt(nextLeft, row, previewDisplay, ConsoleColor.Green);
            nextLeft += previewDisplay.Length;
        }

        WriteAt(nextLeft, row, emptyDisplay, ConsoleColor.White);
        var rightBorderX = startX + 2 + layout.ContentWidth;
        WriteAt(rightBorderX, row, " |", ConsoleColor.DarkGray);
    }

    private static void WriteBorderedLine(int startX, int row, string content, int contentWidth, ConsoleColor color)
    {
        var paddedContent = content.Length > contentWidth
            ? content[..contentWidth]
            : content.PadRight(contentWidth);
        WriteAt(startX, row, "| ", ConsoleColor.DarkGray);
        WriteAt(startX + 2, row, paddedContent, color);
        WriteAt(startX + 2 + contentWidth, row, " |", ConsoleColor.DarkGray);
    }

    private static void WriteAt(int left, int top, string text, ConsoleColor color)
    {
        ConsoleTextComponent.SetCursorPosition(left, top);
        ConsoleTextComponent.Write(text, color);
    }

    private readonly record struct ResourcePanelLayout(int StartX, int StartY, int PanelWidth)
    {
        public int InnerWidth => PanelWidth - 2;
        public int ContentWidth => PanelWidth - 4;
        public int LabelWidth => 11;
        public int ColumnGap => 3;
        public int AvailableDataWidth => Math.Max(12, ContentWidth - LabelWidth - 1 - ColumnGap);
        public int StockWidth => AvailableDataWidth / 2;
        public int StockColumnStart => LabelWidth + 1;
        public int ProductionWidth => AvailableDataWidth - StockWidth;
        public int ProductionColumnStart => StockColumnStart + StockWidth + ColumnGap;
    }

    private sealed record ResourcePanelRow(
        string Label,
        int Amount,
        string ProductionDisplay,
        int StockCost,
        int StockIncrease,
        int ProductionIncrease);
}
