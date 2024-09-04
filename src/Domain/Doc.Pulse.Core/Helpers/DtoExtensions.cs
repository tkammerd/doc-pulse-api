using System.Text.Json;

namespace Doc.Pulse.Core.Helpers;

public static class DtoExtensions
{
    public static T? CloneObjectWithJson<T>(this T? dto) where T : class
    {
        if (dto == null)
            return null;

        var bytes = JsonSerializer.SerializeToUtf8Bytes(dto, new JsonSerializerOptions { WriteIndented = false, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });

        var data = JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(bytes));

        return data;
    }
}
