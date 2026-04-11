using System.Text;

namespace BattleLines.ConsoleApp.Views.Components;

public sealed class ConsoleFrameBuffer
{
    private readonly FrameCell[,] cells;
    private readonly int width;
    private readonly int height;
    private int cursorLeft;
    private int cursorTop;
    private int maxRowWritten;

    public ConsoleFrameBuffer(int width, int height)
    {
        this.width = Math.Max(1, width);
        this.height = Math.Max(1, height);
        cells = new FrameCell[this.height, this.width];

        for (var row = 0; row < this.height; row++)
        {
            for (var column = 0; column < this.width; column++)
            {
                cells[row, column] = new FrameCell(' ', ConsoleColor.Gray);
            }
        }
    }

    public int Width => width;
    public int Height => height;

    public int CursorTop => cursorTop;

    public int LastWrittenRow => GetLastVisibleRow();

    public void SetCursorPosition(int left, int top)
    {
        cursorLeft = Math.Clamp(left, 0, width - 1);
        cursorTop = Math.Clamp(top, 0, height - 1);
    }

    public void Write(string text, ConsoleColor foregroundColor)
    {
        foreach (var character in text)
        {
            if (character == '\r')
            {
                continue;
            }

            if (character == '\n')
            {
                MoveToNextLine();
                continue;
            }

            if (cursorTop >= height)
            {
                return;
            }

            cells[cursorTop, cursorLeft] = new FrameCell(character, foregroundColor);
            maxRowWritten = Math.Max(maxRowWritten, cursorTop);

            cursorLeft++;
            if (cursorLeft >= width)
            {
                MoveToNextLine();
            }
        }
    }

    public void WriteLine(string text, ConsoleColor foregroundColor)
    {
        Write(text, foregroundColor);
        MoveToNextLine();
    }

    public string ToAnsiFrame()
    {
        var builder = new StringBuilder();
        builder.Append("\u001b[?25l");
        builder.Append("\u001b[2J");
        builder.Append("\u001b[H");

        ConsoleColor? currentForeground = null;
        var lastRow = GetLastVisibleRow();

        for (var row = 0; row <= lastRow; row++)
        {
            for (var column = 0; column < width; column++)
            {
                var cell = cells[row, column];
                if (currentForeground != cell.ForegroundColor)
                {
                    builder.Append(GetAnsiColorSequence(cell.ForegroundColor));
                    currentForeground = cell.ForegroundColor;
                }

                builder.Append(cell.Character);
            }

            builder.Append("\u001b[0m");
            currentForeground = null;

            if (row < lastRow)
            {
                builder.AppendLine();
            }
        }

        builder.Append("\u001b[0m");
        return builder.ToString();
    }

    public string ToAnsiDiff(ConsoleFrameBuffer? previousFrame)
    {
        if (previousFrame is null || previousFrame.width != width || previousFrame.height != height)
        {
            return ToAnsiFrame();
        }

        var builder = new StringBuilder();
        var lastRow = Math.Max(GetLastVisibleRow(), previousFrame.GetLastVisibleRow());
        ConsoleColor? currentForeground = null;
        var hasChanges = false;

        for (var row = 0; row <= Math.Min(height - 1, lastRow); row++)
        {
            var column = 0;
            while (column < width)
            {
                if (cells[row, column].Equals(previousFrame.cells[row, column]))
                {
                    column++;
                    continue;
                }

                builder.Append($"\u001b[{row + 1};{column + 1}H");
                hasChanges = true;

                while (column < width && !cells[row, column].Equals(previousFrame.cells[row, column]))
                {
                    var cell = cells[row, column];
                    if (currentForeground != cell.ForegroundColor)
                    {
                        builder.Append(GetAnsiColorSequence(cell.ForegroundColor));
                        currentForeground = cell.ForegroundColor;
                    }

                    builder.Append(cell.Character);
                    column++;
                }
            }
        }

        if (!hasChanges)
        {
            return string.Empty;
        }

        builder.Insert(0, "\u001b[?25l");
        builder.Append("\u001b[0m");
        return builder.ToString();
    }

    private void MoveToNextLine()
    {
        cursorLeft = 0;
        if (cursorTop < height - 1)
        {
            cursorTop++;
            maxRowWritten = Math.Max(maxRowWritten, cursorTop);
        }
    }

    private static string GetAnsiColorSequence(ConsoleColor foregroundColor)
    {
        return $"\u001b[{ToAnsiForegroundCode(foregroundColor)}m";
    }

    private static int ToAnsiForegroundCode(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => 30,
            ConsoleColor.DarkRed => 31,
            ConsoleColor.DarkGreen => 32,
            ConsoleColor.DarkYellow => 33,
            ConsoleColor.DarkBlue => 34,
            ConsoleColor.DarkMagenta => 35,
            ConsoleColor.DarkCyan => 36,
            ConsoleColor.Gray => 37,
            ConsoleColor.DarkGray => 90,
            ConsoleColor.Red => 91,
            ConsoleColor.Green => 92,
            ConsoleColor.Yellow => 93,
            ConsoleColor.Blue => 94,
            ConsoleColor.Magenta => 95,
            ConsoleColor.Cyan => 96,
            ConsoleColor.White => 97,
            _ => 37
        };
    }

    private int GetLastVisibleRow()
    {
        for (var row = height - 1; row >= 0; row--)
        {
            for (var column = 0; column < width; column++)
            {
                if (cells[row, column].Character != ' ')
                {
                    return row;
                }
            }
        }

        return 0;
    }

    private readonly record struct FrameCell(char Character, ConsoleColor ForegroundColor);
}
