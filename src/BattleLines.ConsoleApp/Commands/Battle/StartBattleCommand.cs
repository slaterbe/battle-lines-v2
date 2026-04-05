using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Commands;

public class StartBattleCommand : IGameCommand
{
    public GameCommandCategory Category => GameCommandCategory.Battle;
    public string Label => "Start Battle";
    public string HelpText => "Move to battle prep for the next enemy wave.";

    public bool Execute(GameWorld gameWorld)
    {
        if (gameWorld.State == GameState.Battle ||
            gameWorld.State == GameState.PostWave ||
            gameWorld.State == GameState.PostBattle ||
            gameWorld.EnemyWaves.Waves.Count == 0)
        {
            return false;
        }

        gameWorld.WavePosition = GetCurrentWavePosition(gameWorld);
        gameWorld.State = GameState.PreBattle;
        return false;
    }

    private static int GetCurrentWavePosition(GameWorld gameWorld)
    {
        var remainingWaveCount = gameWorld.EnemyWaves.Waves.Count;
        if (remainingWaveCount <= 0)
        {
            return 0;
        }

        var defeatedWaveCount = Math.Max(0, gameWorld.TotalWaveCount - remainingWaveCount);
        return defeatedWaveCount + 1;
    }
}
