namespace BattleLines.ConsoleApp.Commands;

public sealed record GameCommandCost(
    int Villagers = 0,
    int Spears = 0,
    int Gold = 0,
    int VillagersGain = 0,
    int SpearsGain = 0,
    int GoldGain = 0);
