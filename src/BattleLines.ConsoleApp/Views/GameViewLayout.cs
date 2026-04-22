using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public static class GameViewLayout
{
    public const int LeftColumnStartX = 0;
    public const int RightColumnStartX = 65;

    public const int HeaderStartY = 1;
    public const int HeaderWidth = 64;

    public const int ResourcePanelStartY = 1;
    public const int ResourcePanelWidth = 35;

    public const int VillageUnitsStartY = 6;
    public const int WaveBattlefieldStartY = 5;
    public const int PostBattleSummaryStartY = 5;

    public static int GetBottomAnchoredStartY(int componentHeight)
    {
        return Math.Max(0, ConsoleRenderLayout.MaxLineCount - Math.Max(1, componentHeight));
    }
}
