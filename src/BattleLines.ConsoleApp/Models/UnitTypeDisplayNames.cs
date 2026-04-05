namespace BattleLines.ConsoleApp.Models;

public static class UnitTypeDisplayNames
{
    private static readonly IReadOnlyDictionary<UnitType, string> DisplayNames = new Dictionary<UnitType, string>
    {
        [UnitType.Fighter] = "Fighter",
        [UnitType.SpearmenLvl1] = "Spearmen"
    };

    public static string Get(UnitType unitType)
    {
        return DisplayNames.TryGetValue(unitType, out var displayName)
            ? displayName
            : unitType.ToString();
    }
}
