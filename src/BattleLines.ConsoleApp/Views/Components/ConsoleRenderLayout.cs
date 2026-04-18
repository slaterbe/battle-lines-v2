namespace BattleLines.ConsoleApp.Views.Components;

public static class ConsoleRenderLayout
{
    public const int MaxCharacterCount = 100;
    public const int MaxLineCount = 22;

    public static int ResolveLeft(int left, int width)
    {
        return ResolveCoordinate(left, width);
    }

    public static int ResolveTop(int top, int height)
    {
        return ResolveCoordinate(top, height);
    }

    private static int ResolveCoordinate(int value, int axisLength)
    {
        var effectiveAxisLength = Math.Max(1, axisLength);
        var resolvedValue = value < 0
            ? effectiveAxisLength + value
            : value;

        return Math.Clamp(resolvedValue, 0, effectiveAxisLength - 1);
    }
}
