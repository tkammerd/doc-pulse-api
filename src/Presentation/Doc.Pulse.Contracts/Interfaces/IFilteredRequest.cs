namespace Doc.Pulse.Contracts.Interfaces;

public interface IFilteredRequest
{
    public string? SortBy { get; set; }
    public string? Filter { get; set; }
    public bool? SortDesc { get; set; }
}