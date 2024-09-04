namespace Doc.Pulse.Contracts.Interfaces;

public interface IPaginatedResponse
{
    public int CountTotal { get; set; }
    public int CountAvailable { get; set; }
}