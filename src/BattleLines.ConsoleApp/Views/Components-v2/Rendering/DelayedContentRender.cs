namespace BattleLines.ConsoleApp.Views.ComponentsV2.Rendering;

public sealed class DelayedContentRender<TComponent, TState>
    where TComponent : IDelayedContentComponent<TState>
{
    private readonly TComponent inner;
    private readonly TimeSpan delay;
    private DateTime? animationStartedAtUtc;
    private bool isVisible;

    public DelayedContentRender(TComponent inner, TimeSpan delay)
    {
        this.inner = inner;
        this.delay = delay < TimeSpan.Zero ? TimeSpan.Zero : delay;
    }

    public int MeasureHeight(TState state)
    {
        return inner.MeasureHeight(state);
    }

    public void Render(TState state, int startX, int startY)
    {
        if (isVisible)
        {
            inner.Render(state, startX, startY);
            return;
        }

        animationStartedAtUtc ??= DateTime.UtcNow;
        if ((DateTime.UtcNow - animationStartedAtUtc.Value) < delay)
        {
            return;
        }

        isVisible = true;
        inner.Render(state, startX, startY);
    }
}
