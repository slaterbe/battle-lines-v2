# Battle Lines V2

Baseline `.NET 8` solution containing:

- A console application at `src/BattleLines.ConsoleApp`
- An xUnit test project at `tests/BattleLines.ConsoleApp.Tests`

## Prerequisites

- `.NET SDK 8.0`

## Run the console app

From the repository root:

```bash
dotnet run --project src/BattleLines.ConsoleApp
```

Controls:

- `P` toggles pause and resume
- `Q` quits the game

While the game is running, `Villagers` and `Spears` increase by their production values once per tick.

## Run the tests

From the repository root:

```bash
dotnet test BattleLines.sln
```

## Build the solution

From the repository root:

```bash
dotnet build BattleLines.sln
```
