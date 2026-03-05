namespace Copace.Services;

public static class ConsoleRenderer
{
    private const int BarWidth = 40;

    public static void Render(PaceResult result)
    {
        string monthName = result.Date.ToString("MMMM yyyy");
        string dayLabel = result.IncludesWeekends ? "days" : "weekdays";

        Console.WriteLine();
        WriteColored("  Copilot Premium Usage Tracker", ConsoleColor.Cyan);
        Console.WriteLine($"  ({monthName})");
        Console.WriteLine();

        Console.Write("  Current Usage:   ");
        WriteLineColored($"{result.ActualUsage:F1}%", ConsoleColor.White);

        Console.Write("  Expected Usage:  ");
        WriteLineColored(
            $"{result.ExpectedUsage:F1}%  (day {result.ElapsedDays} of {result.TotalDays} {dayLabel})",
            ConsoleColor.Gray);

        Console.Write("  Difference:      ");
        string diffSign = result.Difference >= 0 ? "+" : "";
        ConsoleColor diffColor = GetStatusColor(result.Status);
        WriteLineColored($"{diffSign}{result.Difference:F1}%  {GetStatusIcon(result.Status)} {GetStatusLabel(result.Status)}", diffColor);

        Console.Write("  Projected EOM:   ");
        ConsoleColor projColor = result.ProjectedUsage switch
        {
            > 110m => ConsoleColor.Red,
            > 100m => ConsoleColor.Yellow,
            >= 85m => ConsoleColor.Green,
            >= 70m => ConsoleColor.Yellow,
            _ => ConsoleColor.Red
        };
        WriteLineColored($"{result.ProjectedUsage:F1}%", projColor);

        Console.WriteLine();

        RenderProgressBar(result);

        Console.WriteLine();

        RenderAdvice(result);

        Console.WriteLine();
    }

    private static void RenderProgressBar(PaceResult result)
    {
        int actualFill = (int)Math.Clamp(result.ActualUsage / 100m * BarWidth, 0, BarWidth);
        int expectedPos = (int)Math.Clamp(result.ExpectedUsage / 100m * BarWidth, 0, BarWidth);

        Console.Write("  [");

        for (int i = 0; i < BarWidth; i++)
        {
            if (i < actualFill)
            {
                ConsoleColor color = GetStatusColor(result.Status);
                WriteColored("\u2588", color);
            }
            else if (i == expectedPos)
            {
                WriteColored("|", ConsoleColor.DarkYellow);
            }
            else
            {
                WriteColored("\u2591", ConsoleColor.DarkGray);
            }
        }

        Console.Write("] ");
        Console.WriteLine($"{result.ActualUsage:F0}%");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"  ");
        Console.Write(new string(' ', expectedPos + 1));
        Console.WriteLine($"^ expected ({result.ExpectedUsage:F0}%)");
        Console.ResetColor();
    }

    private static void RenderAdvice(PaceResult result)
    {
        (string icon, ConsoleColor color, string message) = result.Status switch
        {
            PaceStatus.WellAhead => ("!!", ConsoleColor.Red,
                "You're significantly ahead of pace. Consider slowing down to avoid exceeding your budget."),
            PaceStatus.Ahead => ("!", ConsoleColor.Yellow,
                "You're ahead of pace. Keep an eye on your usage to stay within budget."),
            PaceStatus.OnTrack => ("OK", ConsoleColor.Green,
                "You're on track. Keep up the current pace to fully utilise your budget."),
            PaceStatus.Behind => ("!", ConsoleColor.Yellow,
                "You're behind pace. You have room to use Copilot more — don't leave budget on the table."),
            PaceStatus.WellBehind => ("!!", ConsoleColor.Red,
                "You're significantly behind pace. You're at risk of under-utilising your budget."),
            _ => ("?", ConsoleColor.Gray, "")
        };

        Console.Write("  ");
        WriteColored($"  {icon}  ", color);
        Console.WriteLine(message);
    }

    private static ConsoleColor GetStatusColor(PaceStatus status) => status switch
    {
        PaceStatus.WellAhead => ConsoleColor.Red,
        PaceStatus.Ahead => ConsoleColor.Yellow,
        PaceStatus.OnTrack => ConsoleColor.Green,
        PaceStatus.Behind => ConsoleColor.Yellow,
        PaceStatus.WellBehind => ConsoleColor.Red,
        _ => ConsoleColor.Gray
    };

    private static string GetStatusIcon(PaceStatus status) => status switch
    {
        PaceStatus.WellAhead => "!!",
        PaceStatus.Ahead => "!",
        PaceStatus.OnTrack => "OK",
        PaceStatus.Behind => "!",
        PaceStatus.WellBehind => "!!",
        _ => "?"
    };

    private static string GetStatusLabel(PaceStatus status) => status switch
    {
        PaceStatus.WellAhead => "WELL AHEAD",
        PaceStatus.Ahead => "AHEAD",
        PaceStatus.OnTrack => "ON TRACK",
        PaceStatus.Behind => "BEHIND",
        PaceStatus.WellBehind => "WELL BEHIND",
        _ => "UNKNOWN"
    };

    private static void WriteColored(string text, ConsoleColor color)
    {
        ConsoleColor prev = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = prev;
    }

    private static void WriteLineColored(string text, ConsoleColor color)
    {
        WriteColored(text, color);
        Console.WriteLine();
    }
}
