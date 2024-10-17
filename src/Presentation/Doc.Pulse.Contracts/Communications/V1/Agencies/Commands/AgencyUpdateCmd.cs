using Doc.Pulse.Contracts.Communications.V1.Agencies.Queries;

namespace Doc.Pulse.Contracts.Communications.V1.Agencies.Commands;

public class AgencyUpdateCmd
{
    public int Id { get; set; }
    public string AgencyName { get; set; } = null!;
    public bool Inactive { get; set; } = false;
    public byte[]? RowVersion { get; set; }
}