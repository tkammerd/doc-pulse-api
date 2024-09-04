namespace Ots.AppDmDoc.Abstractions.HashIds;

public class HashIdsAdaptorConfig
{
    public string Salt { get; set; } = default!;
    public int MinHashLength { get; set; } = 5;
    public int MaxHashLength { get; set; } = 256;
}
