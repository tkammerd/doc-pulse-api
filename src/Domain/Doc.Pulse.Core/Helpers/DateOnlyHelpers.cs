namespace Doc.Pulse.Core.Helpers;

public static class DateOnlyHelpers
{
    public static DateOnly? ParseOrDefault(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString)) return default;

        return DateOnly.Parse(dateString);
    }
}
