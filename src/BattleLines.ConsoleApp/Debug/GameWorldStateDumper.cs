using System.Text.Json;
using System.Text.Json.Serialization;
using BattleLines.ConsoleApp.Models;

namespace BattleLines.ConsoleApp.Debug;

public sealed class GameWorldStateDumper
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string outputPath;

    public GameWorldStateDumper(string? outputPath = null)
    {
        this.outputPath = outputPath ?? Path.Combine(Environment.CurrentDirectory, "debug", "game-state.latest.json");
    }

    public void Dump(GameWorld gameWorld)
    {
        var directoryPath = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var json = JsonSerializer.Serialize(gameWorld, SerializerOptions);
        var temporaryPath = $"{outputPath}.tmp";

        File.WriteAllText(temporaryPath, json);
        File.Move(temporaryPath, outputPath, overwrite: true);
    }
}
