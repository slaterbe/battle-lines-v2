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
        var gameWorldFactory = new GameWorldFactory();

        var gameWorld = gameWorldFactory.Create();

        Assert.True(gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var count));
        Assert.Equal(5, count);
    }

    [Fact]
    public void Create_PopulatesAggregatedCombatStats()
    {
        var gameWorldFactory = new GameWorldFactory();

        var gameWorld = gameWorldFactory.Create();

        Assert.Equal(70, gameWorld.PlayerTotalHealth);
        Assert.Equal(25, gameWorld.PlayerTotalAttack);
        Assert.Equal(24, gameWorld.CurrentWaveTotalHealth);
        Assert.Equal(9, gameWorld.CurrentWaveTotalAttack);
    }

    [Fact]
    public void Tick_DoesNotIncrementResources_InVillageState()
    {
        var preparationService = new PreparationService();
        var gameWorld = new GameWorld
        {
            Villagers = 10,
            Spears = 5,
            VillagerProduction = 2,
            SpearProduction = 1
        };

        preparationService.Tick(gameWorld);

        Assert.Equal(10, gameWorld.Villagers);
        Assert.Equal(5, gameWorld.Spears);
    }

    [Fact]
    public void Create_StartsWithEightSpearmenPositions()
    {
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();

        Assert.Equal(8, gameWorld.MaxArmySize);
    }

    [Fact]
    public void AddSpearman_DoesNothing_OutsideVillageState()
    {
        var preparationService = new PreparationService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();
        gameWorld.State = GameState.Battle;

        preparationService.AddSpearman(gameWorld);

        Assert.Equal(5, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(70, gameWorld.PlayerTotalHealth);
        Assert.Equal(25, gameWorld.PlayerTotalAttack);
    }

    [Fact]
    public void AddSpearman_DoesNothing_WhenPlayerCannotPayCost()
    {
        var preparationService = new PreparationService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();
        gameWorld.Villagers = 0;
        gameWorld.Spears = 0;

        preparationService.AddSpearman(gameWorld);

        Assert.Equal(5, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(0, gameWorld.Villagers);
        Assert.Equal(0, gameWorld.Spears);
        Assert.Equal(70, gameWorld.PlayerTotalHealth);
        Assert.Equal(25, gameWorld.PlayerTotalAttack);
    }

    [Fact]
    public void AddSpearman_DoesNothing_WhenSpearmenPositionsAreFull()
    {
        var preparationService = new PreparationService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();
        gameWorld.PlayerUnits[UnitType.SpearmenLvl1] = gameWorld.MaxArmySize;
        gameWorld.Villagers = 10;
        gameWorld.Spears = 10;
        new GameWorldStatsService().Refresh(gameWorld);

        preparationService.AddSpearman(gameWorld);

        Assert.Equal(gameWorld.MaxArmySize, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(10, gameWorld.Villagers);
        Assert.Equal(10, gameWorld.Spears);
    }

    [Fact]
    public void StartBattle_SetsGameStateToPreBattle()
    {
        var battleService = new BattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();

        battleService.StartBattle(gameWorld);

        Assert.Equal(GameState.PreBattle, gameWorld.State);
    }

    [Fact]
    public void BeginBattle_SetsGameStateToBattle()
    {
        var battleService = new BattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();

        battleService.StartBattle(gameWorld);
        battleService.BeginBattle(gameWorld);

        Assert.Equal(GameState.Battle, gameWorld.State);
        Assert.Equal(70, gameWorld.PlayerHealthAtBattleStart);
    }

    [Fact]
    public void ResetCurrentWave_ReturnsToVillageAndRestoresWaveStats()
    {
        var battleService = new BattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();

        battleService.StartBattle(gameWorld);
        battleService.BeginBattle(gameWorld);
        battleService.ResolveBattleTick(gameWorld);
        battleService.ResetCurrentWave(gameWorld);

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
        var battleService = new BattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();

        battleService.StartBattle(gameWorld);
        battleService.ResolveBattleTick(gameWorld);

        Assert.Equal(61, gameWorld.PlayerTotalHealth);
        Assert.Equal(0, gameWorld.CurrentWaveTotalHealth);
    }

    [Fact]
    public void ResolveBattleTick_RecalculatesPlayerAndWaveAttackEachTick()
    {
        var battleService = new BattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();
        gameWorld.EnemyWaveList[0].Enemies[0].Count = 10;
        new GameWorldStatsService().Refresh(gameWorld);

        battleService.StartBattle(gameWorld);
        battleService.BeginBattle(gameWorld);
        battleService.ResolveBattleTick(gameWorld);

        Assert.Equal(55, gameWorld.CurrentWaveTotalHealth);
        Assert.Equal(40, gameWorld.PlayerTotalHealth);
        Assert.Equal(15, gameWorld.PlayerTotalAttack);
        Assert.Equal(21, gameWorld.CurrentWaveTotalAttack);
        Assert.Equal([25], gameWorld.PlayerAttackHistory);
        Assert.Equal([30], gameWorld.EnemyAttackHistory);
    }

    [Fact]
    public void ResolveBattleTick_WhenBattleEnds_MovesToPostBattleStateWithoutAdvancingWave()
    {
        var battleService = new BattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();
        gameWorld.PlayerTotalAttack = 100;

        battleService.StartBattle(gameWorld);
        battleService.ResolveBattleTick(gameWorld);

        Assert.Equal(GameState.PostBattle, gameWorld.State);
        Assert.True(gameWorld.LastBattleWon);
        Assert.True(gameWorld.HasPendingPostBattleResolution);
        Assert.Equal(5, gameWorld.EnemyWaveList.Count);
        Assert.Equal(0, gameWorld.CurrentWaveTotalHealth);
    }

    [Fact]
    public void ExitBattleScreen_AppliesRewardAndAdvancesToNextWave()
    {
        var battleService = new BattleService();
        var postBattleService = new PostBattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();
        gameWorld.PlayerTotalAttack = 100;

        battleService.StartBattle(gameWorld);
        battleService.BeginBattle(gameWorld);
        battleService.ResolveBattleTick(gameWorld);
        postBattleService.ExitBattleScreen(gameWorld);

        Assert.Equal(GameState.Village, gameWorld.State);
        Assert.Equal(4, gameWorld.EnemyWaveList.Count);
        Assert.Equal(5, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(10, gameWorld.Spears);
        Assert.Equal(70, gameWorld.PlayerTotalHealth);
        Assert.Equal(25, gameWorld.PlayerTotalAttack);
        Assert.Equal(40, gameWorld.CurrentWaveTotalHealth);
        Assert.False(gameWorld.HasPendingPostBattleResolution);
    }

    [Fact]
    public void ExitBattleScreen_DoesNothing_IfBattleScreenIsStillActive()
    {
        var postBattleService = new PostBattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();

        postBattleService.ExitBattleScreen(gameWorld);

        Assert.Equal(GameState.Village, gameWorld.State);
        Assert.Equal(5, gameWorld.EnemyWaveList.Count);
    }

    [Fact]
    public void ExitBattleScreen_RunsOnlyOnceAfterBattle()
    {
        var battleService = new BattleService();
        var postBattleService = new PostBattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();
        gameWorld.PlayerTotalAttack = 100;

        battleService.StartBattle(gameWorld);
        battleService.BeginBattle(gameWorld);
        battleService.ResolveBattleTick(gameWorld);
        postBattleService.ExitBattleScreen(gameWorld);
        postBattleService.ExitBattleScreen(gameWorld);

        Assert.Equal(10, gameWorld.Spears);
        Assert.Equal(4, gameWorld.EnemyWaveList.Count);
        Assert.Equal(5, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.False(gameWorld.HasPendingPostBattleResolution);
    }

    [Fact]
    public void ExitBattleScreen_ReducesSpearmenByWholeUnitsLostFromHealthLost()
    {
        var postBattleService = new PostBattleService();
        var gameWorldFactory = new GameWorldFactory();
        var gameWorld = gameWorldFactory.Create();
        gameWorld.State = GameState.PostBattle;
        gameWorld.HasPendingPostBattleResolution = true;
        gameWorld.PlayerHealthAtBattleStart = 70;
        gameWorld.PlayerTotalHealth = 42;

        postBattleService.ExitBattleScreen(gameWorld);

        Assert.Equal(3, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
    }
}
