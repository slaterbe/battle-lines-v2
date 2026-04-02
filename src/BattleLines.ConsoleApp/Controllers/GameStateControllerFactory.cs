using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Controllers;

public class GameStateControllerFactory
{
    private readonly IGameStateController villageController;
    private readonly IGameStateController preBattleController;
    private readonly IGameStateController battleController;
    private readonly IGameStateController postBattleController;

    public GameStateControllerFactory(
        IGameStateController villageController,
        IGameStateController preBattleController,
        IGameStateController battleController,
        IGameStateController postBattleController)
    {
        this.villageController = villageController;
        this.preBattleController = preBattleController;
        this.battleController = battleController;
        this.postBattleController = postBattleController;
    }

    public IGameStateController GetController(GameState gameState)
    {
        return gameState switch
        {
            GameState.Village => villageController,
            GameState.PreBattle => preBattleController,
            GameState.Battle => battleController,
            GameState.PostBattle => postBattleController,
            _ => villageController
        };
    }
}
