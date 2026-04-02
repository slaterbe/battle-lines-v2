using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp;

public static class Program
{
    private static readonly TimeSpan TickRate = TimeSpan.FromSeconds(1);

    public static void Main()
    {
        var battleService = new BattleService();
        var postBattleService = new PostBattleService();
        var preparationService = new PreparationService();
        var gameWorldFactory = new GameWorldFactory();
        var renderService = new RenderService();
        var gameWorld = gameWorldFactory.Create();
        var nextTickAt = DateTime.UtcNow.Add(TickRate);
        var shouldExit = false;
        var selectedCommandIndex = 0;

        Console.CursorVisible = false;

        try
        {
            while (!shouldExit)
            {
                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    var availableCommands = GetCommandOptions(gameWorld);

                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.LeftArrow:
                            selectedCommandIndex = GetPreviousCommandIndex(selectedCommandIndex, availableCommands.Count);
                            break;
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.RightArrow:
                            selectedCommandIndex = GetNextCommandIndex(selectedCommandIndex, availableCommands.Count);
                            break;
                        case ConsoleKey.Enter:
                            shouldExit = ExecuteSelectedCommand(preparationService, battleService, postBattleService, gameWorld, selectedCommandIndex);
                            break;
                    }
                }

                var now = DateTime.UtcNow;
                if (now >= nextTickAt)
                {
                    TickGameState(preparationService, battleService, gameWorld);
                    nextTickAt = now.Add(TickRate);
                }

                var commandOptions = GetCommandOptions(gameWorld);
                if (selectedCommandIndex >= commandOptions.Count)
                {
                    selectedCommandIndex = commandOptions.Count - 1;
                }

                renderService.Render(gameWorld, commandOptions, selectedCommandIndex);
                Thread.Sleep(50);
            }
        }
        finally
        {
            Console.CursorVisible = true;
            Console.Clear();
        }
    }

    private static IReadOnlyList<string> GetCommandOptions(Models.GameWorld gameWorld)
    {
        if (gameWorld.State == Models.GameState.Battle)
        {
            return ["Quit"];
        }

        if (gameWorld.State == Models.GameState.PreBattle)
        {
            return ["Begin Battle", "Back to Village", "Quit"];
        }

        if (gameWorld.State == Models.GameState.PostBattle)
        {
            return ["Continue", "Quit"];
        }

        return ["Add Spearman", "Start Battle", "Quit"];
    }

    private static int GetNextCommandIndex(int selectedCommandIndex, int optionCount)
    {
        return (selectedCommandIndex + 1) % optionCount;
    }

    private static int GetPreviousCommandIndex(int selectedCommandIndex, int optionCount)
    {
        return (selectedCommandIndex - 1 + optionCount) % optionCount;
    }

    private static bool ExecuteSelectedCommand(PreparationService preparationService, BattleService battleService, PostBattleService postBattleService, Models.GameWorld gameWorld, int selectedCommandIndex)
    {
        if (gameWorld.State == Models.GameState.Battle)
        {
            return selectedCommandIndex == 0;
        }

        if (gameWorld.State == Models.GameState.PreBattle)
        {
            switch (selectedCommandIndex)
            {
                case 0:
                    battleService.BeginBattle(gameWorld);
                    return false;
                case 1:
                    gameWorld.State = Models.GameState.Village;
                    return false;
                case 2:
                    return true;
                default:
                    return false;
            }
        }

        if (gameWorld.State == Models.GameState.PostBattle)
        {
            switch (selectedCommandIndex)
            {
                case 0:
                    postBattleService.ExitBattleScreen(gameWorld);
                    return false;
                case 1:
                    return true;
                default:
                    return false;
            }
        }

        switch (selectedCommandIndex)
        {
            case 0:
                preparationService.AddSpearman(gameWorld);
                return false;
            case 1:
                battleService.StartBattle(gameWorld);
                return false;
            case 2:
                return true;
            default:
                return false;
        }
    }

    private static void TickGameState(PreparationService preparationService, BattleService battleService, Models.GameWorld gameWorld)
    {
        switch (gameWorld.State)
        {
            case Models.GameState.Village:
                preparationService.Tick(gameWorld);
                break;
            case Models.GameState.Battle:
                battleService.ResolveBattleTick(gameWorld);
                break;
        }
    }
}
