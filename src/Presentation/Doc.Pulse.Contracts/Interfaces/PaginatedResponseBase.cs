namespace Doc.Pulse.Contracts.Interfaces;

public class PaginatedResponseBase : IPaginatedResponse
{
    public int CountTotal { get; set; }
    public int CountAvailable { get; set; }
}
