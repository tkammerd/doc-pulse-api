namespace Doc.Pulse.Contracts.Communications.V1.Agencies.Queries;

public class AgencyGetByIdResponse
{
    public int? Id { get; set; } = default!;

    public string AgencyName { get; set; } = null!;
    public bool Inactive { get; set; } = false;
    public byte[]? RowVersion { get; set; } = null;
}
