using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views;

namespace BattleLines.ConsoleApp.Services;

public class RenderService
{
    private readonly IReadOnlyDictionary<GameState, IGameView> views;

    public RenderService()
    {
        views = new Dictionary<GameState, IGameView>
        {
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

        view.Render(gameWorld, commandOptions, selectedCommandIndex);
    }
}
