using System.Text.RegularExpressions;

namespace Doc.Pulse.Api.Extensions;

public static class StringHandlingExtensions
{
    public static string Truncate(this string source, int length)
    {
        if (source.Length > length)
        {
            source = source.Substring(0, length);
        }
        return source;
    }

    public static string SplitCamelCase(this string str)
    {
        return Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
    }
}

