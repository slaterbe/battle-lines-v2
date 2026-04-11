using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Debug;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Services;

public class RenderService
{
    private readonly IReadOnlyDictionary<GameState, IGameView> views;
    private readonly DebugPanelComponent debugPanelComponent;
    private readonly RenderDiagnostics renderDiagnostics;

    public RenderService(RenderDiagnostics renderDiagnostics)
    {
        this.renderDiagnostics = renderDiagnostics;
        debugPanelComponent = new DebugPanelComponent(renderDiagnostics);

        views = new Dictionary<GameState, IGameView>
        {
            [GameState.Introduction] = new IntroductionView(),
            [GameState.Village] = new VillageView(),
            [GameState.PreBattle] = new PreWaveView(),
            [GameState.Battle] = new WaveView(),
            [GameState.PostWave] = new PostWaveView(),
            [GameState.PostBattle] = new PostBattleView()
        };
    }

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        if (!views.TryGetValue(gameWorld.State, out var view))
        {
            view = views[GameState.Village];
        }

        renderDiagnostics.RecordRenderAttempt();
        ConsoleTextComponent.BeginFrame();
        view.Render(gameWorld, commandOptions, selectedCommandIndex);
        debugPanelComponent.Render(gameWorld);
        var writtenCharacterCount = ConsoleTextComponent.FlushFrame();
        renderDiagnostics.RecordTerminalWrite(writtenCharacterCount);
    }
}
