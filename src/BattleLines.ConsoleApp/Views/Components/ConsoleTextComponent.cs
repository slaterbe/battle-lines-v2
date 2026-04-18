namespace BattleLines.ConsoleApp.Views.Components;

public static class ConsoleTextComponent
{
    private static ConsoleFrameBuffer? activeFrameBuffer;
    private static ConsoleFrameBuffer? lastFrameBuffer;

    public static int WindowWidth => activeFrameBuffer?.Width ?? Math.Min(Console.WindowWidth, ConsoleRenderLayout.MaxCharacterCount);
    public static int WindowHeight => activeFrameBuffer?.Height ?? Math.Min(Console.WindowHeight, ConsoleRenderLayout.MaxLineCount);

    public static int CursorTop => activeFrameBuffer?.CursorTop ?? Console.CursorTop;

    public static void BeginFrame()
    {
        activeFrameBuffer = new ConsoleFrameBuffer(
            Math.Min(Console.WindowWidth, ConsoleRenderLayout.MaxCharacterCount),
            Math.Min(Console.WindowHeight, ConsoleRenderLayout.MaxLineCount));
    }

    public static int FlushFrame()
    {
        if (activeFrameBuffer is null)
        {
            return 0;
        }

        var frameDiff = activeFrameBuffer.ToAnsiDiff(lastFrameBuffer);
        if (!string.IsNullOrEmpty(frameDiff))
        {
            Console.Write(frameDiff);
        }

        lastFrameBuffer = activeFrameBuffer;
        activeFrameBuffer = null;
        return frameDiff.Length;
    }

    public static void RestoreConsoleAfterExit()
    {
        if (lastFrameBuffer is null)
        {
            Console.ResetColor();
            return;
        }

        var exitRow = Math.Min(Console.WindowHeight, lastFrameBuffer.LastWrittenRow + 2);
        Console.Write($"\u001b[0m\u001b[{exitRow};1H");
    }

    public static void SetCursorPosition(int left, int top)
    {
        var resolvedLeft = ConsoleRenderLayout.ResolveLeft(left, WindowWidth);
        var resolvedTop = ConsoleRenderLayout.ResolveTop(top, WindowHeight);

        if (activeFrameBuffer is not null)
        {
            activeFrameBuffer.SetCursorPosition(resolvedLeft, resolvedTop);
            return;
        }

        Console.SetCursorPosition(resolvedLeft, resolvedTop);
    }

    public static void NewLine()
    {
        WriteLine(string.Empty);
    }

    public static void Write(string text, ConsoleColor color = ConsoleColor.Gray)
    {
        if (activeFrameBuffer is not null)
        {
            activeFrameBuffer.Write(text, color);
            return;
        }

        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }

    public static void WriteLine(string text, ConsoleColor color = ConsoleColor.Gray)
    {
        if (activeFrameBuffer is not null)
        {
            activeFrameBuffer.WriteLine(text, color);
            return;
        }

        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = originalColor;
    }

    public static void WriteWrappedLines(string text, int maxWidth, ConsoleColor color = ConsoleColor.Gray)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            WriteLine(string.Empty, color);
            return;
        }

        var effectiveWidth = Math.Max(1, maxWidth);
        foreach (var rawLine in text.Replace("\r", string.Empty).Split('\n'))
        {
            var remaining = rawLine.Trim();
            if (remaining.Length == 0)
            {
                WriteLine(string.Empty, color);
                continue;
            }

            while (remaining.Length > effectiveWidth)
            {
                var splitIndex = remaining.LastIndexOf(' ', effectiveWidth);
                if (splitIndex <= 0)
                {
                    splitIndex = effectiveWidth;
                }

                WriteLine(remaining[..splitIndex].TrimEnd(), color);
                remaining = remaining[splitIndex..].TrimStart();
            }

            WriteLine(remaining, color);
        }
    }

    public static void WriteHighlighted(
        string text,
        ConsoleColor foregroundColor = ConsoleColor.Black,
        ConsoleColor backgroundColor = ConsoleColor.Yellow)
    {
        var effectiveColor = GetAccentColor(foregroundColor, backgroundColor);

        if (activeFrameBuffer is not null)
        {
            activeFrameBuffer.Write(text, effectiveColor);
            return;
        }

        var originalForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = effectiveColor;
        Console.Write(text);
        Console.ForegroundColor = originalForegroundColor;
    }

    public static void WriteLineSlow(string text, ConsoleColor color = ConsoleColor.Gray, int characterDelayMs = 20)
    {
        if (activeFrameBuffer is not null)
        {
            WriteLine(text, color);
            return;
        }

        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        foreach (var character in text)
        {
            Console.Write(character);
            Thread.Sleep(characterDelayMs);
        }

        Console.WriteLine();
        Console.ForegroundColor = originalColor;
    }

    private static ConsoleColor GetAccentColor(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        return foregroundColor == ConsoleColor.Black
            ? backgroundColor
            : foregroundColor;
    }
}
