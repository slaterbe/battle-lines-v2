using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp;

public static class Program
{
    private static readonly TimeSpan TickRate = TimeSpan.FromSeconds(1);

    public static void Main()
    {
        var gameService = new GameService();
        var renderService = new RenderService();
        var gameWorld = gameService.CreateGameWorld();
        var nextTickAt = DateTime.UtcNow.Add(TickRate);
        var shouldExit = false;

        Console.CursorVisible = false;

        try
        {
            while (!shouldExit)
            {
                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    switch (key)
                    {
                        case ConsoleKey.P:
                            gameService.TogglePause(gameWorld);
                            break;
                        case ConsoleKey.Q:
                            shouldExit = true;
                            break;
                    }
                }

                var now = DateTime.UtcNow;
                if (now >= nextTickAt)
                {
                    gameService.Tick(gameWorld);
                    nextTickAt = now.Add(TickRate);
                }

                renderService.Render(gameWorld);
                Thread.Sleep(50);
            }
        }
        finally
        {
            Console.CursorVisible = true;
            Console.Clear();
        }
    }
}
