using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Services;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Tests;

public class GameFlowTests
{
    [Fact]
    public void RenderUnitCount_ForSpearmen_DisplaysCombinedArmyTotal()
    {
        var gameWorld = new GameWorld
        {
            MaxArmySize = 8,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.SpearmenLvl1] = 3,
                [UnitType.Fighter] = 2
            }
        };

        var renderedCount = UnitDisplayComponent.RenderUnitCount(gameWorld, UnitType.SpearmenLvl1, 3);

        Assert.Equal("|||||OOO", renderedCount);
    }

    [Fact]
    public void RenderUnitCount_ForSpearmen_InBattleDisplaysCombinedArmyLosses()
    {
        var gameWorld = new GameWorld
        {
            MaxArmySize = 8,
            State = GameState.Battle,
            PlayerTotalHealth = 48,
            PlayerHealthAtBattleStart = 62,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.SpearmenLvl1] = 3,
                [UnitType.Fighter] = 2
            },
            PlayerUnitsAtBattleStart = new Dictionary<UnitType, int>
            {
                [UnitType.SpearmenLvl1] = 3,
                [UnitType.Fighter] = 2
            }
        };

        var renderedCount = UnitDisplayComponent.RenderUnitCount(gameWorld, UnitType.SpearmenLvl1, 3);

        Assert.Equal("||||XOOO", renderedCount);
    }

    [Fact]
    public void Create_StartsPlayerWithFiveSpearmenLvl1()
    {
        var gameWorld = new GameWorldFactory().Create();

        Assert.True(gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var count));
        Assert.Equal(5, count);
    }

    [Fact]
    public void Create_PopulatesAggregatedCombatStats()
    {
        var gameWorld = new GameWorldFactory().Create();

        Assert.Equal(70, gameWorld.PlayerTotalHealth);
        Assert.Equal(25, gameWorld.PlayerTotalAttack);
        Assert.Equal(24, gameWorld.CurrentWaveTotalHealth);
        Assert.Equal(9, gameWorld.CurrentWaveTotalAttack);
    }

    [Fact]
    public void Create_StartsWithEightArmyPositions()
    {
        var gameWorld = new GameWorldFactory().Create();

        Assert.Equal(8, gameWorld.MaxArmySize);
    }

    [Fact]
    public void AddSpearman_DoesNothing_OutsideVillageAndPreBattle()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.State = GameState.Battle;

        new AddSpearmanCommand().Execute(gameWorld);

        Assert.Equal(5, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(70, gameWorld.PlayerTotalHealth);
        Assert.Equal(25, gameWorld.PlayerTotalAttack);
    }

    [Fact]
    public void AddSpearman_DoesNothing_WhenPlayerCannotPayCost()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.Villagers = 0;
        gameWorld.Spears = 0;

        new AddSpearmanCommand().Execute(gameWorld);

        Assert.Equal(5, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(0, gameWorld.Villagers);
        Assert.Equal(0, gameWorld.Spears);
        Assert.Equal(70, gameWorld.PlayerTotalHealth);
        Assert.Equal(25, gameWorld.PlayerTotalAttack);
    }

    [Fact]
    public void AddSpearman_DoesNothing_WhenArmyIsFull()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.PlayerUnits[UnitType.SpearmenLvl1] = gameWorld.MaxArmySize;
        gameWorld.Villagers = 10;
        gameWorld.Spears = 10;
        new GameWorldStatsService().Refresh(gameWorld);

        new AddSpearmanCommand().Execute(gameWorld);

        Assert.Equal(gameWorld.MaxArmySize, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(10, gameWorld.Villagers);
        Assert.Equal(10, gameWorld.Spears);
    }

    [Fact]
    public void StartBattle_SetsGameStateToPreBattle()
    {
        var gameWorld = new GameWorldFactory().Create();

        new StartBattleCommand().Execute(gameWorld);

        Assert.Equal(GameState.PreBattle, gameWorld.State);
    }

    [Fact]
    public void BeginBattle_SetsGameStateToBattle()
    {
        var gameWorld = new GameWorldFactory().Create();

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);

        Assert.Equal(GameState.Battle, gameWorld.State);
        Assert.Equal(70, gameWorld.PlayerHealthAtBattleStart);
    }

    [Fact]
    public void ReturnToVillage_ReturnsToVillageAndRestoresWaveStats()
    {
        var gameWorld = new GameWorldFactory().Create();

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);
        new ReturnToVillageCommand().Execute(gameWorld);

        Assert.Equal(GameState.Village, gameWorld.State);
        Assert.Equal(24, gameWorld.CurrentWaveTotalHealth);
        Assert.Equal(9, gameWorld.CurrentWaveTotalAttack);
        Assert.Empty(gameWorld.PlayerHealthHistory);
        Assert.Empty(gameWorld.PlayerAttackHistory);
        Assert.Empty(gameWorld.EnemyHealthHistory);
    }

    [Fact]
    public void ResolveBattleTick_InBattleState_DealsDamageToBothSides()
    {
        var gameWorld = new GameWorldFactory().Create();

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);

        Assert.Equal(61, gameWorld.PlayerTotalHealth);
        Assert.Equal(0, gameWorld.CurrentWaveTotalHealth);
    }

    [Fact]
    public void ResolveBattleTick_RecalculatesPlayerAndWaveAttackEachTick()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.EnemyWaves.Waves[0].Enemies[0].Count = 10;
        new GameWorldStatsService().Refresh(gameWorld);

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);

        Assert.Equal(55, gameWorld.CurrentWaveTotalHealth);
        Assert.Equal(40, gameWorld.PlayerTotalHealth);
        Assert.Equal(15, gameWorld.PlayerTotalAttack);
        Assert.Equal(21, gameWorld.CurrentWaveTotalAttack);
        Assert.Equal([25], gameWorld.PlayerAttackHistory);
        Assert.Equal([30], gameWorld.EnemyAttackHistory);
    }

    [Fact]
    public void ResolveBattleTick_WhenFinalBattleEnds_MovesToPostBattleStateWithoutAdvancingWave()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.EnemyWaves.Waves.RemoveRange(1, gameWorld.EnemyWaves.Waves.Count - 1);
        gameWorld.TotalWaveCount = gameWorld.EnemyWaves.Waves.Count;
        new GameWorldStatsService().Refresh(gameWorld);
        gameWorld.PlayerTotalAttack = 100;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);

        Assert.Equal(GameState.PostBattle, gameWorld.State);
        Assert.True(gameWorld.LastBattleWon);
        Assert.True(gameWorld.HasPendingPostBattleResolution);
        Assert.Single(gameWorld.EnemyWaves.Waves);
        Assert.Equal(0, gameWorld.CurrentWaveTotalHealth);
    }

    [Fact]
    public void ExitPostBattle_AppliesRewardAndReturnsToVillage()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.EnemyWaves.Waves.RemoveRange(1, gameWorld.EnemyWaves.Waves.Count - 1);
        gameWorld.TotalWaveCount = gameWorld.EnemyWaves.Waves.Count;
        new GameWorldStatsService().Refresh(gameWorld);
        gameWorld.PlayerTotalAttack = 100;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);

        Assert.Equal(GameState.Village, gameWorld.State);
        Assert.Empty(gameWorld.EnemyWaves.Waves);
        Assert.Equal(10, gameWorld.Spears);
        Assert.Equal(70, gameWorld.PlayerTotalHealth);
        Assert.Equal(25, gameWorld.PlayerTotalAttack);
        Assert.False(gameWorld.HasPendingPostBattleResolution);
    }

    [Fact]
    public void ExitPostBattle_DoesNothing_OutsidePostBattleStates()
    {
        var gameWorld = new GameWorldFactory().Create();

        new ExitPostBattleCommand().Execute(gameWorld);

        Assert.Equal(GameState.Village, gameWorld.State);
        Assert.Equal(5, gameWorld.EnemyWaves.Waves.Count);
    }

    [Fact]
    public void ExitPostBattle_RunsOnlyOnceAfterBattle()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.EnemyWaves.Waves.RemoveRange(1, gameWorld.EnemyWaves.Waves.Count - 1);
        gameWorld.TotalWaveCount = gameWorld.EnemyWaves.Waves.Count;
        new GameWorldStatsService().Refresh(gameWorld);
        gameWorld.PlayerTotalAttack = 100;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);

        Assert.Equal(10, gameWorld.Spears);
        Assert.Empty(gameWorld.EnemyWaves.Waves);
        Assert.False(gameWorld.HasPendingPostBattleResolution);
    }

    [Fact]
    public void ExitPostBattle_ReducesSpearmenByWholeUnitsLostFromHealthLost()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.State = GameState.PostBattle;
        gameWorld.HasPendingPostBattleResolution = true;
        gameWorld.PlayerHealthAtBattleStart = 70;
        gameWorld.PlayerTotalHealth = 42;

        new ExitPostBattleCommand().Execute(gameWorld);

        Assert.Equal(3, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
    }
}
