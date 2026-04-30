namespace BattleLines.ConsoleApp.Views.ComponentsV2.Rendering;

public interface IDelayedContentComponent<in TState>
{
    int MeasureHeight(TState state);
    void Render(TState state, int startX, int startY);
}
