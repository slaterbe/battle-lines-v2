namespace BattleLines.ConsoleApp.Views.Components;

public class GameTitleComponent
{
    public void Render()
    {
        ConsoleTextComponent.WriteLine("Battle Lines", ConsoleColor.White);
        ConsoleTextComponent.WriteLine(new string('=', 80));
    }
}
