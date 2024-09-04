using System.Text.Json.Nodes;

namespace Doc.Pulse.Core.Helpers;

public static class StringHelpers
{
    public static bool IsValidJson(this string source)
    {
        if (string.IsNullOrWhiteSpace(source)) { return false; }
        source = source.Trim();

        if (source.StartsWith("{") && source.EndsWith("}") || source.StartsWith("[") && source.EndsWith("]"))
        {
            try
            {
                _ = JsonNode.Parse(source);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
