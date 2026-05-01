using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;

namespace BattleLines.ConsoleApp.Tests;

public class EnemyWaveFactoryTests
{
    [Fact]
    public void HasBattle_ReturnsTrue_ForFourthBattle()
    {
        var factory = new EnemyWaveFactory();

        Assert.True(factory.HasBattle(3));
        Assert.True(factory.HasBattle(4));
    }

    [Fact]
    public void CreateBattle_ReturnsFourthBattleWaveSet()
    {
        var factory = new EnemyWaveFactory();

        var battle = factory.CreateBattle(3);

        Assert.Equal("Unlocked Militia Yard", battle.FlashingVictoryMessage);
        Assert.Equal(6, battle.Waves.Count);
        Assert.Equal(UnitType.Raider, battle.Waves[0].Enemies[0].EnemyType);
        Assert.Equal(4, battle.Waves[0].Enemies[0].Count);
    }

    [Fact]
    public void CreateBattle_ReturnsFifthBattleWaveSet()
    {
        var factory = new EnemyWaveFactory();

        var battle = factory.CreateBattle(4);

        Assert.Equal("Frontier Secured", battle.FlashingVictoryMessage);
        Assert.Equal(6, battle.Waves.Count);
        Assert.Equal(UnitType.Raider, battle.Waves[0].Enemies[0].EnemyType);
        Assert.Equal(6, battle.Waves[0].Enemies[0].Count);
    }
}
