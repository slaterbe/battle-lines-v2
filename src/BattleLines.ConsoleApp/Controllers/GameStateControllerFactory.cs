using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class GameStateControllerFactory
{
    private readonly IGameStateController introductionController;
    private readonly IGameStateController villageController;
    private readonly IGameStateController preBattleController;
    private readonly IGameStateController battleController;
    private readonly IGameStateController postWaveController;
    private readonly IGameStateController postBattleController;

    public GameStateControllerFactory(
        IGameStateController introductionController,
        IGameStateController villageController,
        IGameStateController preBattleController,
        IGameStateController battleController,
        IGameStateController postWaveController,
        IGameStateController postBattleController)
    {
        this.introductionController = introductionController;
        this.villageController = villageController;
        this.preBattleController = preBattleController;
        this.battleController = battleController;
        this.postWaveController = postWaveController;
        this.postBattleController = postBattleController;
    }

    public IGameStateController GetController(GameState gameState)
    {
        return gameState switch
        {
            GameState.Introduction => introductionController,
            GameState.Village => villageController,
            GameState.PreBattle => preBattleController,
            GameState.Battle => battleController,
            GameState.PostWave => postWaveController,
            GameState.PostBattle => postBattleController,
            _ => villageController
        };
    }
}
