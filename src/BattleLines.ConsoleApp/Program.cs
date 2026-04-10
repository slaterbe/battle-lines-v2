using BattleLines.ConsoleApp.Controllers;
using BattleLines.ConsoleApp.Debug;
using BattleLines.ConsoleApp.Services;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp;

public static class Program
{
    private static readonly TimeSpan TickRate = TimeSpan.FromSeconds(1);

    public static void Main(string[] args)
    {
        var controllerFactory = new GameStateControllerFactory(
            new IntroductionController(),
            new VillageController(),
            new PreWaveController(),
            new WaveController(),
            new PostWaveController(),
            new PostBattleController());
        var gameEventService = new GameEventService();
        var gameWorldFactory = new GameWorldFactory();
        var renderService = new RenderService();
        var skipIntroduction = args.Any(arg => arg.Equals("--skip-intro", StringComparison.OrdinalIgnoreCase));
        var gameWorld = gameWorldFactory.Create(skipIntroduction);
        var stateDumper = new GameWorldStateDumper();
        var nextTickAt = DateTime.UtcNow.Add(TickRate);
        var shouldExit = false;
        var selectedCommandIndex = 0;
        var previousGameState = gameWorld.State;
        ConsoleCancelEventHandler cancelHandler = (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            shouldExit = true;
        };

        Console.CursorVisible = false;
        Console.CancelKeyPress += cancelHandler;

        try
        {
            gameEventService.CheckEvents(gameWorld);
            stateDumper.Dump(gameWorld);

            while (!shouldExit)
            {
                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    var activeController = controllerFactory.GetController(gameWorld.State);
                    var availableCommands = activeController.GetCommandOptions(gameWorld);

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
                            shouldExit = activeController.HandleCommand(gameWorld, selectedCommandIndex);
                            gameEventService.CheckEvents(gameWorld);
                            stateDumper.Dump(gameWorld);
                            break;
                    }
                }

                if (gameWorld.State != previousGameState)
                {
                    selectedCommandIndex = 0;
                    previousGameState = gameWorld.State;
                }

                var now = DateTime.UtcNow;
                if (now >= nextTickAt)
                {
                    controllerFactory.GetController(gameWorld.State).Tick(gameWorld);
                    gameEventService.CheckEvents(gameWorld);
                    stateDumper.Dump(gameWorld);
                    nextTickAt = now.Add(TickRate);
                }

                if (gameWorld.State != previousGameState)
                {
                    selectedCommandIndex = 0;
                    previousGameState = gameWorld.State;
                }

                var commandOptions = controllerFactory.GetController(gameWorld.State).GetCommandOptions(gameWorld);
                if (commandOptions.Count == 0)
                {
                    selectedCommandIndex = 0;
                }
                else if (selectedCommandIndex >= commandOptions.Count)
                {
                    selectedCommandIndex = commandOptions.Count - 1;
                }

                renderService.Render(gameWorld, commandOptions, selectedCommandIndex);
                Thread.Sleep(50);
            }
        }
        finally
        {
            Console.CancelKeyPress -= cancelHandler;
            ConsoleTextComponent.RestoreConsoleAfterExit();
            Console.CursorVisible = true;
        }
    }

    private static int GetNextCommandIndex(int selectedCommandIndex, int optionCount)
    {
        if (optionCount == 0)
        {
            return 0;
        }

        return (selectedCommandIndex + 1) % optionCount;
    }

    private static int GetPreviousCommandIndex(int selectedCommandIndex, int optionCount)
    {
        if (optionCount == 0)
        {
            return 0;
        }

        return (selectedCommandIndex - 1 + optionCount) % optionCount;
    }
}
