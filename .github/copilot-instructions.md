# Project Guidelines

## Code Style

- **No `var`** — always use explicit types (`decimal usage`, `string label`, `ConsoleColor color`)
- **No comments** — code should be self-documenting through clear naming; do not add XML doc comments, inline comments, or TODO markers
- **File-scoped namespaces** — `namespace Copace.Services;`
- **No per-file `using` directives** — all usings are declared as `<Using>` items in the csproj; never add `using` statements to `.cs` files
- **Allman brace style** with modern pattern matching (`switch` expressions, `is not (... or ...)`)
- **Expression-bodied members** for simple single-expression methods
- See [Services/PaceCalculator.cs](Services/PaceCalculator.cs) and [Models/PaceResult.cs](Models/PaceResult.cs) for reference

## Architecture

```
Program.cs              → CLI wiring (top-level statements, System.CommandLine v3)
Models/PaceResult.cs    → Immutable record + PaceStatus enum
Services/
  PaceCalculator.cs     → Pure calculation logic (static)
  ConsoleRenderer.cs    → Colored terminal output (static)
```

Data flow: `Program.cs` → `PaceCalculator.Calculate()` → `PaceResult` → `ConsoleRenderer.Render()`

All services are **static classes** — no DI, no `IHost`. Keep it simple.

## Build and Test

```shell
dotnet build
dotnet run -- 42.5                        # weekdays-only (default)
dotnet run -- 42.5 --include-weekends     # calendar days
dotnet run -- 42.5 -d 2026-03-15          # override date
```

Target: `net10.0` | Nullable: enabled | ImplicitUsings: enabled

## Project Conventions

- **System.CommandLine v3 preview** — uses `SetAction(Action<ParseResult>)` and `parseResult.GetValue(symbol)`, not the older v2 `SetHandler` API
- **`record` with `required init`** — models use `required` init-only properties plus computed properties (`Difference`, `Status`) on the record itself
- **Decimal literals** — use `m` suffix consistently (`100m`, `5m`, `15m`)
- **Console colors** — managed via `WriteColored`/`WriteLineColored` helpers that save/restore `Console.ForegroundColor`; no external UI library
- **Root namespace** is `Copace`
