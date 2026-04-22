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
        string selectedCommandLabel,
        int startX,
        int startY)
    {
        var resolvedStartX = ConsoleRenderLayout.ResolveLeft(startX, ConsoleTextComponent.WindowWidth);
        var resolvedStartY = ConsoleRenderLayout.ResolveTop(startY, ConsoleTextComponent.WindowHeight);
        var panelWidth = Math.Max(MinimumPanelWidth, ConsoleTextComponent.WindowWidth - resolvedStartX - 1);
        var layout = new ResourcePanelLayout(resolvedStartX, resolvedStartY, panelWidth);
        var rows = BuildRows(gameWorld, selectedCommandCost, selectedCommandLabel);
        var currentRow = layout.StartY;

        WriteAt(layout.StartX, currentRow++, $"+{new string('-', layout.InnerWidth)}+", ConsoleColor.DarkGray);
        WriteHeaderRow(layout.StartX, currentRow++, layout);

        foreach (var row in rows)
        {
            WriteResourceRow(layout.StartX, currentRow++, row, layout);
        }

        WriteAt(layout.StartX, currentRow++, $"| {new string('-', layout.ContentWidth)} |", ConsoleColor.DarkGray);
        WriteStatRow(
            layout.StartX,
            currentRow++,
            "Battle Line",
            gameWorld.FrontLineCapacity.ToString(),
            "--",
            0,
            selectedCommandLabel == "Expand Battle Line",
            selectedCommandLabel == "Expand Battle Line",
            layout);
        WriteAt(layout.StartX, currentRow, $"+{new string('-', layout.InnerWidth)}+", ConsoleColor.DarkGray);
    }

    private static List<ResourcePanelRow> BuildRows(
        GameWorld gameWorld,
        GameCommandCost? selectedCommandCost,
        string selectedCommandLabel)
    {
        var rows = new List<ResourcePanelRow>
        {
            new(
                "Villagers",
                gameWorld.Villagers,
                $"+{gameWorld.VillagerProduction}",
                selectedCommandCost?.Villagers ?? 0,
                selectedCommandLabel == "Boost Villagers",
                selectedCommandLabel == "Boost Villagers")
        };

        if (gameWorld.IsSpearControlsVisible)
        {
            rows.Add(
                new(
                    "Spears",
                    gameWorld.Spears,
                    $"+{gameWorld.SpearProduction}",
                    selectedCommandCost?.Spears ?? 0,
                    selectedCommandLabel == "Boost Spears",
                    selectedCommandLabel == "Boost Spears"));
        }

        if (gameWorld.IsUpgradesVisible)
        {
            rows.Add(
                new(
                    "Gold",
                    gameWorld.Gold,
                    "--",
                    selectedCommandCost?.Gold ?? 0,
                    false,
                    false));
        }

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
            resourceRow.ShowStockIncrease,
            resourceRow.ShowProductionIncrease,
            layout);
    }

    private static void WriteStatRow(
        int startX,
        int row,
        string label,
        string value,
        string trailingValue,
        int stockCost,
        bool showStockIncrease,
        bool showProductionIncrease,
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

        if (showStockIncrease)
        {
            stockText += "[+1]";
        }

        var paddedStockText = stockText.PadLeft(layout.StockWidth);
        WriteAt(stockStartX, row, paddedStockText[..Math.Min(paddedStockText.Length, layout.StockWidth)], ConsoleColor.Gray);

        if (stockCost > 0)
        {
            var costText = $"[-{stockCost}]";
            WriteAt(stockStartX + paddedStockText.Length - costText.Length - (showStockIncrease ? 4 : 0), row, costText, ConsoleColor.Red);
        }

        if (showStockIncrease)
        {
            WriteAt(stockStartX + paddedStockText.Length - 4, row, "[+1]", ConsoleColor.Green);
        }

        var trailingStartX = startX + 2 + layout.ProductionColumnStart;
        var suffix = showProductionIncrease ? "[+1]" : string.Empty;
        var productionText = trailingValue + suffix;
        var paddedProductionText = productionText.PadLeft(layout.ProductionWidth);
        WriteAt(trailingStartX, row, paddedProductionText[..Math.Min(paddedProductionText.Length, layout.ProductionWidth)], ConsoleColor.Gray);

        if (showProductionIncrease)
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
        bool ShowStockIncrease,
        bool ShowProductionIncrease);
}
