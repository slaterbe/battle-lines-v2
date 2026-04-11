namespace BattleLines.ConsoleApp.Debug;

public sealed class RenderDiagnostics
{
    private static readonly TimeSpan SnapshotWindow = TimeSpan.FromSeconds(1);

    private DateTime bucketStartedAt = DateTime.UtcNow;
    private int renderAttemptsInBucket;
    private int terminalWritesInBucket;
    private int terminalCharactersInBucket;
    private RenderDiagnosticsSnapshot lastSnapshot = new(0, 0, 0);

    public void RecordRenderAttempt()
    {
        AdvanceWindowIfNeeded();
        renderAttemptsInBucket++;
    }

    public void RecordTerminalWrite(int characterCount)
    {
        AdvanceWindowIfNeeded();

        if (characterCount <= 0)
        {
            return;
        }

        terminalWritesInBucket++;
        terminalCharactersInBucket += characterCount;
    }

    public RenderDiagnosticsSnapshot GetSnapshot()
    {
        AdvanceWindowIfNeeded();
        return lastSnapshot;
    }

    private void AdvanceWindowIfNeeded()
    {
        var now = DateTime.UtcNow;
        if (now - bucketStartedAt < SnapshotWindow)
        {
            return;
        }

        lastSnapshot = new RenderDiagnosticsSnapshot(
            renderAttemptsInBucket,
            terminalWritesInBucket,
            terminalCharactersInBucket);

        bucketStartedAt = now;
        renderAttemptsInBucket = 0;
        terminalWritesInBucket = 0;
        terminalCharactersInBucket = 0;
    }
}

public readonly record struct RenderDiagnosticsSnapshot(
    int RenderAttemptsPerSecond,
    int TerminalWritesPerSecond,
    int TerminalCharactersPerSecond);
