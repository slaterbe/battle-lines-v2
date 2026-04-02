namespace BattleLines.ConsoleApp.Models;

public class GameWorld
{
    public int Commoners { get; set; }

    public int Spears { get; set; }

    public int CommonerProduction { get; set; }

    public int SpearProduction { get; set; }

    public bool IsPaused { get; set; } = true;
}
