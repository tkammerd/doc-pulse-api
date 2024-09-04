using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Pulse.DatabaseLoader;

public static class ParsingHelpers
{
    public static string TrimPreventNull(string? value, string? valueName = default) => TrimValue(value, valueName, TrimOptions.ExceptionOnNull)!;
    public static string? TrimAllowNull(string? value) => TrimValue(value, default, TrimOptions.AllowNull);
    public static string TrimWithDefault(string? value, string defaultValue = "") => TrimValue(value, default, TrimOptions.AllowNull) ?? defaultValue;

    public static string? TrimValue(string? value, string? valueName = default, TrimOptions options = TrimOptions.AllowNull)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "NULL")
            return (options == TrimOptions.AllowNull) ? null : throw new Exception($"Error on TrimValue {valueName ?? "value"} Cannot be null.");

        return value.Trim();
    }

    public static DateTimeOffset ParseTimePreventNull(string? value, string? valueName = default) => ParseTime(value, valueName, TrimOptions.ExceptionOnNull)!.Value;
    public static DateTimeOffset? ParseTimeAllowNull(string? value) => ParseTime(value, default, TrimOptions.AllowNull);
    public static DateTimeOffset ParseTimeWithOptions(string? value, TimeDefaultOptions defaultOptions = TimeDefaultOptions.MinValue) => ParseTime(value, default, TrimOptions.AllowNull, (defaultOptions == TimeDefaultOptions.MinValue) ? DateTimeOffset.MinValue : DateTimeOffset.MaxValue)!.Value;
    public static DateTimeOffset ParseTimeWithDefault(string? value, DateTimeOffset? defaultValue) => ParseTime(value, default, TrimOptions.AllowNull, defaultValue ?? DateTimeOffset.MaxValue) ?? defaultValue ?? DateTimeOffset.MaxValue;

    public static DateTimeOffset? ParseTime(string? value, string? valueName = default, TrimOptions options = TrimOptions.AllowNull, DateTimeOffset? defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "NULL")
            return (options == TrimOptions.AllowNull) ? defaultValue : throw new Exception($"Error on ParseTime {valueName ?? "value"} Cannot be null.");

        return DateTimeOffset.Parse(value);
    }
}

public enum TrimOptions
{
    AllowNull, ExceptionOnNull
}

public enum TimeDefaultOptions
{
    MaxValue, MinValue
}
