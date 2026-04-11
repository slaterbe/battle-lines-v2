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

To skip the introduction screen:

```bash
dotnet run --project src/BattleLines.ConsoleApp -- --skip-intro
```

To hide the debug panel:

```bash
dotnet run --project src/BattleLines.ConsoleApp -- --hide-debug
```

You can combine flags:

```bash
dotnet run --project src/BattleLines.ConsoleApp -- --skip-intro --hide-debug
```

Controls:

- `Up` / `Left` changes the selected command
- `Down` / `Right` changes the selected command
- `Enter` confirms the selected command

Game flow notes:

- The game starts on an introduction screen by default
- Passing `--skip-intro` starts directly in the village
- Passing `--hide-debug` hides the debug panel at the bottom of the console
- Village production is applied when returning from battle preparation to the village

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
