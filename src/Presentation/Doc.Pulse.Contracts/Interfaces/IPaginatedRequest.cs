namespace Doc.Pulse.Contracts.Interfaces;

public interface IPaginatedRequest : IFilteredRequest
{
    public int? SkipAmount { get; set; }
    public int? TakeAmount { get; set; }
    //public string? SortBy { get; set; }
    //public string? Filter { get; set; }
    //public bool? SortDesc { get; set; }
}
