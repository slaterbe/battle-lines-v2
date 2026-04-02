namespace BattleLines.ConsoleApp.Models;

public static class UnitCatalog
{
    public static readonly IReadOnlyDictionary<UnitType, UnitModel> DefaultUnits =
        new Dictionary<UnitType, UnitModel>
        {
            [UnitType.GiantRat] = new()
            {
                Health = 8,
                Attack = 3
            },
            [UnitType.SpearmenLvl1] = new()
            {
                Health = 14,
                Attack = 5
            }
        };
}
