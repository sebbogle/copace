Argument<decimal> usageArgument = new("usage")
{
    Description = "Your current Copilot Premium usage percentage (e.g. 42.5)."
};

usageArgument.Validators.Add(result =>
{
    decimal usage = result.GetValue(usageArgument);
    if (usage is < 0m or > 100m)
    {
        result.AddError("Usage percentage must be between 0 and 100.");
    }
});

Option<bool> includeWeekendsOption = new("--include-weekends", "-w")
{
    Description = "Include weekend days in the pace calculation (default: weekdays only)."
};

Option<DateOnly?> dateOption = new("--date", "-d")
{
    Description = "Override today's date for testing (format: yyyy-MM-dd)."
};

RootCommand rootCommand = new("Copace — Track your GitHub Copilot Premium usage pace.")
{
    usageArgument,
    includeWeekendsOption,
    dateOption
};

rootCommand.SetAction((ParseResult parseResult) =>
{
    decimal usage = parseResult.GetValue(usageArgument);
    bool includeWeekends = parseResult.GetValue(includeWeekendsOption);
    DateOnly? date = parseResult.GetValue(dateOption);

    PaceResult result = PaceCalculator.Calculate(usage, includeWeekends, date);
    ConsoleRenderer.Render(result);
});

ParseResult parsedResult = rootCommand.Parse(args);
return parsedResult.Invoke();
