using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views.ComponentsV2.Rendering;

public sealed class DelayedTextRender<TComponent> where TComponent : ITextContentComponent
{
    private readonly TComponent inner;
    private readonly int charactersPerSecond;
    private readonly ConsoleColor color;
    private DateTime? animationStartedAtUtc;
    private bool isFullyRendered;

    public DelayedTextRender(TComponent inner, int charactersPerSecond, ConsoleColor color = ConsoleColor.Gray)
    {
        this.inner = inner;
        this.charactersPerSecond = Math.Max(1, charactersPerSecond);
        this.color = color;
    }

    public void Render(GameWorld gameWorld, int startX, int startY)
    {
        ConsoleTextComponent.SetCursorPosition(startX, startY);

        var lines = inner.GetLines(gameWorld);
        if (isFullyRendered)
        {
            RenderAllLines(lines, startX);
            return;
        }

        animationStartedAtUtc ??= DateTime.UtcNow;
        var revealedCharacterCount = GetRevealedCharacterCount(animationStartedAtUtc.Value);
        var renderedAllLines = RenderVisibleLines(lines, revealedCharacterCount, startX);
        isFullyRendered = renderedAllLines;
    }

    private int GetRevealedCharacterCount(DateTime animationStartedAt)
    {
        var elapsedMilliseconds = Math.Max(0, (DateTime.UtcNow - animationStartedAt).TotalMilliseconds);
        return (int)(elapsedMilliseconds * charactersPerSecond / 1000d);
    }

    private bool RenderVisibleLines(IReadOnlyList<string> lines, int revealedCharacterCount, int startX)
    {
        var remainingCharacters = revealedCharacterCount;
        var maxWidth = Math.Max(1, ConsoleTextComponent.WindowWidth - startX);

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                if (remainingCharacters <= 0)
                {
                    return false;
                }

                ConsoleTextComponent.NewLine();
                remainingCharacters--;
                continue;
            }

            if (remainingCharacters <= 0)
            {
                return false;
            }

            var visibleLength = Math.Min(line.Length, remainingCharacters);
            ConsoleTextComponent.WriteWrappedLines(line[..visibleLength], maxWidth, color);

            if (visibleLength < line.Length)
            {
                return false;
            }

            remainingCharacters -= line.Length;
        }

        return true;
    }

    private void RenderAllLines(IReadOnlyList<string> lines, int startX)
    {
        var maxWidth = Math.Max(1, ConsoleTextComponent.WindowWidth - startX);
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                ConsoleTextComponent.NewLine();
                continue;
            }

            ConsoleTextComponent.WriteWrappedLines(line, maxWidth, color);
        }
    }
}
