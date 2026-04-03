using BattleLines.ConsoleApp.Commands;
using BattleLines.ConsoleApp.Models;
using BattleLines.ConsoleApp.Views.Components;

namespace BattleLines.ConsoleApp.Views;

public class VillageView : IGameView
{
    private static readonly GameScreenLayoutComponent Layout = new();

    public void Render(GameWorld gameWorld, IReadOnlyList<GameCommandOption> commandOptions, int selectedCommandIndex)
    {
        var supplementalDetails =
            $"Commoner Production: +{gameWorld.CommonerProduction}\n" +
            $"Spear Production: +{gameWorld.SpearProduction}\n" +
            $"Max Spearmen Capacity: {gameWorld.MaxSpearmenPositions}";

        Layout.Render(
            gameWorld,
            "Village: Choose upgrades or start a battle",
            ConsoleColor.Green,
            commandOptions,
            selectedCommandIndex,
            supplementalDetails,
            showWaveOverview: false,
            showCurrentWave: false);
    }
}
