using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Views.Components;

public class ResourcePanelComponent
{
    private const int PanelWidth = 38;
    private const int MinimumLeftColumnWidth = 44;
    private const int ContentWidth = PanelWidth - 4;

    public void Render(GameWorld gameWorld, GameCommandCost? selectedCommandCost, string selectedCommandLabel)
    {
        if (!TryGetPanelOrigin(out var startX))
        {
            return;
        }

        var top = 2;
        var rows = BuildRows(gameWorld, selectedCommandCost, selectedCommandLabel);
        var innerWidth = PanelWidth - 2;
        var currentRow = top;

        WriteAt(startX, currentRow++, $"+{new string('-', innerWidth)}+", ConsoleColor.DarkGray);
        WriteBorderedLine(startX, currentRow++, "Resources", ContentWidth, ConsoleColor.White);
        WriteAt(startX, currentRow++, $"| {new string('-', ContentWidth)} |", ConsoleColor.DarkGray);
        WriteBorderedLine(startX, currentRow++, "Resource    Stock             Prod", ContentWidth, ConsoleColor.DarkYellow);

        foreach (var row in rows)
        {
            WriteResourceRow(startX, currentRow++, row);
        }

        WriteAt(startX, currentRow++, $"| {new string('-', ContentWidth)} |", ConsoleColor.DarkGray);
        WriteStatRow(
            startX,
            currentRow++,
            "Army Cap",
            gameWorld.MaxArmySize.ToString(),
            "--",
            0,
            selectedCommandLabel == "Boost Army Size",
            selectedCommandLabel == "Boost Army Size");
        WriteAt(startX, currentRow, $"+{new string('-', innerWidth)}+", ConsoleColor.DarkGray);
    }

    private static bool TryGetPanelOrigin(out int startX)
    {
        startX = 0;

        try
        {
            var windowWidth = ConsoleTextComponent.WindowWidth;
            if (windowWidth < MinimumLeftColumnWidth + PanelWidth + 1)
            {
                return false;
            }

            startX = windowWidth - PanelWidth - 1;
            return true;
        }
        catch
        {
            return false;
        }
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

    private static void WriteResourceRow(int startX, int row, ResourcePanelRow resourceRow)
    {
        WriteStatRow(
            startX,
            row,
            resourceRow.Label,
            resourceRow.Amount.ToString(),
            resourceRow.ProductionDisplay,
            resourceRow.StockCost,
            resourceRow.ShowStockIncrease,
            resourceRow.ShowProductionIncrease);
    }

    private static void WriteStatRow(
        int startX,
        int row,
        string label,
        string value,
        string trailingValue,
        int stockCost,
        bool showStockIncrease,
        bool showProductionIncrease)
    {
        WriteAt(startX, row, "| ", ConsoleColor.DarkGray);
        WriteAt(startX + 2, row, $"{label,-11} ", ConsoleColor.Gray);

        var stockStartX = startX + 14;
        WriteAt(stockStartX, row, value.PadLeft(5), ConsoleColor.Gray);

        var currentX = stockStartX + 5;
        if (stockCost > 0)
        {
            var costText = $"[-{stockCost}]";
            WriteAt(currentX, row, costText, ConsoleColor.Red);
            currentX += costText.Length;
        }

        if (showStockIncrease)
        {
            WriteAt(currentX, row, "[+1]", ConsoleColor.Green);
        }

        var trailingStartX = startX + 25;
        var suffix = showProductionIncrease ? " [+1]" : string.Empty;
        var leadingText = trailingValue.PadLeft(ContentWidth - 23 - suffix.Length);
        WriteAt(trailingStartX, row, leadingText, ConsoleColor.Gray);

        if (showProductionIncrease)
        {
            WriteAt(trailingStartX + leadingText.Length, row, suffix.Replace(" ", string.Empty), ConsoleColor.Green);
        }

        var rightBorderX = startX + 2 + ContentWidth;
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

    private sealed record ResourcePanelRow(
        string Label,
        int Amount,
        string ProductionDisplay,
        int StockCost,
        bool ShowStockIncrease,
        bool ShowProductionIncrease);
}
