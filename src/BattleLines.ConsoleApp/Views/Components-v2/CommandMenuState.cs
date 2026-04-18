using BattleLines.ConsoleApp.Commands;

namespace BattleLines.ConsoleApp.Views.ComponentsV2;

public readonly record struct CommandMenuState(
    IReadOnlyList<GameCommandOption> CommandOptions,
    int SelectedCommandIndex,
    bool ShowAnimatedEnterPrompt = false);
