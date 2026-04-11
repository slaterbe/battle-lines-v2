using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Debug;
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
    public void BattleLine_RendersEnemyArmySummary()
    {
        var gameWorld = new GameWorld
        {
            EnemyWaves = new EnemyWaveSetModel
            {
                Waves =
                [
                    new EnemyWaveModel
                    {
                        Enemies =
                        [
                            new EnemyWaveUnitModel { EnemyType = UnitType.GiantRat, Count = 3 }
                        ]
                    }
                ]
            }
        };

        var renderedLine = BattleLineComponent.RenderEnemyArmyLine(gameWorld);

        Assert.Equal("Enemy Army: |||", renderedLine);
    }

    [Fact]
    public void BattleLine_RendersPlayerArmySummary()
    {
        var gameWorld = new GameWorld
        {
            MaxArmySize = 6,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.Fighter] = 2,
                [UnitType.SpearmenLvl1] = 1
            }
        };

        var renderedLine = BattleLineComponent.RenderPlayerArmyLine(gameWorld);

        Assert.Equal("Player Army: |||OOO", renderedLine);
    }

    [Fact]
    public void Create_StartsPlayerWithNoSpearmen()
    {
        var gameWorld = new GameWorldFactory().Create();

        Assert.True(gameWorld.PlayerUnits.TryGetValue(UnitType.SpearmenLvl1, out var count));
        Assert.Equal(0, count);
    }

    [Fact]
    public void Create_StartsAtIntroductionByDefault()
    {
        var gameWorld = new GameWorldFactory().Create();

        Assert.Equal(GameState.Introduction, gameWorld.State);
    }

    [Fact]
    public void Create_PopulatesAggregatedCombatStats()
    {
        var gameWorld = new GameWorldFactory().Create();

        Assert.Equal(10, gameWorld.PlayerTotalHealth);
        Assert.Equal(3, gameWorld.PlayerTotalAttack);
        Assert.Equal(16, gameWorld.CurrentWaveTotalHealth);
        Assert.Equal(6, gameWorld.CurrentWaveTotalAttack);
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

        Assert.Equal(0, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(10, gameWorld.PlayerTotalHealth);
        Assert.Equal(3, gameWorld.PlayerTotalAttack);
    }

    [Fact]
    public void AddSpearman_DoesNothing_WhenPlayerCannotPayCost()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.State = GameState.Village;
        gameWorld.Villagers = 0;
        gameWorld.Spears = 0;

        new AddSpearmanCommand().Execute(gameWorld);

        Assert.Equal(0, gameWorld.PlayerUnits[UnitType.SpearmenLvl1]);
        Assert.Equal(0, gameWorld.Villagers);
        Assert.Equal(0, gameWorld.Spears);
        Assert.Equal(10, gameWorld.PlayerTotalHealth);
        Assert.Equal(3, gameWorld.PlayerTotalAttack);
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
        gameWorld.State = GameState.Village;

        new StartBattleCommand().Execute(gameWorld);

        Assert.Equal(GameState.PreBattle, gameWorld.State);
    }

    [Fact]
    public void DefendVillage_FromIntroduction_MovesToVillageScreen()
    {
        var gameWorld = new GameWorldFactory().Create();

        new DefendVillageCommand().Execute(gameWorld);

        Assert.Equal(GameState.Village, gameWorld.State);
        Assert.Equal(1, gameWorld.WavePosition);
    }

    [Fact]
    public void BeginBattle_SetsGameStateToBattle()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.State = GameState.Village;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);

        Assert.Equal(GameState.Battle, gameWorld.State);
        Assert.Equal(10, gameWorld.PlayerHealthAtBattleStart);
    }

    [Fact]
    public void ReturnToVillage_ReturnsToVillageAndRestoresWaveStats()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.State = GameState.Village;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);
        new ReturnToVillageCommand().Execute(gameWorld);

        Assert.Equal(GameState.Village, gameWorld.State);
        Assert.Equal(16, gameWorld.CurrentWaveTotalHealth);
        Assert.Equal(6, gameWorld.CurrentWaveTotalAttack);
        Assert.Empty(gameWorld.PlayerHealthHistory);
        Assert.Empty(gameWorld.PlayerAttackHistory);
        Assert.Empty(gameWorld.EnemyHealthHistory);
    }

    [Fact]
    public void ResolveBattleTick_InBattleState_DealsDamageToBothSides()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.State = GameState.Village;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);

        Assert.Equal(4, gameWorld.PlayerTotalHealth);
        Assert.Equal(13, gameWorld.CurrentWaveTotalHealth);
    }

    [Fact]
    public void ResolveBattleTick_RecalculatesPlayerAndWaveAttackEachTick()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.PlayerUnits[UnitType.Fighter] = 4;
        gameWorld.EnemyWaves.Waves[0].Enemies[0].Count = 10;
        new GameWorldStatsService().Refresh(gameWorld);
        gameWorld.State = GameState.Village;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);

        Assert.Equal(68, gameWorld.CurrentWaveTotalHealth);
        Assert.Equal(10, gameWorld.PlayerTotalHealth);
        Assert.Equal(3, gameWorld.PlayerTotalAttack);
        Assert.Equal(27, gameWorld.CurrentWaveTotalAttack);
        Assert.Equal([12], gameWorld.PlayerAttackHistory);
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
        gameWorld.PlayerTotalMaxAttack = 100;
        gameWorld.State = GameState.Village;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);

        Assert.Equal(GameState.PostWave, gameWorld.State);
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
        gameWorld.PlayerTotalMaxAttack = 100;
        gameWorld.State = GameState.Village;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);

        Assert.Equal(GameState.Village, gameWorld.State);
        Assert.Equal(5, gameWorld.EnemyWaves.Waves.Count);
        Assert.Equal(0, gameWorld.Spears);
        Assert.Equal(5, gameWorld.Gold);
        Assert.Equal(10, gameWorld.PlayerTotalHealth);
        Assert.Equal(3, gameWorld.PlayerTotalAttack);
        Assert.False(gameWorld.HasPendingPostBattleResolution);
    }

    [Fact]
    public void ExitPostBattle_DoesNothing_OutsidePostBattleStates()
    {
        var gameWorld = new GameWorldFactory().Create();

        new ExitPostBattleCommand().Execute(gameWorld);

        Assert.Equal(GameState.Introduction, gameWorld.State);
        Assert.Single(gameWorld.EnemyWaves.Waves);
    }

    [Fact]
    public void ExitPostBattle_RunsOnlyOnceAfterBattle()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.EnemyWaves.Waves.RemoveRange(1, gameWorld.EnemyWaves.Waves.Count - 1);
        gameWorld.TotalWaveCount = gameWorld.EnemyWaves.Waves.Count;
        new GameWorldStatsService().Refresh(gameWorld);
        gameWorld.PlayerTotalAttack = 100;
        gameWorld.PlayerTotalMaxAttack = 100;
        gameWorld.State = GameState.Village;

        new StartBattleCommand().Execute(gameWorld);
        new BeginBattleCommand().Execute(gameWorld);
        new ResolveBattleTickCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);
        new ExitPostBattleCommand().Execute(gameWorld);

        Assert.Equal(0, gameWorld.Spears);
        Assert.Equal(5, gameWorld.EnemyWaves.Waves.Count);
        Assert.Equal(5, gameWorld.Gold);
        Assert.False(gameWorld.HasPendingPostBattleResolution);
    }

    [Fact]
    public void ExitPostBattle_ReducesSpearmenByWholeUnitsLostFromHealthLost()
    {
        var gameWorld = new GameWorldFactory().Create();
        gameWorld.State = GameState.PostBattle;
        gameWorld.HasPendingPostBattleResolution = true;
        gameWorld.PlayerHealthAtBattleStart = 30;
        gameWorld.PlayerTotalHealth = 42;
        gameWorld.PlayerUnitsAtBattleStart = new Dictionary<UnitType, int>
        {
            [UnitType.Fighter] = 1
        };

        new ExitPostBattleCommand().Execute(gameWorld);

        Assert.Equal(1, gameWorld.PlayerUnits[UnitType.Fighter]);
    }

    [Fact]
    public void Dump_WritesLatestGameWorldStateToJsonFile()
    {
        var gameWorld = new GameWorld
        {
            State = GameState.Battle,
            Villagers = 12,
            Gold = 7,
            PlayerUnits = new Dictionary<UnitType, int>
            {
                [UnitType.Fighter] = 3
            }
        };
        var outputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}-game-state.latest.json");
        var dumper = new GameWorldStateDumper(outputPath);

        try
        {
            dumper.Dump(gameWorld);

            Assert.True(File.Exists(outputPath));

            var json = File.ReadAllText(outputPath);

            Assert.Contains("\"State\": \"Battle\"", json);
            Assert.Contains("\"Villagers\": 12", json);
            Assert.Contains("\"Gold\": 7", json);
        }
        finally
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }
    }
}
