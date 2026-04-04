namespace BattleLines.ConsoleApp.Commands;

public sealed record GameCommandOption(GameCommandCategory Category, string Label, string HelpText, GameCommandCost? Cost);
