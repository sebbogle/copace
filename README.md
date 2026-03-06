# Copace CLI

A command-line tool to track your **GitHub Copilot Premium** usage pace throughout the month. Know at a glance whether you're on track, over-spending, or leaving budget on the table.

## Why?

GitHub Copilot Premium usage resets monthly. It's easy to burn through your allowance early or forget to use it at all. Copace calculates where you _should_ be based on how far through the month you are, compares that to your actual usage, and gives you a clear visual summary with actionable advice.

By default, only **weekdays (Mon–Fri)** are counted — because that's when most people code. Pass `--include-weekends` if your schedule differs.

## Installation

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### As a global tool

```shell
dotnet tool install --global Copace
```

Then run from anywhere:

```shell
copace 42.5
```

### From source

```shell
git clone https://github.com/sebbogle/Copace.git
cd Copace
dotnet run 42.5
```

## Usage

```
copace <usage> [options]
```

| Argument / Option           | Description                                                        |
| --------------------------- | ------------------------------------------------------------------ |
| `<usage>`                   | Your current Copilot Premium usage percentage from `0` to `100` (e.g. `42.5`) |
| `--include-weekends`, `-w`  | Include weekend days in the pace calculation (default: weekdays only) |

### Examples

```shell
# Weekday-only pacing (default)
copace 42.5

# Include weekends in the calculation
copace 42.5 --include-weekends
```

## Output

Copace displays:

- **Current Usage** — the percentage you entered
- **Expected Usage** — where you should be based on elapsed weekdays (or calendar days)
- **Difference** — how far ahead or behind you are, with a status label
- **Projected End-of-Month** — where you'll land if you continue at the current rate
- **Progress Bar** — a visual bar comparing actual vs. expected usage
- **Advice** — a contextual tip based on your pace status

### Pace Statuses

| Status         | Condition          | Meaning                                        |
| -------------- | ------------------ | ---------------------------------------------- |
| **Well Ahead** | > +15 %            | Significantly over-pacing — risk of overspend  |
| **Ahead**      | +5 % to +15 %      | Slightly ahead — keep an eye on it             |
| **On Track**   | −5 % to +5 %       | Right where you should be                      |
| **Behind**     | −15 % to −5 %      | Slightly behind — room to use more             |
| **Well Behind**| < −15 %            | Significantly under-utilising your budget      |

## Project Structure

```
Program.cs              → CLI wiring (System.CommandLine v3)
Models/
  PaceResult.cs         → Immutable record + PaceStatus enum
Services/
  PaceCalculator.cs     → Pure calculation logic (static)
  ConsoleRenderer.cs    → Colored terminal output (static)
```