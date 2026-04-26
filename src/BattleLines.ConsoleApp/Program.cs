using BattleLines.ConsoleApp.Controllers;
using BattleLines.ConsoleApp.Debug;
using BattleLines.ConsoleApp.Services;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp;

public static class Program
{
    private static readonly TimeSpan TickRate = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan BaseGatherDuration = TimeSpan.FromSeconds(1.4);
    private static readonly TimeSpan MinimumGatherDuration = TimeSpan.FromSeconds(0.55);
    private static readonly TimeSpan GatherAccelerationDuration = TimeSpan.FromSeconds(10);

    public static void Main(string[] args)
    {
        var skipIntroduction = args.Any(arg => arg.Equals("--skip-intro", StringComparison.OrdinalIgnoreCase));
        var hideDebugPanel = args.Any(arg => arg.Equals("--hide-debug", StringComparison.OrdinalIgnoreCase));
        var controllerFactory = new GameStateControllerFactory(
            new IntroductionController(),
            new VillageController(),
            new PreWaveController(),
            new WaveController(),
            new PostWaveController(),
            new PostBattleController());
        var gameEventService = new GameEventService();
        var gameWorldFactory = new GameWorldFactory();
        var renderDiagnostics = new RenderDiagnostics();
        var renderService = new RenderService(renderDiagnostics, showDebugPanel: !hideDebugPanel);
        var gameWorld = gameWorldFactory.Create(skipIntroduction);
        var stateDumper = new GameWorldStateDumper();
        var nextTickAt = DateTime.UtcNow.Add(TickRate);
        var shouldExit = false;
        var selectedCommandIndex = 0;
        var previousGameState = gameWorld.State;
        DateTime? gatherStartedAt = null;
        DateTime? gatherCycleStartedAt = null;
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
                            ResetVillageGatherState(gameWorld, ref gatherStartedAt, ref gatherCycleStartedAt);
                            break;
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.RightArrow:
                            selectedCommandIndex = GetNextCommandIndex(selectedCommandIndex, availableCommands.Count);
                            ResetVillageGatherState(gameWorld, ref gatherStartedAt, ref gatherCycleStartedAt);
                            break;
                        case ConsoleKey.Escape:
                            ResetVillageGatherState(gameWorld, ref gatherStartedAt, ref gatherCycleStartedAt);
                            break;
                        case ConsoleKey.Enter:
                            if (IsVillageHoldCommandSelected(gameWorld, availableCommands, selectedCommandIndex))
                            {
                                if (gatherStartedAt is null)
                                {
                                    var gatherNow = DateTime.UtcNow;
                                    gatherStartedAt = gatherNow;
                                    gatherCycleStartedAt = gatherNow;
                                    gameWorld.IsVillageGoldGatheringActive = true;
                                }
                            }
                            else
                            {
                                shouldExit = activeController.HandleCommand(gameWorld, selectedCommandIndex);
                                gameEventService.CheckEvents(gameWorld);
                                stateDumper.Dump(gameWorld);
                                ResetVillageGatherState(gameWorld, ref gatherStartedAt, ref gatherCycleStartedAt);
                            }
                            break;
                    }
                }

                if (gameWorld.State != previousGameState)
                {
                    selectedCommandIndex = 0;
                    previousGameState = gameWorld.State;
                }

                var now = DateTime.UtcNow;
                UpdateVillageHoldProgress(
                    gameWorld,
                    commandOptions: controllerFactory.GetController(gameWorld.State).GetCommandOptions(gameWorld),
                    selectedCommandIndex,
                    now,
                    ref gatherStartedAt,
                    ref gatherCycleStartedAt,
                    controllerFactory,
                    gameEventService,
                    stateDumper,
                    ref shouldExit);

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

    private static void UpdateVillageHoldProgress(
        Models.GameWorld gameWorld,
        IReadOnlyList<Commands.GameCommandOption> commandOptions,
        int selectedCommandIndex,
        DateTime now,
        ref DateTime? gatherStartedAt,
        ref DateTime? gatherCycleStartedAt,
        Controllers.GameStateControllerFactory controllerFactory,
        Services.GameEventService gameEventService,
        Debug.GameWorldStateDumper stateDumper,
        ref bool shouldExit)
    {
        if (!IsVillageHoldCommandSelected(gameWorld, commandOptions, selectedCommandIndex))
        {
            ResetVillageGatherState(gameWorld, ref gatherStartedAt, ref gatherCycleStartedAt);
            return;
        }

        if (gatherStartedAt is null || gatherCycleStartedAt is null)
        {
            gameWorld.VillageGoldGatherProgress = 0;
            gameWorld.IsVillageGoldGatheringActive = false;
            gameWorld.VillageGoldGatherSpeedMultiplier = 1;
            return;
        }

        gameWorld.IsVillageGoldGatheringActive = true;
        var currentGatherDuration = GetCurrentGatherDuration(now - gatherStartedAt.Value);
        gameWorld.VillageGoldGatherSpeedMultiplier = BaseGatherDuration.TotalMilliseconds / currentGatherDuration.TotalMilliseconds;
        gameWorld.VillageGoldGatherProgress = Math.Clamp((now - gatherCycleStartedAt.Value).TotalMilliseconds / currentGatherDuration.TotalMilliseconds, 0, 1);
        if (gameWorld.VillageGoldGatherProgress < 1)
        {
            return;
        }

        shouldExit = controllerFactory.GetController(gameWorld.State).HandleCommand(gameWorld, selectedCommandIndex);
        gameEventService.CheckEvents(gameWorld);
        stateDumper.Dump(gameWorld);
        gatherCycleStartedAt = now;
        gameWorld.VillageGoldGatherProgress = 0;
    }

    private static bool IsVillageHoldCommandSelected(
        Models.GameWorld gameWorld,
        IReadOnlyList<Commands.GameCommandOption> commandOptions,
        int selectedCommandIndex)
    {
        return gameWorld.State == Models.GameState.Village &&
            selectedCommandIndex >= 0 &&
            selectedCommandIndex < commandOptions.Count &&
            commandOptions[selectedCommandIndex].RequiresHoldToExecute;
    }

    private static TimeSpan GetCurrentGatherDuration(TimeSpan activeDuration)
    {
        var rampProgress = Math.Clamp(activeDuration.TotalMilliseconds / GatherAccelerationDuration.TotalMilliseconds, 0, 1);
        var durationMilliseconds =
            BaseGatherDuration.TotalMilliseconds -
            ((BaseGatherDuration.TotalMilliseconds - MinimumGatherDuration.TotalMilliseconds) * rampProgress);
        return TimeSpan.FromMilliseconds(durationMilliseconds);
    }

    private static void ResetVillageGatherState(
        Models.GameWorld gameWorld,
        ref DateTime? gatherStartedAt,
        ref DateTime? gatherCycleStartedAt)
    {
        gatherStartedAt = null;
        gatherCycleStartedAt = null;
        gameWorld.VillageGoldGatherProgress = 0;
        gameWorld.IsVillageGoldGatheringActive = false;
        gameWorld.VillageGoldGatherSpeedMultiplier = 1;
    }
}
