using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Services;

public class GameService
{
    private readonly EnemyWaveFactory enemyWaveFactory = new();

    public GameWorld CreateGameWorld()
    {
        return new GameWorld
        {
            Commoners = 10,
            Spears = 5,
            CommonerProduction = 2,
            SpearProduction = 1,
            IsPaused = true,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.SpearmenLvl1] = 1
            },
            EnemyWaveList = enemyWaveFactory.CreateGiantRatWaves(5)
        };
    }

    public void Tick(GameWorld gameWorld)
    {
        if (gameWorld.IsPaused)
        {
            return;
        }

        gameWorld.Commoners += gameWorld.CommonerProduction;
        gameWorld.Spears += gameWorld.SpearProduction;
    }

    public void TogglePause(GameWorld gameWorld)
    {
        gameWorld.IsPaused = !gameWorld.IsPaused;
    }
}
