namespace BattleLines.ConsoleApp.Models;

public static class UnitCatalog
{
    public static readonly IReadOnlyDictionary<UnitType, UnitModel> DefaultUnits =
        new Dictionary<UnitType, UnitModel>
        {
            [UnitType.GiantRat] = new()
            {
                UnitAcronym = 'R',
                Health = 8,
                Attack = 3,
                MaxAttack = 0
            },
            [UnitType.SpearmenLvl1] = new()
            {
                UnitAcronym = 'S',
                Health = 14,
                Attack = 5,
                MaxAttack = 10
            },
            [UnitType.Fighter] = new()
            {
                UnitAcronym = 'F',
                Health = 10,
                Attack = 3,
                MaxAttack = 0
            }
        };
}
