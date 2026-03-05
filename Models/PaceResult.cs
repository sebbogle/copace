namespace Copace.Models;

public record PaceResult
{
    public required decimal ActualUsage { get; init; }

    public required decimal ExpectedUsage { get; init; }

    public decimal Difference => ActualUsage - ExpectedUsage;

    public required decimal ProjectedUsage { get; init; }

    public required int ElapsedDays { get; init; }

    public required int TotalDays { get; init; }

    public required bool IncludesWeekends { get; init; }

    public required DateOnly Date { get; init; }

    public PaceStatus Status => Difference switch
    {
        > 15m => PaceStatus.WellAhead,
        > 5m => PaceStatus.Ahead,
        >= -5m => PaceStatus.OnTrack,
        >= -15m => PaceStatus.Behind,
        _ => PaceStatus.WellBehind
    };
}

public enum PaceStatus
{
    WellAhead,
    Ahead,
    OnTrack,
    Behind,
    WellBehind
}
