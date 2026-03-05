namespace Copace.Services;

public static class PaceCalculator
{
    public static PaceResult Calculate(decimal actualUsage, bool includeWeekends = false, DateOnly? date = null)
    {
        DateOnly today = date ?? DateOnly.FromDateTime(DateTime.Now);
        int year = today.Year;
        int month = today.Month;
        int daysInMonth = DateTime.DaysInMonth(year, month);

        int totalDays;
        int elapsedDays;

        if (includeWeekends)
        {
            totalDays = daysInMonth;
            elapsedDays = today.Day;
        }
        else
        {
            totalDays = CountWeekdays(year, month, 1, daysInMonth);
            elapsedDays = CountWeekdays(year, month, 1, today.Day);
        }

        decimal expectedUsage = totalDays > 0
            ? (decimal)elapsedDays / totalDays * 100m
            : 0m;

        decimal projectedUsage = elapsedDays > 0
            ? actualUsage / elapsedDays * totalDays
            : 0m;

        return new PaceResult
        {
            ActualUsage = actualUsage,
            ExpectedUsage = Math.Round(expectedUsage, 1),
            ProjectedUsage = Math.Round(projectedUsage, 1),
            ElapsedDays = elapsedDays,
            TotalDays = totalDays,
            IncludesWeekends = includeWeekends,
            Date = today
        };
    }

    private static int CountWeekdays(int year, int month, int fromDay, int toDay)
    {
        int count = 0;
        for (int day = fromDay; day <= toDay; day++)
        {
            DayOfWeek dow = new DateOnly(year, month, day).DayOfWeek;
            if (dow is not (DayOfWeek.Saturday or DayOfWeek.Sunday))
                count++;
        }
        return count;
    }
}
